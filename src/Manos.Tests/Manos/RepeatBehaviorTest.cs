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

