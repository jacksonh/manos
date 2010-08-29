

using System;
using System.Collections;
using System.Collections.Specialized;


namespace Manos.Routing {

	public interface IMatchOperation {

		bool IsMatch (string input, int start, NameValueCollection data, out int end);
	}
}

