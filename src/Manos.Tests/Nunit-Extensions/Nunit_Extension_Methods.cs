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

