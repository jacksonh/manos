


using System;
using System.IO;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;


using Mono.Unix.Native;


namespace Mango.Server {

	public class HttpRequest : IHttpRequest {

		private NameValueCollection uri_data;
		
		public HttpRequest (IHttpTransaction transaction, HttpHeaders headers, string method, string resource, bool support_1_1)
		{
			Transaction = transaction;
			Headers = headers;
			Method = method;
			ResourceUri = resource;
			Http_1_1_Supported = support_1_1;

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
		
		public NameValueCollection PostData {
			get;
			private set;
		}

		public NameValueCollection QueryData {
			get;
			private set;
		}

		public NameValueCollection UriData {
			get {
				if (uri_data == null)
					uri_data = new NameValueCollection ();
				return uri_data;
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
		}

		private void SetPathAndQuery ()
		{
			// This is used with the OPTIONS verb
			if (ResourceUri == "*")
				return;

			string uri = ResourceUri;
			string scheme;
			string path;
			string query;

			if (!UriParser.TryParse (uri, out scheme, out path, out query)) {
				Transaction.Abort (400, "Invalid resource path. '{0}'", uri);
				return;
			}

			LocalPath = path;
			QueryData = HttpUtility.ParseUrlEncodedData (query);
		}

		internal void SetWwwFormData (byte [] data)
		{
			//
			// The best I can tell, you can't actually set the content-type of
			// the url-encoded form data.  Looking at the source of apache
			// seems to confirm this.  So for now I wont worry about the
			// encoding type and I'll just use ASCII.
			//

			string post = Encoding.ASCII.GetString (data);

			PostData = HttpUtility.ParseUrlEncodedData (post);
		}

		internal void SetMultiPartFormData (byte [] data)
		{
			string post = Encoding.ASCII.GetString (data);

			Console.WriteLine ("MULTIPART:  {0}", post);
		}
	}
}

