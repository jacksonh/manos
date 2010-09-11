using System;
using NUnit.Framework;
namespace Manos.Tests
{
	[TestFixture()]
	public class InfiniteRepeatBehaviorTest
	{
		[Test()]
		public void ShouldContinueToRepeat_AlwaysReturnsTrue ()
		{
			var infinite = new InfiniteRepeatBehavior ();
			
			infinite.RepeatPerformed ();
			infinite.RepeatPerformed ();
			
			bool should_continue = infinite.ShouldContinueToRepeat ();
			Assert.IsTrue (should_continue);
		}
	}
}

