using System;

namespace Manos.IO.Managed
{
	class IdleWatcher : Watcher, IIdleWatcher
	{
		private Action cb;

		public IdleWatcher (Context context, Action callback)
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

