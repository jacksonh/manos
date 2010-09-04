using System;
namespace Manos
{
	public class InfiniteRepeatBehavior : IRepeatBehavior
	{
		public InfiniteRepeatBehavior ()
		{
		}

		public bool ShouldContinueToRepeat ()
		{
			return true;
		}

		public void RepeatPerformed ()
		{
		}
	}
}

