
using System;
using NUnit.Framework;

using Mango.Routing;
using System.Collections.Specialized;

namespace Mango.Routing.Tests
{


	[TestFixture()]
	public class NopMatchOperationTest
	{

		[Test()]
		public void TestIsMatch ()
		{
			var op = new NopMatchOperation ();
			var data = new NameValueCollection ();
			int end;
			
			bool m = op.IsMatch ("foobar", 0, data, out end);
			
			Assert.IsTrue (m, "a1");
			Assert.AreEqual (0, data.Count, "a2");
			Assert.AreEqual (0, end, "a3");
			
			m = op.IsMatch ("foobar", 3, data, out end);
			
			Assert.IsTrue (m, "a4");
			Assert.AreEqual (0, data.Count, "a5");
			Assert.AreEqual (3, end, "a6");
			
		}
	}
}
