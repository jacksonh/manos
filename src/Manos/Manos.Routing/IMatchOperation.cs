

using System;
using System.Collections;
using System.Collections.Specialized;


namespace Mango.Routing {

	public interface IMatchOperation {

		bool IsMatch (string input, int start, NameValueCollection data, out int end);
	}
}

