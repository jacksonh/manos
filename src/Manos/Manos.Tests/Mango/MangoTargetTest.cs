
using System;
using NUnit.Framework;

namespace Mango.Tests
{


	[TestFixture()]
	public class MangoTargetTest
	{

		public static void FakeAction (IMangoContext ctx)
		{
		}
		
		[Test()]
		public void TextNullCtor ()
		{
			Assert.Throws<ArgumentNullException> (() => new MangoTarget (null));
		}
		
		[Test]
		public void TestSetAction ()
		{
			var t = new MangoTarget (FakeAction);
			
			Assert.NotNull (t.Action, "not null");
			Assert.AreEqual (new MangoAction (FakeAction), t.Action, "equals");
		}
		
		[Test]
		public void TestSetActionNull ()
		{
			var t = new MangoTarget (FakeAction);
			
			Assert.Throws<ArgumentNullException> (() => t.Action = null);
		}
	}
}
