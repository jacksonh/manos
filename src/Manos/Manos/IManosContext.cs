
using System;

using Manos.Server;

namespace Manos
{
	public interface IManosContext
	{
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
