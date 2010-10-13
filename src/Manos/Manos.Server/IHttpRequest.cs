

using System;
using System.Text;
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
		
		DataDictionary PostData {
			get;
		}

		DataDictionary QueryData {
			get;
		}

		DataDictionary UriData {
			get;	
		}
		
		DataDictionary Cookies {
			get;	
		}

		HttpHeaders Headers {
			get;
		}
		
		bool Http_1_1_Supported {
			get;     
		}

		Encoding ContentEncoding {
			get;
		}

		void SetWwwFormData (DataDictionary data);
		
	}
}

