

using System;
using System.Collections;
using System.Collections.Generic;


namespace Mango.Server {

	public interface IHttpTransaction {

		HttpRequest Request {
			get;
		}

		HttpResponse Response {
			get;
		}

		void Write (List<ArraySegment<byte>> data);

		void SendFile (string file);

		void Finish ();
		
		void Abort (int status, string message, params object [] p);
	}
}

