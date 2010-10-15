

using System;

using Manos.Server;

namespace Manos {

	public class ManosContext : IManosContext {

		public ManosContext (IHttpTransaction transaction)
		{
			Transaction = transaction;
		}

		public HttpServer Server {
			get { return Transaction.Server; }
		}

		public IHttpTransaction Transaction {
			get;
			private set;
		}

		public IHttpRequest Request {
			get { return Transaction.Request; }
		}

		public IHttpResponse Response {
			get { return Transaction.Response; }
		}
	}
}
