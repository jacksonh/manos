
using System;

using Manos.Server;

namespace Manos
{
	public interface IManosContext
	{
		HttpServer Server {
			get;
		}

		IHttpTransaction Transaction {
			get;
		}

		IHttpRequest Request {
			get;
		}

		IHttpResponse Response {
			get;
		}
	}
}
