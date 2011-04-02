

using System;
using System.Runtime.InteropServices;
using Manos;

namespace Libev {

	public abstract class Watcher : BaseWatcher {

		protected IntPtr watcher_ptr;

		protected GCHandle gc_handle;

		internal Watcher (Loop loop): base(loop)
		{
		}
		
				

		protected IntPtr WatcherPtr {
			get { return watcher_ptr; }
		}
		
		public override void Dispose ()
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



        public override void Start()
		{
			if (running)
			   return;

			running = true;
			gc_handle = GCHandle.Alloc (this);
			
			StartImpl ();
		}

		public override void Stop ()
		{
			if (!running)
			   return;

			running = false;
			gc_handle.Free ();

			StopImpl ();
		}

        public new LibEvLoop Loop
        {
            get { return (LibEvLoop)base.Loop; }
        }

		protected abstract void StartImpl ();
		protected abstract void StopImpl ();		
		protected abstract void UnmanagedCallbackHandler (IntPtr loop, IntPtr watcher, EventTypes revents);
	}
}

