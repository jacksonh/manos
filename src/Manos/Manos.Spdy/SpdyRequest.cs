using System;
using System.Text;
using System.Collections.Generic;

using Manos.IO;
using Manos.Http;
using Manos.Collections;

namespace Manos.Spdy
{
	public class SpdyRequest : SpdyEntity, IHttpRequest, IHttpDataRecipient
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
		private byte [] rawdata;

		public SpdyRequest (Context context,SynStreamFrame frame,byte [] dat = null) : base(context)
		{
			var version = frame.Headers ["version"];
			var num = version.Split ('/') [1];
			var numsplit = num.Split ('.');
			this.MajorVersion = int.Parse (numsplit [0]);
			this.MinorVersion = int.Parse (numsplit [1]);
			this.headers = frame.Headers.ToHttpHeaders (new string[] { "version", "url" });
			this.Path = frame.Headers ["url"];
			this.Method = MethodFromString (frame.Headers ["method"]);
			this.StreamID = frame.StreamID;
			string ct;
			if (dat != null && dat.Length > 0) {
				this.rawdata = dat;
				if (!Headers.TryGetValue ("Content-Type", out ct)) {
					body_handler = new HttpBufferedBodyHandler ();
				} else {
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
				context.CreateTimerWatcher (new TimeSpan (1), () => {
					body_handler.HandleData (this, new ByteBuffer (rawdata, 0, rawdata.Length), 0, rawdata.Length);
					body_handler.Finish (this);
				}).Start ();
			}
		}

		#region IHttpRequest implementation

		public void Read (Action onClose)
		{
			onClose ();
		}

		public void SetWwwFormData (Manos.Collections.DataDictionary data)
		{
			PostData = data;
		}

		public void WriteMetadata (System.Text.StringBuilder builder)
		{
			throw new NotImplementedException ("WriteMetadata");
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
		public override void WriteToBody (byte[] data, int position, int length)
		{
			throw new NotImplementedException ();
		}



		#endregion

		Dictionary<string, HttpMethod> lookup = new Dictionary<string, HttpMethod> () {
			{ "OPTIONS", HttpMethod.HTTP_OPTIONS },
			{ "GET", HttpMethod.HTTP_GET },
			{ "HEAD", HttpMethod.HTTP_HEAD },
			{ "POST", HttpMethod.HTTP_POST },
			{ "PUT", HttpMethod.HTTP_PUT },
			{ "DELETE", HttpMethod.HTTP_DELETE },
			{ "TRACE", HttpMethod.HTTP_TRACE },
			{ "CONNECT", HttpMethod.HTTP_CONNECT }
		};

		HttpMethod MethodFromString (string str)
		{
			str = str.ToUpper ();
			if (lookup.ContainsKey (str)) {
				return lookup [str];
			} else {
				return HttpMethod.ERROR;
			}
		}
	}
}

