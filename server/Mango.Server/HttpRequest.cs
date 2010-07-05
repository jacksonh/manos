


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

	public class HttpRequest {

		public HttpRequest (HttpConnection connection, HttpHeaders headers, string method, string resource, bool support_1_1)
		{
			Connection = connection;
			Headers = headers;
			Method = method;
			ResourceUri = resource;
			Http_1_1_Supported = support_1_1;

			SetEncoding ();
			SetPathAndQuery ();
		}

		public HttpConnection Connection {
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
				Connection.Abort (400, "Invalid resource path. '{0}'", uri);
				return;
			}

			LocalPath = path;
			QueryData = HttpUtility.ParseQueryString (query);
		}

		internal void SetWwwFormData (byte [] data)
		{
			string post = Encoding.ASCII.GetString (data);

			Console.WriteLine ("POST:  {0}", post);
		}

		internal void SetMultiPartFormData (byte [] data)
		{
			string post = Encoding.ASCII.GetString (data);

			Console.WriteLine ("POST:  {0}", post);
		}
	}
}

