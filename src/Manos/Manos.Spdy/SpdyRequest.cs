using System;
using System.Text;
using System.Collections.Generic;

using Manos.IO;
using Manos.Http;
using Manos.Collections;

namespace Manos.Spdy
{
	public class SpdyRequest : IHttpRequest, IHttpDataRecipient
	{
		private static readonly long MAX_BUFFERED_CONTENT_LENGTH = 2621440; // 2.5MB (Eventually this will be an environment var)
		private HttpHeaders headers;
		public int StreamID { get; set; }
		private IHttpBodyHandler body_handler;
		private Dictionary<string, UploadedFile> uploaded_files;
		private Dictionary<string, object> properties;
		
		private DataDictionary uri_data;
		private	DataDictionary query_data;
		private DataDictionary cookies;
		private DataDictionary post_data;
		private	DataDictionary data;
		private byte[] rawdata;
		
		public SpdyRequest (Context context, SynStreamFrame frame, byte[] dat = null)
		{
			var version = frame.Headers["version"];
			var num = version.Split('/')[1];
			var numsplit = num.Split('.');
			this.MajorVersion = int.Parse(numsplit[0]);
			this.MinorVersion = int.Parse(numsplit[1]);
			this.headers = frame.Headers.ToHttpHeaders(new string[] { "version", "url" });
			this.Path = frame.Headers["url"];
			this.Method = MethodFromString(frame.Headers["method"]);
			this.StreamID = frame.StreamID;
			string ct;
			if (dat != null  && dat.Length > 0)
			{
				Console.WriteLine(dat.ToString());
				this.rawdata = dat;
				if (!Headers.TryGetValue ("Content-Type", out ct)) {
					body_handler = new HttpBufferedBodyHandler ();
				}
				else {
					if (ct.StartsWith ("application/x-www-form-urlencoded", StringComparison.InvariantCultureIgnoreCase)) {
						body_handler = new HttpFormDataHandler ();
					}
		
					if (ct.StartsWith ("multipart/form-data", StringComparison.InvariantCultureIgnoreCase)) {
						string boundary = HttpRequest.ParseBoundary (ct);
						IUploadedFileCreator file_creator = GetFileCreator ();
						body_handler = new HttpMultiPartFormDataHandler (boundary, ContentEncoding, file_creator);
					}
				}
				if (body_handler == null)
					body_handler = new HttpBufferedBodyHandler ();
				context.CreateTimerWatcher(new TimeSpan(1), () => {
					body_handler.HandleData(this, new ByteBuffer(rawdata, 0, rawdata.Length),0, rawdata.Length);
					body_handler.Finish(this);
				}).Start();
			}
		}
		private IUploadedFileCreator GetFileCreator ()
		{
			if (Headers.ContentLength == null || Headers.ContentLength >= MAX_BUFFERED_CONTENT_LENGTH)
				return new TempFileUploadedFileCreator ();
			return new InMemoryUploadedFileCreator ();
		}

		#region IHttpRequest implementation
		
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
		
		public void Read (Action onClose)
		{
			onClose();
		}

		public void SetWwwFormData (Manos.Collections.DataDictionary data)
		{
			PostData = data;
		}

		public void WriteMetadata (System.Text.StringBuilder builder)
		{
			throw new NotImplementedException ("WriteMetadata");
		}

		public HttpMethod Method { get; set; }

		public string Path { get; set; }

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
		protected void SetDataDictionary (DataDictionary old, DataDictionary newd)
		{
			if (data != null && old != null)
				data.Children.Remove (old);
			if (newd != null)
				Data.Children.Add (newd);
		}

		public DataDictionary QueryData {
			get {
				if (query_data == null) {
					query_data = new DataDictionary ();
					Data.Children.Add (query_data);
				}
				return query_data;
			}
			set {
				SetDataDictionary (query_data, value);
				query_data = value;
			}
		}

		public DataDictionary UriData {
			get {
				if (uri_data == null) {
					uri_data = new DataDictionary ();
					Data.Children.Add (uri_data);
				}
				return uri_data;
			}
			set {
				SetDataDictionary (uri_data, value);
				uri_data = value;
			}
		}

		public DataDictionary Cookies {
			get {
				if (cookies == null)
					cookies = ParseCookies ();
				return cookies;
			}
		}
		private DataDictionary ParseCookies ()
		{
			string cookie_header;

			if (!Headers.TryGetValue ("Cookie", out cookie_header))
				return new DataDictionary ();

			return HttpCookie.FromHeader (cookie_header);
		}

		public HttpHeaders Headers {
			get {
				return headers;
			}
			set {
				headers = value;
			}
		}

		public Dictionary<string,UploadedFile> Files {
			get {
			    if (uploaded_files == null)
			       uploaded_files = new Dictionary<string,UploadedFile> ();
			    return uploaded_files;
			}
		}

		public int MajorVersion { get; set; }

		public int MinorVersion { get; set; }

		public Encoding ContentEncoding {
			get {
				return Headers.ContentEncoding;
			}
			set {
				Headers.ContentEncoding = value;
			}
		}

		public Socket Socket {
			get {
				throw new NotImplementedException("Socket");
			}
		}

		public string PostBody { get; set; }
		#endregion
		
		Dictionary<string, HttpMethod> lookup = new Dictionary<string, HttpMethod>() {
			{ "OPTIONS", HttpMethod.HTTP_OPTIONS },
			{ "GET", HttpMethod.HTTP_GET },
			{ "HEAD", HttpMethod.HTTP_HEAD },
			{ "POST", HttpMethod.HTTP_POST },
			{ "PUT", HttpMethod.HTTP_PUT },
			{ "DELETE", HttpMethod.HTTP_DELETE },
			{ "TRACE", HttpMethod.HTTP_TRACE },
			{ "CONNECT", HttpMethod.HTTP_CONNECT }
		};
		HttpMethod MethodFromString(string str)
		{
			str = str.ToUpper();
			if (lookup.ContainsKey(str)) {
				return lookup[str];
			} else {
				return HttpMethod.ERROR;
			}
		}
		
		#region IDisposable implementation
		public void Dispose ()
		{
		}
		#endregion
	}
}

