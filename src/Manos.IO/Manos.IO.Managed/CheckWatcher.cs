using System;

namespace Manos.IO.Managed
{
	class CheckWatcher : Watcher, ICheckWatcher
	{
		private Action cb;

		public CheckWatcher (Context context, Action callback)
			: base (context)
		{
			if (callback == null)
				throw new ArgumentNullException ("callback");
			
			this.cb = callback;
		}

		public void Invoke ()
		{
			if (IsRunning) {
				cb ();
			}
		}

		protected override void Dispose (bool disposing)
		{
			Context.Remove (this);
		}
	}
}

