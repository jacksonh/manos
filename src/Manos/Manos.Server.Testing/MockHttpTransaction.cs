
using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;


namespace Manos.Server.Testing
{
	public class MockHttpTransaction : IHttpTransaction
	{
		private bool aborted;

		public MockHttpTransaction (IHttpRequest request)
		{
			if (request == null)
			   throw new ArgumentNullException ("request");

			Request = request;
			Response = new HttpResponse (this, Encoding.Default);
		}

		// TODO: I guess we need a mock server?
		public HttpServer Server {
			get { return null; }
		}
				
		public IHttpRequest Request {
			get;
			private set;
		}

		public IHttpResponse Response {
			get;
			private set;
		}

		public bool Aborted {
			get;
			private set;
		}

		public int AbortedStatusCode {
		       get;
		       private set;
		}

		public string AbortedMessage {
		       get;
		       private set;
		}

		public bool Finished {
		       get;
		       private set;
		}

		public void Finish ()
		{
			Finished = true;
		}
		
		public void Abort (int status, string message, params object [] p)
		{
			Aborted = true;
			AbortedStatusCode = status;
			AbortedMessage = String.Format (message, p);
		}

		public void Write (List<ArraySegment<byte>> data)
		{
		}

		public void SendFile (string file)
		{
		}
	}
}
