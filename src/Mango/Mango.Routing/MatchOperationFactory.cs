
using System;
using System.Text.RegularExpressions;

namespace Mango.Routing
{


	public static class MatchOperationFactory
	{
		// If any of these guys are in there we consider it a regex string
		public static readonly char [] REGEX_CHARS = new char [] {
			'.', '[', ']', '^', '$', '(', ')', '*',
		};
		
		public static IMatchOperation Create (string pattern)
		{
			if (pattern == null)
				throw new ArgumentNullException ("pattern");
			
			if (pattern.Length == 0)
				return new NopMatchOperation ();	
			
			if (pattern.IndexOfAny (REGEX_CHARS) >= 0) {
				Regex r = new Regex (pattern);
				return new RegexMatchOperation (r);
			}
			
			return new StringMatchOperation (pattern);
		}
	}
}
