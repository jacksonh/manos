
using System;

using Manos;
using Manos.Server;
using System.Collections.Specialized;
using Manos.Collections;

namespace Manos.Server.Testing
{


	public class MockHttpRequest : IHttpRequest
	{
		private DataDictionary uri_data;
		
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
		
		public bool Aborted {
			get;
			private set;
		}
		
		public DataDictionary UriData {
			get {
				if (uri_data == null)
					uri_data = new DataDictionary ();
				return uri_data;
			}
		}
	}
}
