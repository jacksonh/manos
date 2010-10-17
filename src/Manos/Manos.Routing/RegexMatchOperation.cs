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
using System.Collections;
using System.Collections.Specialized;
using System.Text.RegularExpressions;
using Manos.Collections;



namespace Manos.Routing {

	public class RegexMatchOperation : IMatchOperation {

		private Regex regex;

		public RegexMatchOperation (Regex regex)
		{
			if (regex == null)
				throw new ArgumentNullException ("regex");
			this.regex = regex;
		}

		public Regex Regex {
			get { return regex; }
			set {
				if (value == null)
					throw new ArgumentNullException ("value");
				regex = value;
			}
		}

		public bool IsMatch (string input, int start, out DataDictionary data, out int end)
		{
			Match m = regex.Match (input, start);

			regex.Match (input, start);
			if (!m.Success || m.Index != start) {
				data = null;
				end = start;
				return false;
			}

			data = new DataDictionary ();
			AddUriData (m, data);
			end = m.Index + m.Length;
			return true;
		}

		private void AddUriData (Match m, DataDictionary uri_data)
		{
			string [] groups = regex.GetGroupNames ();
			foreach (string gn in groups) {
				Group g = m.Groups [gn];
				
				//
				// Unfortunately regex matching creates named groups with
				// the match index as their name, we want to filter all of
				// these guys out.
				//
				int dummy;
				if (Int32.TryParse (gn, out dummy))
					continue;
				uri_data.Set (gn, g.Value);
			}	
		}
	}
}


