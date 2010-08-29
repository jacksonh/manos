

using System;
using System.Collections.Specialized;

namespace Manos.Server {

	public interface IHttpRequest {
		
		string Method {
			get;	
		}
		
		string LocalPath {
			get;
		}

		NameValueCollection UriData {
			get;	
		}
	}
}

