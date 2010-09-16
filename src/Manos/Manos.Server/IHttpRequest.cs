

using System;
using System.Collections.Specialized;

using Manos.Collections;

namespace Manos.Server {

	public interface IHttpRequest {
		
		string Method {
			get;	
		}
		
		string LocalPath {
			get;
		}

		DataDictionary Data {
			get;	
		}
		
		DataDictionary UriData {
			get;	
		}
	}
}

