
using System;
using NUnit.Framework;

namespace Manos.Tests
{


	[TestFixture()]
	public class ManosTargetTest
	{

		public static void FakeAction (IManosContext ctx)
		{
		}
		
		[Test()]
		public void TextNullCtor ()
		{
			Assert.Throws<ArgumentNullException> (() => new ManosTarget (null));
		}
		
		[Test]
		public void TestSetAction ()
		{
			var t = new ManosTarget (FakeAction);
			
			Assert.NotNull (t.Action, "not null");
			Assert.AreEqual (new ManosAction (FakeAction), t.Action, "equals");
		}
		
		[Test]
		public void TestSetActionNull ()
		{
			var t = new ManosTarget (FakeAction);
			
			Assert.Throws<ArgumentNullException> (() => t.Action = null);
		}
	}
}
