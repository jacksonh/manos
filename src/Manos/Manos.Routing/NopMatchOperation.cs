
using System;
using System.Collections.Specialized;
using Manos.Collections;

namespace Manos.Routing
{


	public class NopMatchOperation : IMatchOperation
	{
		public NopMatchOperation ()
		{
		}
		
		public bool IsMatch (string input, int start, out DataDictionary data, out int end)
		{
			data = null;
			end = start;
			return true;
		}
	}
}
