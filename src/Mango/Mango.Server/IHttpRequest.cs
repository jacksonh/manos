

using System;

namespace Mango.Server {

	public interface IHttpRequest {
		
		string Method {
			get;	
		}
		
		string LocalPath {
			get;
		}

	}
}

