using System;
namespace Manos
{
	public interface IRepeatBehavior
	{
		bool ShouldContinueToRepeat ();
		
		void RepeatPerformed ();
	}
}

