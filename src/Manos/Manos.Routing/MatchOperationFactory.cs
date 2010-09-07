
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
			
			// Must contain { and } chars, and those chars must not be escaped ie {{ }}
			int cls;
			int idx = pattern.IndexOf ('{');
			if (idx >= 0 && 
			    idx < pattern.Length - 1 && pattern [idx + 1] != '{' && 
			    (cls = pattern.IndexOf ('}', idx)) > 0 && 
				(cls == pattern.Length - 1 || pattern [cls + 1] != '}')) {
				
				return new SimpleMatchOperation (pattern);	
			}
			
			return new StringMatchOperation (pattern);
		}
	}
}
