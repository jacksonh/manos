using System;
using NUnit.Framework;
namespace Manos.Tests
{
	[TestFixture()]
	public class RepeatBehaviorTest
	{
		[Test()]
		public void Single_RepeatsOnce ()
		{
			var repeat = RepeatBehavior.Single;
			
			bool should_continue = repeat.ShouldContinueToRepeat ();
			Assert.IsTrue (should_continue);
		}
		
		[Test]
		public void Single_DoesNotRepeatTwice ()
		{
			var repeat = RepeatBehavior.Single;
			
			repeat.RepeatPerformed ();
			
			bool should_continue = repeat.ShouldContinueToRepeat ();
			Assert.IsFalse (should_continue);
		}
		
		[Test]
		public void Forever_RepeatsForver ()
		{
			var repeat = RepeatBehavior.Forever;
			
			repeat.RepeatPerformed ();
			repeat.RepeatPerformed ();
			repeat.RepeatPerformed ();
			repeat.RepeatPerformed ();
			
			bool should_continue = repeat.ShouldContinueToRepeat ();
			Assert.IsTrue (should_continue);
		}
	}
}

