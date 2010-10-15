

using System;
using System.Collections;
using System.Collections.Generic;


namespace Manos.Server {

	public interface IHttpTransaction {

		IHttpRequest Request {
			get;
		}

		IHttpResponse Response {
			get;
		}

		HttpServer Server {
			get;
		}

		bool Aborted {
			get;	
		}
		
		void Write (List<ArraySegment<byte>> data);

		void SendFile (string file);

		void Finish ();
		
		void Abort (int status, string message, params object [] p);
	}
}

