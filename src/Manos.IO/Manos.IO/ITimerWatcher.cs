using System;

namespace Manos.IO
{
	/// <summary>
	/// Timer watchers represent recurring timer events.
	/// <para>Corresponding event: the initial timeout has passed, or the
	/// repeat timeout has passed.</para>
	/// </summary>
	public interface ITimerWatcher : IBaseWatcher
	{
		/// <summary>
		/// Gets or sets the interval at which the watcher callback should
		/// be invoked. Changing this value need not immediatly restart the
		/// timeout, use the <see cref="Again"/> method for that.
		/// </summary>
		TimeSpan Repeat {
			get;
			set;
		}
		
		/// <summary>
		/// Rearm the timer with the interval specified in <see cref="Repeat"/>.
		/// </summary>
		void Again ();
	}
}

