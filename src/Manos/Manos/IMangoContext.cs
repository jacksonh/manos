
using System;

using Mango.Server;

namespace Mango
{
	public interface IMangoContext
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
