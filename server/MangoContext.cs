

using System;

using Mango.Server;

namespace Mango {

	public class MangoContext {

		public MangoContext (HttpConnection connection)
		{
			Connection = connection;
		}

		public HttpConnection Connection {
			get;
			private set;
		}

		public HttpRequest Request {
			get { return Connection.Request; }
		}

		public HttpResponse Response {
			get { return Connection.Response; }
		}
	}
}
