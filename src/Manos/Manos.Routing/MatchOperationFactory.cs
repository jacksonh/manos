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
using System.Text.RegularExpressions;

namespace Manos.Routing
{


	public static class MatchOperationFactory
	{
		// If any of these guys are in there we consider it a regex string
		public static readonly char [] REGEX_CHARS = new char [] {
			'.', '[', ']', '^', '$', '(', ')', '*',
		};
		
		public static readonly char [] SIMPLE_CHARS = new char [] {
			'{', '}',	
		};
		
		public static IMatchOperation Create (string pattern, MatchType type)
		{
			if (pattern == null)
				throw new ArgumentNullException ("pattern");
			
			if (pattern.Length == 0)
				return new NopMatchOperation ();	

			switch (type) {
			case MatchType.Simple:
				return new SimpleMatchOperation (pattern);
			case MatchType.String:
				return new StringMatchOperation (pattern);
			case MatchType.Regex:
				Regex r = new Regex (pattern);
				return new RegexMatchOperation (r);
			default:
				throw new InvalidOperationException ("unknown match operation type.");
			}
			
		}
	}
}
