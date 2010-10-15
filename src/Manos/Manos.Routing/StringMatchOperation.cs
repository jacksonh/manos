
using System;
using System.Collections.Specialized;
using Manos.Collections;

namespace Manos.Routing
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
	
		public bool IsMatch (string input, int start, DataDictionary data, out int end)
		{
			if (!StartsWith (input, start, String)) {
				end = start;
				return false;
			}
			
			end = start + String.Length;
			return true;
		}

		public static bool StartsWith (string input, int start, string str)
		{
			if (input.Length < str.Length + start)
				return false;

			for (int i = 0; i < str.Length; i++) {
				if (input [start + i] != str [i])
					return false;
			}

			return true;
		}
		
	}
}
