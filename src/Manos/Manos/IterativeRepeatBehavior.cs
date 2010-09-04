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

