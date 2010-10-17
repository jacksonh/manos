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

using Manos.Routing;
using System.Collections.Specialized;
using Manos.ShouldExt;
using Manos.Collections;


namespace Manos.Routing.Tests
{


	[TestFixture()]
	public class StringMatchOperationTest
	{

		[Test()]
		public void TestCtor ()
		{
			Should.Throw<ArgumentNullException> (() => new StringMatchOperation (null), "a1");
			Should.Throw<ArgumentException> (() => new StringMatchOperation (String.Empty), "a2");
			
			var op = new StringMatchOperation ("foo");
			Assert.AreEqual ("foo", op.String, "a3");
		}
		
		[Test()]
		public void TestStringProperty ()
		{
			var op = new StringMatchOperation ("foo");
			Assert.AreEqual ("foo", op.String, "a1");
			
			Should.Throw<ArgumentNullException> (() => op.String = null, "a2");
			Should.Throw<ArgumentException> (() => op.String = String.Empty, "a3");
			
			op.String = "baz";
			Assert.AreEqual ("baz", op.String);
		}
		
		[Test()]
		public void MatchFullStringTest ()
		{
			var op = new StringMatchOperation ("foobar");
			var data = new DataDictionary ();
			int end;
			
			bool m = op.IsMatch ("foobar", 0, out data, out end);
			
			Assert.IsTrue (m, "a1");
			Assert.IsNull (data, "a2");
			Assert.AreEqual (6, end, "a3");
		}
		
		[Test()]
		public void MatchPartialStringTest ()
		{
			var op = new StringMatchOperation ("foo");
			var data = new DataDictionary ();
			int end;
			
			bool m = op.IsMatch ("foobar", 0, out data, out end);
			
			Assert.IsTrue (m, "a1");
			Assert.IsNull (data, "a2");
			Assert.AreEqual (3, end, "a3");
			
			op = new StringMatchOperation ("bar");
			m = op.IsMatch ("foobar", end, out data, out end);
			Assert.IsTrue (m, "a4");
			Assert.IsNull (data, "a5");
			Assert.AreEqual (6, end, "a3");
			
		}
		
		[Test ()]
		public void MatchNotAtStartShouldFail ()
		{
			var op = new StringMatchOperation ("oobar");
			var data = new DataDictionary ();
			int end;
			
			bool m = op.IsMatch ("foobar", 0, out data, out end);
			
			Assert.IsFalse (m, "a1");
			Assert.IsNull (data, "a2");
			Assert.AreEqual (0, end, "a3");
		}
	}
}
