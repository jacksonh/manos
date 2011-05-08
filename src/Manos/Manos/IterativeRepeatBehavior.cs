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
namespace Manos
{
	public class IterativeRepeatBehavior : IRepeatBehavior
	{
		private int iterations;
		
		public IterativeRepeatBehavior (int num_iterations)
		{
			if (num_iterations <= 0)
				throw new ArgumentOutOfRangeException ("iterations", "IterativeRepeatBehavior must be created with at least one iteration.");
			RemainingIterations = num_iterations;
		}
		
		public int RemainingIterations {
			get { return iterations; }
			set {
				if (value < 0)
					throw new ArgumentOutOfRangeException ("iterations", "Can not set RemainingIterations to a negative value.");
				iterations = value;
			}
		}	
	
		public bool ShouldContinueToRepeat ()
		{
			return iterations > 0;
		}

		public void RepeatPerformed ()
		{
			if (iterations > 0)
				--iterations;
		}
	}
}

