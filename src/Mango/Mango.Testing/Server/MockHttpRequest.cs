
using System;

using Mango;
using Mango.Server;

namespace Mango.Testing.Server
{


	public class MockHttpRequest : IHttpRequest
	{
		public MockHttpRequest (string method, string local_path)
		{
			Method = method;
			LocalPath = local_path;
		}
		
		public string Method {
			get;
			private set;
		}
		
		
		public string LocalPath {
			get;
			private set;
		}
	}
}
