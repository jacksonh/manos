using System;
namespace Manos
{
	public static class RepeatBehavior
	{
		private static InfiniteRepeatBehavior forever = new InfiniteRepeatBehavior ();

		public static IRepeatBehavior Forever {
				get { return forever; }
		}
		
		public static IRepeatBehavior Single {
			get { return new IterativeRepeatBehavior (1); }
		}
		
		public static IRepeatBehavior Iterations (int num_iterations)
		{
			return new	IterativeRepeatBehavior (num_iterations);
		}
		
	}
}

