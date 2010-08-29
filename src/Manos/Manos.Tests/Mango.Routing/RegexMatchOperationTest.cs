
using System;
using System.Text.RegularExpressions;


using NUnit.Framework;

using Mango.Routing;
using System.Collections.Specialized;

namespace Mango.Routing.Tests
{


	[TestFixture()]
	public class RegexMatchOperationTest
	{

		[Test]
		public void CtorTest ()
		{
			Assert.Throws<ArgumentNullException> (() => new RegexMatchOperation (null), "a1");
			
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
			
			Assert.Throws<ArgumentNullException> (() => op.Regex = null, "a2");
			
			r = new Regex ("foo");
			op.Regex = r;
			Assert.AreEqual (r, op.Regex, "a3");
		}
		
		[Test()]
		public void EmptyRegexTest ()
		{
			var r = new Regex ("");
			var op = new RegexMatchOperation (r);
			var col = new NameValueCollection ();
			int end;

			bool m = op.IsMatch ("", 0, col, out end);
			
			Assert.IsTrue (m, "a1");
			Assert.AreEqual (0, col.Count, "a2");
			Assert.AreEqual (0, end, "a3");
		}
		
		[Test]
		public void SimpleRegexTest ()
		{
			var r = new Regex (".og");
			var op = new RegexMatchOperation (r);
			var col = new NameValueCollection ();
			int end;
			bool m;
			
			m = op.IsMatch ("dog", 0, col, out end);
			Assert.IsTrue (m, "a1");
			Assert.AreEqual (0, col.Count, "a2");
			Assert.AreEqual (3, end, "a3");
			
			m = op.IsMatch ("log", 0, col, out end);
			Assert.IsTrue (m, "a4");
			Assert.AreEqual (0, col.Count, "a5");
			Assert.AreEqual (3, end, "a6");
			
			m = op.IsMatch ("fox", 0, col, out end);
			Assert.IsFalse (m, "a7");
			Assert.AreEqual (0, col.Count, "a8");
			Assert.AreEqual (0, end, "a9");
		}
		
		[Test]
		public void SimpleGroupTest ()
		{
			var r = new Regex ("-(?<foo>.*?)-");
			var op = new RegexMatchOperation (r);
			var col = new NameValueCollection ();
			int end;
			bool m;
			
			m = op.IsMatch ("-mango-", 0, col, out end);
			Assert.IsTrue (m, "a1");
			Assert.AreEqual (1, col.Count, "a2");
			Assert.AreEqual ("mango", col ["foo"], "a3");
			Assert.AreEqual (7, end, "a4");
			
			col = new NameValueCollection ();
			m = op.IsMatch ("mango-", 0, col, out end);
			Assert.IsFalse (m, "a5");
			Assert.AreEqual (0, col.Count, "a6");
			Assert.AreEqual (0, end, "a7");
		}
	}
}
