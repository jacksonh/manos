


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
			// TODO: Pass this to the encoder to populate
			DataDictionary query_data = HttpUtility.ParseUrlEncodedData (query);
			if (query_data != null)
				QueryData.Children.Add (query_data);
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

			// TODO: pass this to the encoder to populate
			DataDictionary post_data = HttpUtility.ParseUrlEncodedData (post);
			if (post_data != null)
				PostData.Children.Add (post_data);
		}

		internal void SetMultiPartFormData (byte [] data)
		{
			string post = Encoding.ASCII.GetString (data);

			Console.WriteLine ("MULTIPART:  {0}", post);
		}
	}
}

