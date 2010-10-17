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
namespace Manos.Tests
{
	[TestFixture()]
	public class IterativeRepeatBehaviorTest
	{
		[Test()]
		public void Ctor_NegativeValue_Throws ()
		{
			Should.Throw<ArgumentOutOfRangeException> (() => new IterativeRepeatBehavior (-1));
		}
		
		[Test]
		public void Ctor_ZeroValue_Throws ()
		{
			Should.Throw<ArgumentOutOfRangeException> (() => new IterativeRepeatBehavior (0));
		}
		
		[Test]
		public void Ctor_PositiveValue_SetsRemainingIterationsProperty ()
		{
			var repeat = new IterativeRepeatBehavior (25);
			
			Assert.AreEqual (25, repeat.RemainingIterations);
		}
		
		[Test]
		public void RemainingIterations_SetNegative_Throws ()
		{
			var repeat = new IterativeRepeatBehavior (25);
			
			Should.Throw<ArgumentOutOfRangeException> (() => repeat.RemainingIterations = -1);
		}
		
		[Test]
		public void RemainingIterations_SetZero_DoesNotthrow ()
		{
			var repeat = new IterativeRepeatBehavior (10);
			
			repeat.RemainingIterations = 0;
		}
		
		[Test]
		public void RemainingIterations_SetPositiveValue_SetsValue ()
		{
			var repeat = new IterativeRepeatBehavior (5);
			
			repeat.RemainingIterations = 10;
			
			Assert.AreEqual (10, repeat.RemainingIterations);
		}
		
		[Test]
		public void RepeatPerformed_DecrementsRemainingIterations ()
		{
			var repeat = new IterativeRepeatBehavior (5);	
		
			repeat.RepeatPerformed ();
			
			Assert.AreEqual (4, repeat.RemainingIterations);
		}
		
		[Test]
		public void RepeatPerformed_CallMoreThanRemainingIterationsTotal_RemainingIterationsIsZero ()
		{
			var repeat = new IterativeRepeatBehavior (3);
			
			repeat.RepeatPerformed ();
			repeat.RepeatPerformed ();
			repeat.RepeatPerformed ();
			repeat.RepeatPerformed ();
			repeat.RepeatPerformed ();
			
			Assert.AreEqual (0, repeat.RemainingIterations);
		}
		
		[Test]
		public void ShouldContinueToRepeat_NonZeroRemainingIterations_ReturnsTrue ()
		{
			var repeat = new IterativeRepeatBehavior (1);
			
			bool should_continue = repeat.ShouldContinueToRepeat ();
			Assert.IsTrue (should_continue);
		}
		
		[Test]
		public void ShouldContinueToRepeat_ZeroRemainingIterations_ReturnsFalse ()
		{
			var repeat = new IterativeRepeatBehavior (1);
			
			repeat.RepeatPerformed ();
			
			bool should_continue = repeat.ShouldContinueToRepeat ();
			Assert.IsFalse (should_continue);
		}
	}
}

