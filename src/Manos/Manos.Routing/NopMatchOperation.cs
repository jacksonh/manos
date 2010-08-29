
using System;
using System.Collections.Specialized;

namespace Manos.Routing
{


	public class NopMatchOperation : IMatchOperation
	{
		public NopMatchOperation ()
		{
		}
		
		public bool IsMatch (string input, int start, NameValueCollection data, out int end)
		{
			end = start;
			return true;
		}
	}
}
