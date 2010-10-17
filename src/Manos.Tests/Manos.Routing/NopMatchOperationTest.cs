
using System;
using NUnit.Framework;

using Manos.Routing;
using System.Collections.Specialized;
using Manos.Collections;

namespace Manos.Routing.Tests
{


	[TestFixture()]
	public class NopMatchOperationTest
	{

		[Test()]
		public void TestIsMatch ()
		{
			var op = new NopMatchOperation ();
			var data = new DataDictionary ();
			int end;
			
			bool m = op.IsMatch ("foobar", 0, out data, out end);
			
			Assert.IsTrue (m, "a1");
			Assert.IsNull (data, "a2");
			Assert.AreEqual (0, end, "a3");
			
			m = op.IsMatch ("foobar", 3, out data, out end);
			
			Assert.IsTrue (m, "a4");
			Assert.IsNull (data, "a5");
			Assert.AreEqual (3, end, "a6");
			
		}
	}
}
