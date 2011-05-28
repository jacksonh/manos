using System;
using System.Threading;

namespace Manos.IO.Managed
{
	class TimerWatcher : Watcher, ITimerWatcher
	{
		private Action cb;
		private Timer timer;
		private TimeSpan after;

		public TimerWatcher (Context context, Action callback, TimeSpan after, TimeSpan repeat)
			: base (context)
		{
			if (callback == null)
				throw new ArgumentNullException ("callback");
			
			this.cb = callback;
			this.timer = new Timer (Invoke);
			this.after = after;
			this.Repeat = repeat;
		}

		void Invoke (object state)
		{
			if (IsRunning) {
				Context.Enqueue (cb);
				after = TimeSpan.Zero;
			}
		}

		public override void Start ()
		{
			base.Start ();
			timer.Change ((int) after.TotalMilliseconds,
				Repeat == TimeSpan.Zero ? Timeout.Infinite : (int) Repeat.TotalMilliseconds);
		}

		public override void Stop ()
		{
			timer.Change (Timeout.Infinite, Timeout.Infinite);
			base.Stop ();
		}

		protected override void Dispose (bool disposing)
		{
			Context.Remove (this);
		}

		public void Again ()
		{
			after = TimeSpan.Zero;
			Start ();
		}

		public TimeSpan Repeat {
			get;
			set;
		}
	}
}

