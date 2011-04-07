//
// Copyright (C) 2010 Jackson Harper (jackson@manosdemono.com)
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//
//


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
				str = value.ToLower();
			}
		}
	
		public bool IsMatch (string input, int start, out DataDictionary data, out int end)
		{
			if (!StartsWith (input, start, String)) {
				
				// some special processing - ugh
				// route to '/' when the parent app has Route("/test", new SubModule)
				// and sub module has [Get("/")]
				if ("/" == String && (input.Length + 1 == str.Length + start)) {
					data = null;
					end = input.Length; // force acceptance
					return true;					
				} else {				
					data = null;
					end = start;
					return false;
				}
			}

			data = null;
			end = start + String.Length;
			return true;
		}

		public static bool StartsWith (string input, int start, string str)
		{
			if (input.Length < str.Length + start)
				return false;
				
			return String.Compare (input, start, str, 0, str.Length, StringComparison.OrdinalIgnoreCase) == 0;
		}
		
	}
}
