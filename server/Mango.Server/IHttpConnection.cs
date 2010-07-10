

using System;


namespace Mango.Server {

	public interface IHttpConnection {

		HttpRequest Request {
			get;
		}

		HttpResponse Response {
			get;
		}
	}
}

