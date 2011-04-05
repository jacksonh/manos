using System;
using System.Runtime.InteropServices;

namespace Libev
{
	public abstract class Watcher : IDisposable
	{
		protected IntPtr watcher_ptr;
		private bool running, disposed;
		protected GCHandle gc_handle;

		internal Watcher (Loop loop)
		{
			Loop = loop;
			gc_handle = GCHandle.Alloc (this);
		}

		public bool IsRunning {
			get { return running; }
		}

		public Loop Loop {
			get;
			private set;
		}

		public object UserData {
			get;
			set;
		}

		~Watcher ()
		{
			if (watcher_ptr != IntPtr.Zero) {
				Dispose ();
			}
		}

		public virtual void Dispose ()
		{
			if (disposed) {
				throw new ObjectDisposedException (GetType ().Name);
			}
			
			Stop ();
			
			DestroyWatcher ();
			
			watcher_ptr = IntPtr.Zero;
			gc_handle.Free ();
			
			GC.SuppressFinalize (this);
			disposed = true;
		}

		public void Start ()
		{
			if (running)
				return;

			running = true;
			
			StartImpl ();
		}

		public void Stop ()
		{
			if (!running)
				return;

			running = false;

			StopImpl ();
		}

		protected abstract void StartImpl ();

		protected abstract void StopImpl ();

		protected abstract void DestroyWatcher ();

		protected abstract void UnmanagedCallbackHandler (IntPtr loop, IntPtr watcher, EventTypes revents);
	}
}

