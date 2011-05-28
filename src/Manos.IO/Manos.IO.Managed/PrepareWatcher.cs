using System;

namespace Manos.IO.Managed
{
	class PrepareWatcher : Watcher, IPrepareWatcher
	{
		private Action cb;

		public PrepareWatcher (Context context, Action callback)
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

