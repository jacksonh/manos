

using System;

using Mango.Server;

namespace Mango {

	public class MangoContext : IMangoContext {

		public MangoContext (IHttpTransaction transaction)
		{
			Transaction = transaction;
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
