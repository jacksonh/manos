using System;

namespace Manos.IO.Managed
{
	class AsyncWatcher : Watcher, Manos.IO.IAsyncWatcher
	{
		private bool pending;
		private Action callback;

		public AsyncWatcher (Context context, Action callback)
			: base (context)
		{
			if (callback == null)
				throw new ArgumentNullException ("callback");
			
			this.callback = callback;
		}

		public void Send ()
		{
			if (!pending && IsRunning) {
				Context.Enqueue (delegate {
					pending = false;
					callback ();
				});
				pending = true;
			}
		}

		public override void Start ()
		{
			base.Start ();
			pending = false;
		}

		public override void Stop ()
		{
			base.Stop ();
			pending = false;
		}

		protected override void Dispose (bool disposing)
		{
			Context.Remove (this);
		}
	}
}

