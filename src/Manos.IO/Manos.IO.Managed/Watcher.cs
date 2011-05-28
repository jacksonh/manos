using System;

namespace Manos.IO.Managed
{
	abstract class Watcher : Manos.IO.IBaseWatcher
	{
		public Watcher (Context context)
		{
			this.Context = context;
		}

		public Context Context {
			get;
			private set;
		}

		public virtual void Start ()
		{
			IsRunning = true;
		}

		public virtual void Stop ()
		{
			IsRunning = false;
		}

		public bool IsRunning {
			get;
			protected set;
		}

		public virtual void Dispose ()
		{
			Dispose (true);
		}

		protected abstract void Dispose (bool disposing);
	}
}

