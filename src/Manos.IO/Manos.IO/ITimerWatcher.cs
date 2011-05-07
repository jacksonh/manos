using System;

namespace Manos.IO
{
	public interface ITimerWatcher : IBaseWatcher
	{
		TimeSpan Repeat {
			get;
			set;
		}

		void Again ();
	}
}

