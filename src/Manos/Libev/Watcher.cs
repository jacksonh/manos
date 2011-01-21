

using System;
using System.Runtime.InteropServices;

namespace Libev {

	public abstract class Watcher : IDisposable {

		protected IntPtr watcher_ptr;

		private bool running;
		protected GCHandle gc_handle;

		internal Watcher (Loop loop)
		{
			Loop = loop;
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

		protected IntPtr WatcherPtr {
			get { return watcher_ptr; }
		}
		
		public virtual void Dispose ()
		{
			Stop ();
			
			Marshal.FreeHGlobal (watcher_ptr);
			watcher_ptr = IntPtr.Zero;			
		}	

		protected void InitializeUnmanagedWatcher (object unmanaged_watcher)
		{
			watcher_ptr = Marshal.AllocHGlobal (Marshal.SizeOf (unmanaged_watcher.GetType ()));
			Marshal.StructureToPtr (unmanaged_watcher, watcher_ptr, true);
		}

		

		public void Start ()
		{
			if (running)
			   return;

			running = true;
			gc_handle = GCHandle.Alloc (this);
			
			StartImpl ();
		}

		public void Stop ()
		{
			if (!running)
			   return;

			running = false;
			gc_handle.Free ();

			StopImpl ();
		}

		protected abstract void StartImpl ();
		protected abstract void StopImpl ();		
		protected abstract void UnmanagedCallbackHandler (IntPtr loop, IntPtr watcher, EventTypes revents);
	}
}

