


using System;
using System.IO;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Collections;
using System.Collections.Generic;

using Mono.Unix.Native;


namespace Mango.Server {

	public class HttpRequest {

		public HttpRequest (HttpConnection connection, HttpHeaders headers, string verb, string path)
		{
			HttpConnection = connection;
			HttpHeaders = headers;
			Verb = verb;
			Path = path;
		}

		public HttpConnection HttpConnection {
			get;
			private set;
		}

		public HttpHeaders HttpHeaders {
			get;
			private set;
		}

		public string Verb {
			get;
			private set;
		}

		public string Path {
			get;
			private set;
		}
	}
}

