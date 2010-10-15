using System;

using Manos.Server;

namespace Manos.Testing
{
	public class ManosContextStub : IManosContext
	{
		public ManosContextStub ()
		{
		}

		public HttpServer Server {
			get {
				throw new NotImplementedException ();
			}
		}

		public IHttpTransaction Transaction {
			get {
				throw new NotImplementedException ();
			}
		}

		public IHttpRequest Request {
			get {
				throw new NotImplementedException ();
			}
		}

		public IHttpResponse Response {
			get {
				throw new NotImplementedException ();
			}
		}		
	}
}

