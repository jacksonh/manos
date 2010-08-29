
using System;

namespace Manos.Server
{
	public class HttpException : Exception
	{
		public HttpException (string message) : base (message)
		{
		}
	}
}
