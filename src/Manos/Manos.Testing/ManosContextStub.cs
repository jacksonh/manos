using System;


namespace Manos.Testing
{
	public class ManosContextStub : IManosContext
	{
		public ManosContextStub ()
		{
		}
		
		public Server.IHttpTransaction Transaction {
			get {
				throw new NotImplementedException ();
			}
		}

		public Server.IHttpRequest Request {
			get {
				throw new NotImplementedException ();
			}
		}

		public Server.IHttpResponse Response {
			get {
				throw new NotImplementedException ();
			}
		}		
	}
}

