


using System;
using System.Collections;
using System.Collections.Specialized;
using System.Text.RegularExpressions;



namespace Mango.Routing {

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

		public bool IsMatch (string input, int start, NameValueCollection data, out int end)
		{
			Match m = regex.Match (input, start);

			regex.Match (input, start);
			if (!m.Success || m.Index != start) {
				end = start;
				return false;
			}
			
			AddUriData (m, data);
			end = m.Index + m.Length;
			return true;
		}

		private void AddUriData (Match m, NameValueCollection uri_data)
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
				uri_data.Add (gn, g.Value);
			}	
		}
	}
}


