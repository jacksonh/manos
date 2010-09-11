using System;

using NUnit.Framework;
//
// Lets you use some of Nunits modern assert methods on monodevelops archaic nunit 
//

namespace Manos.ShouldExt
{
	public delegate void TestSnippet ();
	
	public class Should {
	
		public static void Throw<T> (TestSnippet snippet) where T : Exception
		{
			try {
				snippet ();
			} catch (Exception e) {
				if (e.GetType () == typeof (T))
					return;
				throw new Exception (String.Format ("Invalid exception type. Expected '{0}' got '{1}'", typeof (T), e.GetType ()));
			}
		
			throw new Exception ("No exception thrown.");
		}
		
		public static void Throw<T> (TestSnippet snippet, string message) where T : Exception
		{
			try {
				snippet ();
			} catch (Exception e) {
				if (e.GetType () == typeof (T))
					return;
				throw new Exception (String.Format ("{0}: Invalid exception type. Expected '{1}' got '{2}'", message, typeof (T), e.GetType ()));
			}
		
			throw new Exception (String.Format ("{0}: No exception thrown.", message));
		}
			
		public static void NotBeNull (object o)
		{
			if (o == null)
				throw new Exception ("Object is null.");
		}
		
		public static void NotBeNull (object o, string message)
		{
			if (o == null)
				throw new Exception (String.Format ("{0}: should not be null.", message));
		}
		
		public static void BeInstanceOf<T> (object o)
		{
			if (o == null)
				throw new Exception ("Item is null.");
			
			if (o.GetType () != typeof (T))
				throw new Exception (String.Format ("Expected '{0}' got '{1}'", typeof (T), o.GetType ()));
		}
		
		public static void BeInstanceOf<T> (object o, string message)
		{
			if (o == null)
				throw new Exception (String.Format ("{0}: Item is null.", message));
			
			if (o.GetType () != typeof (T))
				throw new Exception (String.Format ("{0}: Expected '{1}' got '{2}'", message, typeof (T), o.GetType ()));
		}
		
		public static void NotThrow (TestSnippet snippet)
		{
			snippet ();
		}
	}
}

