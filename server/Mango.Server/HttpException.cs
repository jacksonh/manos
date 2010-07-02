
using System;

namespace Mango.Server
{
	public class HttpException : Exception
	{
		public HttpException (string message) : base (message)
		{
		}
	}
}
