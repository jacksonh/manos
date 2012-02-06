using System;

namespace Manos.IO.Managed
{
	class Notifier : INotifier
	{
		Action callback;
		Context context;
		int count = 0;
		object syncRoot = new object();

		public Notifier (Context context, Action callback)
		{
			this.callback = callback;
			this.context = context;
		}

		public void Notify ()
		{
			lock (syncRoot) {
				if (IsRunning) {
					context.Enqueue (callback);
				} else {
					count++;
				}
			}
		}

		public void Start ()
		{
			lock (syncRoot) {
				if (!IsRunning) {
					while (count > 0) {
						context.Enqueue (callback);
						count--;
					}
					IsRunning = true;
				}
			}
		}

		public void Stop ()
		{
			lock (syncRoot) {
				if (IsRunning) {
					IsRunning = false;
				}
			}
		}

		public bool IsRunning {
			get;
			protected set;
		}

		public void Dispose ()
		{
		}
	}
}

