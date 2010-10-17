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


using NUnit.Framework;

using Manos.Routing;
using System.Collections.Specialized;
using Manos.ShouldExt;

using Manos.Collections;

namespace Manos.Routing.Tests
{


	[TestFixture()]
	public class RegexMatchOperationTest
	{

		[Test]
		public void CtorTest ()
		{
			Should.Throw<ArgumentNullException> (() => new RegexMatchOperation (null), "a1");
			
			var r = new Regex (".*");
			var op = new RegexMatchOperation (r);
			Assert.AreEqual (r, op.Regex, "a2");
		}
		
		[Test]
		public void SetRegexTest ()
		{
			var r = new Regex (".*");
			var op = new RegexMatchOperation (r);
			Assert.AreEqual (r, op.Regex, "a1");
			
			Should.Throw<ArgumentNullException> (() => op.Regex = null, "a2");
			
			r = new Regex ("foo");
			op.Regex = r;
			Assert.AreEqual (r, op.Regex, "a3");
		}
		
		[Test()]
		public void EmptyRegexTest ()
		{
			var r = new Regex ("");
			var op = new RegexMatchOperation (r);
			var col = new DataDictionary ();
			int end;

			bool m = op.IsMatch ("", 0, out col, out end);
			
			Assert.IsTrue (m, "a1");
			Assert.AreEqual (0, end, "a2");
		}
		
		[Test]
		public void SimpleRegexTest ()
		{
			var r = new Regex (".og");
			var op = new RegexMatchOperation (r);
			var col = new DataDictionary ();
			int end;
			bool m;
			
			m = op.IsMatch ("dog", 0, out col, out end);
			Assert.IsTrue (m, "a1");
			Assert.AreEqual (0, col.Count, "a2");
			Assert.AreEqual (3, end, "a3");
			
			m = op.IsMatch ("log", 0, out col, out end);
			Assert.IsTrue (m, "a4");
			Assert.AreEqual (0, col.Count, "a5");
			Assert.AreEqual (3, end, "a6");
			
			m = op.IsMatch ("fox", 0, out col, out end);
			Assert.IsFalse (m, "a7");
			Assert.IsNull (col, "a8");
			Assert.AreEqual (0, end, "a9");
		}
		
		[Test]
		public void SimpleGroupTest ()
		{
			var r = new Regex ("-(?<foo>.*?)-");
			var op = new RegexMatchOperation (r);
			var col = new DataDictionary ();
			int end;
			bool m;
			
			m = op.IsMatch ("-manos-", 0, out col, out end);
			Assert.IsTrue (m, "a1");
			Assert.AreEqual (1, col.Count, "a2");
			Assert.AreEqual ("manos", col ["foo"], "a3");
			Assert.AreEqual (7, end, "a4");
			
			col = new DataDictionary ();
			m = op.IsMatch ("manos-", 0, out col, out end);
			Assert.IsFalse (m, "a5");
			Assert.IsNull (col, "a6");
			Assert.AreEqual (0, end, "a7");
		}
	}
}
