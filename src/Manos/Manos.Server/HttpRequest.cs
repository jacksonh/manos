


using System;
using System.IO;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;


using Mono.Unix.Native;

using Manos.Collections;

namespace Manos.Server {

	public class HttpRequest : IHttpRequest {
		
		private DataDictionary cookies;
		private Dictionary<string,UploadedFile> uploaded_files;
		
		public HttpRequest (IHttpTransaction transaction, HttpHeaders headers, string method, string resource, bool support_1_1)
		{
			Transaction = transaction;
			Headers = headers;
			Method = method;
			ResourceUri = resource;
			Http_1_1_Supported = support_1_1;

			Data = new DataDictionary ();
			UriData = new DataDictionary ();
			QueryData = new DataDictionary ();
			PostData = new DataDictionary ();
			
			Data.Children.Add (UriData);
			Data.Children.Add (QueryData);
			Data.Children.Add (PostData);
					
			SetEncoding ();
			SetPathAndQuery ();
		}

		public IHttpTransaction Transaction {
			get;
			private set;
		}

		public HttpHeaders Headers {
			get;
			private set;
		}

		public string Method {
			get;
			private set;
		}

		public string ResourceUri {
			get;
			private set;
		}

		public bool Http_1_1_Supported {
			get;
			private set;
		}

		public string LocalPath {
			get;
			private set;
		}
		
		public DataDictionary Data {
			get;
			private set;
		}
		
		public DataDictionary PostData {
			get;
			private set;
		}

		public DataDictionary QueryData {
			get;
			private set;
		}

		public DataDictionary UriData {
			get;
			private set;
		}
		
		public DataDictionary Cookies {
			get {
				if (cookies == null)
					cookies = ParseCookies ();
				return cookies;
			}
		}
		
		public Dictionary<string,UploadedFile> Files {
			get {
			    if (uploaded_files == null)
			       uploaded_files = new Dictionary<string,UploadedFile> ();
			    return uploaded_files;
			}
		}

		public Encoding ContentEncoding {
			get;
			private set;
		}

		private void SetEncoding ()
		{
			string content;

			if (!Headers.TryGetValue ("Content-Type", out content)) {
				ContentEncoding = Encoding.ASCII;
				return;
			}

			// TODO
			ContentEncoding = Encoding.ASCII;
		}

		private void SetPathAndQuery ()
		{
			// This is used with the OPTIONS verb
			if (ResourceUri == "*")
				return;

			string uri = ResourceUri;
			string scheme;
			string host;
			string path;
			string query;

			if (!UriParser.TryParse (uri, out scheme, out host, out path, out query)) {
				Transaction.Abort (400, "Invalid resource path. '{0}'", uri);
				return;
			}

			LocalPath = path;
			// TODO: Pass this to the encoder to populate
			DataDictionary query_data = HttpUtility.ParseUrlEncodedData (query);
			if (query_data != null)
				QueryData.Children.Add (query_data);
		}

		private DataDictionary ParseCookies ()
		{
			string cookie_header = Headers ["Cookie"];
			
			if (cookie_header == null)
				return new DataDictionary ();
			
			return HttpCookie.FromHeader (cookie_header);
		}
		
		public void SetWwwFormData (DataDictionary data)
		{
		       if (data == null) {
		       	  PostData.Clear ();
			  return;
		       }
		       PostData.Children.Add (data);
		}
	}
}

