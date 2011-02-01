//
// Copyright (C) 2010 Jackson Harper (jackson@manosdemono.com)
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//
//




using System;
using System.IO;
using System.Text;
using System.Net;
using System.Globalization;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;


using Libev;
using Manos.IO;
using Manos.Collections;

namespace Manos.Http {

	/// <summary>
	///  A base class for HttpRequest and HttpResponse.  Generally user code should not care at all about
	///  this class, it just exists to eliminate some code duplication between the two derived types.
	/// </summary>
	public abstract class HttpEntity : IDisposable {

		private static readonly long MAX_BUFFERED_CONTENT_LENGTH = 2621440; // 2.5MB (Eventually this will be an environment var)

		private HttpHeaders headers;

		private HttpParser parser;
		private ParserSettings parser_settings;
		private StringBuilder current_header_field = new StringBuilder ();
		private StringBuilder current_header_value = new StringBuilder ();

		private DataDictionary data;
		private DataDictionary post_data;

		private Dictionary<string,object> properties;
		private Dictionary<string,UploadedFile> uploaded_files;

		private IHttpBodyHandler body_handler;

		private AsyncWatcher end_watcher;

		public HttpEntity ()
		{
			end_watcher = new AsyncWatcher (IOLoop.Instance.EventLoop, OnEnd);
			end_watcher.Start ();
		}

		~HttpEntity ()
		{
			Dispose ();
		}

		public void Dispose ()
		{
			Socket = null;

			if (Stream != null) {
				Stream.Dispose ();
				Stream = null;
			}

			if (end_watcher != null) {
				end_watcher.Dispose ();
				end_watcher = null;
			}
		}

		public SocketStream Socket {
			get;
			protected set;
		}

		public HttpStream Stream {
			get;
			protected set;
		}

		public HttpHeaders Headers {
			get {
				if (headers == null)
					headers = new HttpHeaders ();
				return headers;
			}
			set {
				headers = value;
			}
		}

		public HttpMethod Method {
			get;
			set;
		}

		public int MajorVersion {
			get;
			set;
		}

		public int MinorVersion {
			get;
			set;
		}

		public string RemoteAddress {
			get;
			set;
		}

		public int RemotePort {
			get;
			set;
		}

		public string Path {
			get;
			set;
		}

		public bool StreamBody {
			get;
			set;
		}

		public Encoding ContentEncoding {
			get { return Headers.ContentEncoding; }
			set { Headers.ContentEncoding = value; }
		}

		
		public DataDictionary Data {
			get {
				if (data == null)
					data = new DataDictionary ();
				return data;
			}
		}

		public DataDictionary PostData {
			get {
				if (post_data == null) {
					post_data = new DataDictionary ();
					Data.Children.Add (post_data);
				}
				return post_data;
			}
			set {
				SetDataDictionary (post_data, value);
				post_data = value;
			}
		}

		public string PostBody {
			get;
			set;
		}

		public Dictionary<string,UploadedFile> Files {
			get {
			    if (uploaded_files == null)
			       uploaded_files = new Dictionary<string,UploadedFile> ();
			    return uploaded_files;
			}
		}

		public Dictionary<string,object> Properties {
			get {
				if (properties == null)
					properties = new Dictionary<string,object> ();
				return properties;
			}
		}

		public void SetProperty (string name, object o)
		{
			if (name == null)
				throw new ArgumentNullException ("name");

			if (o == null && properties == null)
				return;

			if (o == null) {
				properties.Remove (name);
				if (properties.Count == 0)
					properties = null;
				return;
			}

			properties [name] = o;
		}

		public object GetProperty (string name)
		{
			if (name == null)
				throw new ArgumentNullException ("name");

			if (properties == null)
				return null;

			object res = null;
			if (!properties.TryGetValue (name, out res))
				return null;
			return res;
		}

		public T GetProperty<T> (string name)
		{
			object res = GetProperty (name);
			if (res == null)
				return default (T);
			return (T) res;
		}

		protected void SetDataDictionary (DataDictionary old, DataDictionary newd)
		{
			if (data != null && old != null)
				data.Children.Remove (old);
			if (newd != null)
				Data.Children.Add (newd);
		}

		protected void CreateParserSettingsInternal ()
		{
			this.parser_settings = CreateParserSettings ();

			parser_settings.OnError = OnParserError;

			parser_settings.OnBody = OnBody;
			parser_settings.OnMessageBegin = OnMessageBegin;
			parser_settings.OnMessageComplete = OnMessageComplete;

			parser_settings.OnHeaderField = OnHeaderField;
			parser_settings.OnHeaderValue = OnHeaderValue;
			parser_settings.OnHeadersComplete = OnHeadersComplete;
		}

		private int OnMessageBegin (HttpParser parser)
		{
			return 0;
		}

		private int OnMessageComplete (HttpParser parser)
		{
			OnFinishedReading (parser);
			return 0;
		}

		public int OnHeaderField (HttpParser parser, ByteBuffer data, int pos, int len)
		{
			string str = Encoding.ASCII.GetString (data.Bytes, pos, len);

			if (current_header_value.Length != 0)
				FinishCurrentHeader ();

			current_header_field.Append (str);
			return 0;
		}

		public int OnHeaderValue (HttpParser parser, ByteBuffer data, int pos, int len)
		{
			string str = Encoding.ASCII.GetString (data.Bytes, pos, len);

			if (current_header_field.Length == 0)
				throw new HttpException ("Header Value raised with no header field set.");

			current_header_value.Append (str);
			return 0;
		}

		private void FinishCurrentHeader ()
		{
			try {
				if (headers == null)
					headers = new HttpHeaders ();
				headers.SetHeader (current_header_field.ToString (), current_header_value.ToString ());
				current_header_field.Length = 0;
				current_header_value.Length = 0;
			} catch (Exception e) {
				Console.WriteLine (e);
			}
		}

		protected virtual int OnHeadersComplete (HttpParser parser)
		{
			if (current_header_field.Length != 0)
				FinishCurrentHeader ();

			MajorVersion = parser.Major;
			MinorVersion = parser.Minor;
			Method = parser.HttpMethod;

			return 0;
		}

		public int OnBody (HttpParser parser, ByteBuffer data, int pos, int len)
		{
			if (StreamBody) {
				if (BodyData != null)
					BodyData (data.Bytes, pos, len);
				return 0;
			}

			if (body_handler == null)
				CreateBodyHandler ();

			if (body_handler != null)
				body_handler.HandleData (this, data, pos, len);

			return 0;
		}

		
		private void CreateBodyHandler ()
		{
			string ct;

			if (!Headers.TryGetValue ("Content-Type", out ct)) {
				body_handler = new HttpBufferedBodyHandler ();
				return;
			}

			if (ct.StartsWith ("application/x-www-form-urlencoded", StringComparison.InvariantCultureIgnoreCase)) {
				body_handler = new HttpFormDataHandler ();
				return;
			}

			if (ct.StartsWith ("multipart/form-data", StringComparison.InvariantCultureIgnoreCase)) {
				string boundary = ParseBoundary (ct);
				IUploadedFileCreator file_creator = GetFileCreator ();

				body_handler = new HttpMultiPartFormDataHandler (boundary, ContentEncoding, file_creator);
				return;
			}

			body_handler = new HttpBufferedBodyHandler ();
		}

		private IUploadedFileCreator GetFileCreator ()
		{
			if (Headers.ContentLength == null || Headers.ContentLength >= MAX_BUFFERED_CONTENT_LENGTH)
				return new TempFileUploadedFileCreator ();
			return new InMemoryUploadedFileCreator ();
		}

		private void OnParserError (HttpParser parser, string message, ByteBuffer buffer, int initial_position)
		{
			// Transaction.Abort (-1, "HttpParser error: {0}", message);
			Socket.Close ();
		}

		public virtual void Reset ()
		{
			Path = null;
			ContentEncoding = null;

			headers = null;
			data = null;
			post_data = null;

			if (parser_settings == null)
				CreateParserSettingsInternal ();

			parser = new HttpParser ();
		}

		public void Read ()
		{
			Reset ();
			Socket.ReadBytes (OnBytesRead);
		}

		private void OnBytesRead (IOStream stream, byte [] data, int offset, int count)
		{
			ByteBuffer bytes = new ByteBuffer (data, offset, count);

			try {
				parser.Execute (parser_settings, bytes);
			} catch (Exception e) {
				Console.WriteLine ("Exception while parsing");
				Console.WriteLine (e);
			}
		}

		protected virtual void OnFinishedReading (HttpParser parser)
		{
			if (body_handler != null) {
				body_handler.Finish (this);
				body_handler = null;
			}
		}

		public static string ParseBoundary (string ct)
		{
			if (ct == null)
				return null;

			int start = ct.IndexOf ("boundary=");
			if (start < 1)
				return null;
			
			return ct.Substring (start + "boundary=".Length);
		}

		
		public void Write (string str)
		{
			byte [] data = ContentEncoding.GetBytes (str);

			WriteToBody (data, 0, data.Length);
		}

		public void Write (byte [] data)
		{
			WriteToBody (data, 0, data.Length);
		}

		public void Write (byte [] data, int offset, int length)
		{
			WriteToBody (data, offset, length);
		}

		public void Write (string str, params object [] prms)
		{
			Write (String.Format (str, prms));	
		}

		public void End (string str)
		{
			Write (str);
			End ();
		}

		public void End (byte [] data)
		{
			Write (data);
			End ();
		}

		public void End (byte [] data, int offset, int length)
		{
			Write (data, offset, length);
			End ();
		}

		public void End (string str, params object [] prms)
		{
			Write (str, prms);
			End ();
		}

		public void End ()
		{
			end_watcher.Send ();
		}

		internal virtual void OnEnd (Loop loop, AsyncWatcher watcher, EventTypes revents)
		{
			if (!Stream.Chunked) {
				Headers.ContentLength = Stream.Length;
			}

			Stream.End (null);
		}

		public void WriteLine (string str)
		{
			Write (str + Environment.NewLine);	
		}
		
		public void WriteLine (string str, params object [] prms)
		{
			WriteLine (String.Format (str, prms));	
		}
		
		public void SendFile (string file)
		{
			Stream.SendFile (file);
		}

		private void WriteToBody (byte [] data, int offset, int length)
		{
			Stream.Write (data, offset, length);
		}

		public abstract void WriteMetadata (StringBuilder builder);
		public abstract ParserSettings CreateParserSettings ();

		public event Action<IHttpResponse> Connected;
		public event Action<byte [], int, int> BodyData;

		public event Action<string> Error; // TODO: Proper error object of some sort
	}

}


