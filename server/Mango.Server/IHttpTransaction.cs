

using System;


namespace Mango.Server {

	public interface IHttpTransaction {

		HttpRequest Request {
			get;
		}

		HttpResponse Response {
			get;
		}
	}
}

