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
using Manos.Http;

namespace Manos.Spdy {

	/// <summary>
	///  A base class for SpdyRequest and SpdyResponse.  Generally user code should not care at all about
	///  this class, it just exists to eliminate some code duplication between the two derived types.
	/// </summary>
	public abstract class SpdyEntity : IDisposable, IHttpDataRecipient {

		private static readonly long MAX_BUFFERED_CONTENT_LENGTH = 2621440; // 2.5MB (Eventually this will be an environment var)

		private HttpHeaders headers;

		private DataDictionary data;
		private DataDictionary post_data;

		private Dictionary<string,object> properties;
		private Dictionary<string,UploadedFile> uploaded_files;

		private IHttpBodyHandler body_handler;
		private bool finished_reading;


		public SpdyEntity (Context context)
		{
			this.Context = context;
		}
		
		public Context Context {
			get;
			private set;
		}

		~SpdyEntity ()
		{
			Dispose ();
		}

		public void Dispose ()
		{
			Socket = null;
		}

		public Socket Socket {
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

		internal IUploadedFileCreator GetFileCreator ()
		{
			if (Headers.ContentLength == null || Headers.ContentLength >= MAX_BUFFERED_CONTENT_LENGTH)
				return new TempFileUploadedFileCreator ();
			return new InMemoryUploadedFileCreator ();
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
			HandleEnd();
		}

		internal virtual void HandleEnd ()
		{
			if (OnEnd != null)
				OnEnd ();
		}

		public void Complete (Action callback)
		{
			IAsyncWatcher completeWatcher;
			completeWatcher = Context.CreateAsyncWatcher (delegate {
				completeWatcher.Dispose ();
				callback ();
			});
			completeWatcher.Start ();
		}

		public void WriteLine (string str)
		{
			Write (str + Environment.NewLine);	
		}
		
		public void WriteLine (string str, params object [] prms)
		{
			WriteLine (String.Format (str, prms));	
		}

		public event Action<string> Error; // TODO: Proper error object of some sort

		public event Action OnEnd;
		public event Action OnCompleted;
		public abstract void WriteToBody(byte[] data, int position, int length);
	}

}


