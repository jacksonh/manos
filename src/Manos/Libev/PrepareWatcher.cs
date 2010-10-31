

using System;
using System.Runtime.InteropServices;


namespace Libev {

	public class PrepareWatcher : Watcher, IDisposable {

		private PrepareWatcherCallback callback;
		private UnmanagedPrepareWatcher unmanaged_watcher;
		
		public PrepareWatcher (Loop loop, PrepareWatcherCallback callback) : base (loop)
		{ 
			this.callback = callback;
			
			unmanaged_watcher = new UnmanagedPrepareWatcher ();
			unmanaged_watcher.callback = CallbackFunctionPtr;

			InitializeUnmanagedWatcher (unmanaged_watcher);
		}


		protected override void StartImpl ()
		{
			ev_prepare_start (Loop.Handle, WatcherPtr);
		}
		
		protected override void StopImpl ()
		{			
			ev_prepare_stop (Loop.Handle, WatcherPtr);	
		}
		
		protected override void UnmanagedCallbackHandler (IntPtr _loop, IntPtr _watcher, int revents)
		{
			// Maybe I should verify the pointers?
			
			callback (Loop, this, revents);
		}

        [DllImport ("libev", CallingConvention = CallingConvention.Cdecl)]
		private static extern void ev_prepare_start (IntPtr loop, IntPtr watcher);

        [DllImport ("libev", CallingConvention = CallingConvention.Cdecl)]
		private static extern void ev_prepare_stop (IntPtr loop, IntPtr watcher);
	}
	
    [UnmanagedFunctionPointer (System.Runtime.InteropServices.CallingConvention.Cdecl)] 
	public delegate void PrepareWatcherCallback (Loop loop, PrepareWatcher watcher, int revents);
	
	[StructLayout (LayoutKind.Sequential)]
	internal struct UnmanagedPrepareWatcher {
		
		public int active;
		public int pending;
		public int priority;
		
		public IntPtr data;
		public IntPtr callback;
		
		internal IntPtr next;
	}
}

