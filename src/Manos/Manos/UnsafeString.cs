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
using System.Text;

namespace Manos
{
	public class UnsafeString
	{
		private string unsafe_value;
		private string safe_value;
		private bool has_unsafe_data;
		
		public UnsafeString (string str)
		{
			this.unsafe_value = str;
		}
		
		public string UnsafeValue {
			get { return unsafe_value; }	
		}
		
		public string SafeValue {
			get {
				if (safe_value == null)
					safe_value = Escape (unsafe_value, out has_unsafe_data);
				return safe_value;
			}
		}
		
		public bool HasUnsafefData {
			get {
				if (safe_value == null)
					safe_value = Escape (unsafe_value, out has_unsafe_data);
				return has_unsafe_data;
			}
		}
		
		public override string ToString ()
		{
			return SafeValue;
		}
		
		public static implicit operator string (UnsafeString input)
		{
			if (input == null)
				return null;
			return input.ToString ();	
		}
		
		public static string Escape (string input)
		{
			bool dummy;
			
			return Escape (input, out dummy);
		}
		
		public static string Escape (string input, out bool has_unsafe_data)
		{
			StringBuilder builder = new StringBuilder ();

			has_unsafe_data = false;
			
			for (int i = 0; i < input.Length; i++) {
				char c = input [i];
				
				switch (c) {
				case '&':
					builder.Append ("&amp;");
					has_unsafe_data  = true;
					break;
				case '<':
					builder.Append ("&lt;");
					has_unsafe_data = true;
					break;
				case '>':
					builder.Append ("&gt;");
					has_unsafe_data = true;
					break;
				case '"':
					builder.Append ("&quot;");
					has_unsafe_data = true;
					break;
				case '\'':
					builder.Append ("&#39;");
					has_unsafe_data = true;
					break;
				default:
					builder.Append (c);
					break;
				}
			}
			
			if (has_unsafe_data)
				return builder.ToString ();
			return input;	
		}
	}
}

