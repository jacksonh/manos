using System;
using Manos.Http;

namespace Manos.Spdy
{
	public class SpdyTransaction : IHttpTransaction
	{
		public SpdyTransaction ()
		{
		}

		#region IHttpTransaction implementation
		public void OnRequestReady ()
		{
			throw new NotImplementedException ();
		}

		public void OnResponseFinished ()
		{
			throw new NotImplementedException ();
		}

		public void Abort (int status, string message, params object[] p)
		{
			throw new NotImplementedException ();
		}

		public Manos.IO.Context Context {
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

		public HttpServer Server {
			get {
				throw new NotImplementedException ();
			}
		}

		public bool Aborted {
			get {
				throw new NotImplementedException ();
			}
		}

		public bool ResponseReady {
			get {
				throw new NotImplementedException ();
			}
		}
		#endregion
	}
}

