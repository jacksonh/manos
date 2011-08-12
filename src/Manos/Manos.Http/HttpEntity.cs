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
		private bool finished_reading;

		private IAsyncWatcher end_watcher;

		public HttpEntity (Context context)
		{
			this.Context = context;
			end_watcher = context.CreateAsyncWatcher (HandleEnd);
			end_watcher.Start ();
		}
		
		public Context Context {
			get;
			private set;
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

		public ITcpSocket Socket {
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

			if (properties == null)
				properties = new Dictionary<string,object> ();

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
			// Upgrade connections will raise this event at the end of OnBytesRead
			if (!parser.Upgrade) 
				OnFinishedReading (parser);
			finished_reading = true;
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
			uploaded_files = null;
			finished_reading = false;

			if (parser_settings == null)
				CreateParserSettingsInternal ();

			parser = new HttpParser ();
		}
		
		public void Read ()
		{
			Read (() => {});
		}

		public void Read (Action onClose)
		{
			Reset ();
			Socket.GetSocketStream ().Read (OnBytesRead, (obj) => {}, onClose);
		}

		private void OnBytesRead (ByteBuffer bytes)
		{
			try {
				parser.Execute (parser_settings, bytes);
			} catch (Exception e) {
				Console.WriteLine ("Exception while parsing");
				Console.WriteLine (e);
			}

			if (finished_reading && parser.Upgrade) {

				//
				// Well, this is a bit of a hack.  Ideally, maybe there should be a putback list
				// on the socket so we can put these bytes back into the stream and the upgrade
				// protocol handler can read them out as if they were just reading normally.
				//

				if (bytes.Position < bytes.Length) {
					byte [] upgrade_head = new byte [bytes.Length - bytes.Position];
					Array.Copy (bytes.Bytes, bytes.Position, upgrade_head, 0, upgrade_head.Length);

					SetProperty ("UPGRADE_HEAD", upgrade_head);
				}

				// This is delayed until here with upgrade connnections.
				OnFinishedReading (parser);
			}
		}

		protected virtual void OnFinishedReading (HttpParser parser)
		{
			if (body_handler != null) {
				body_handler.Finish (this);
				body_handler = null;
			}

			if (OnCompleted != null)
				OnCompleted ();
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

		internal virtual void HandleEnd ()
		{
			if (OnEnd != null)
				OnEnd ();
		}

		public void Complete (Action callback)
		{
			IAsyncWatcher completeWatcher = null;
			completeWatcher = Context.CreateAsyncWatcher (delegate {
				completeWatcher.Dispose ();
				callback ();
			});
			completeWatcher.Start ();
			Stream.End (completeWatcher.Send);
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

		public byte [] GetBody ()
		{
			StringBuilder data = null;

			if (PostBody != null) {
				data = new StringBuilder ();
				data.Append (PostBody);
			}

			if (post_data != null) {
				data = new StringBuilder ();
				bool first = true;
				foreach (string key in post_data.Keys) {
					if (!first)
						data.Append ('&');
					first = false;

					UnsafeString s = post_data.Get (key);
					if (s != null) {
						data.AppendFormat ("{0}={1}", key, s.UnsafeValue);
						continue;
					}
				}
			}

			if (data == null)
				return null;

			return ContentEncoding.GetBytes (data.ToString ());
			
		}

		public abstract void WriteMetadata (StringBuilder builder);
		public abstract ParserSettings CreateParserSettings ();

		public event Action<string> Error; // TODO: Proper error object of some sort

		public event Action OnEnd;
		public event Action OnCompleted;
	}

}


