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
	/// <summary>
	/// Since returning non-encoded content to the browser can introduce unwanted 
	/// Cross-Site Scripting attacks (and other bad things), Manos will encode the
	/// output automatically, this class allows a developer to specify content that should be returned as is.
	/// "With great power comes great responsibility."
	/// </summary>
	/// <remarks>
	/// ATT: I think that the summary is right, Jackson, can you confirm?
	/// </remarks>
	public class UnsafeString
	{
		private string unsafe_value;
		private string safe_value;
		private bool has_unsafe_data;
		
		public UnsafeString (string str)
		{
			this.unsafe_value = str;
		}
		
		/// <summary>
		/// The original, non-escaped string.
		/// </summary>
		public string UnsafeValue {
			get { return unsafe_value; }	
		}
		
		/// <summary>
		/// The "safer" version of this string, has some common "unsafe" characters replaced with their HTML Entity counterparts.
		/// </summary>
		public string SafeValue {
			get {
				if (safe_value == null)
					safe_value = Escape (unsafe_value, out has_unsafe_data);
				return safe_value;
			}
		}
		
		/// <summary>
		/// Indicates true if the original string value contained "unsafe" content.
		/// </summary>
		public bool HasUnsafeData {
			get {
				if (safe_value == null)
					safe_value = Escape (unsafe_value, out has_unsafe_data);
				return has_unsafe_data;
			}
		}
		
		/// <summary>
		/// Returns a "safe" version of this string.
		/// </summary>
		/// <returns>
		/// A <see cref="System.String"/>
		/// </returns>
		public override string ToString ()
		{
			return SafeValue;
		}
		
		/// <summary>
		/// Implicitly constructs an unsafe string object from a <see cref="System.String"/>.
		/// </summary>
		/// <param name="input">
		/// A <see cref="UnsafeString"/>
		/// </param>
		/// <returns>
		/// A <see cref="System.String"/>
		/// </returns>
		public static implicit operator string (UnsafeString input)
		{
			if (input == null)
				return null;
			return input.ToString ();	
		}
		
		/// <summary>
		/// Substitute common characters that could cause security vulnerabilities with their HTML Entity counterparts. 
		/// </summary>
		/// <param name="input">
		/// The string that has potentially unsafe values.
		/// </param>
		/// <returns>
		/// The input with common "unsafe" characters replaced with their "safe" HTML Entity counterparts.
		/// </returns>
		public static string Escape (string input)
		{
			bool dummy;
			
			return Escape (input, out dummy);
		}
		
		/// <summary>
		/// Substitute common characters that could cause security vulnerabilities with their HTML Entity counterparts. 
		/// </summary>
		/// <param name="input">
		/// The string that has potentially unsafe values.
		/// </param>
		/// <param name="has_unsafe_data">
		/// True if any substitutions take place, false otherwise.
		/// </param>
		/// <returns>
		/// The input with common "unsafe" characters replaced with their "safe" HTML Entity counterparts.
		/// </returns>
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

