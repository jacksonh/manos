
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
		
		public bool IsMatch (string input, int start, DataDictionary data, out int end)
		{
			end = start;
			return true;
		}
	}
}
