

using System;
using System.Collections;
using System.Collections.Specialized;
using Manos.Collections;


namespace Manos.Routing {

	public interface IMatchOperation {

		bool IsMatch (string input, int start, DataDictionary data, out int end);
	}
}

