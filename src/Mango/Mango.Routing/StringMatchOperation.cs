
using System;
using System.Collections.Specialized;

namespace Mango.Routing
{


	public class StringMatchOperation : IMatchOperation
	{
		private string str;
		
		public StringMatchOperation (string str)
		{
			String = str;
		}
		
		public string String {
			get { return str; }
			set {
				if (value == null)
					throw new ArgumentNullException ("value");
				if (value.Length == 0)
					throw new ArgumentException ("StringMatch operations should not use empty strings.");
				str = value;
			}
		}
	
		public bool IsMatch (string input, int start, NameValueCollection data, out int end)
		{
			int i = input.IndexOf (String, start);

			if (i != start) {
				end = start;
				return false;
			}
			
			end = start + String.Length;
			return true;
		}


	}
}
