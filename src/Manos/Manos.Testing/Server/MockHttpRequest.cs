
using System;

using Mango;
using Mango.Server;
using System.Collections.Specialized;

namespace Mango.Server.Testing
{


	public class MockHttpRequest : IHttpRequest
	{
		private NameValueCollection uri_data;
		
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
		
		public NameValueCollection UriData {
			get {
				if (uri_data == null)
					uri_data = new NameValueCollection ();
				return uri_data;
			}
		}
	}
}
