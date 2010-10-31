

using System;
using System.Runtime.InteropServices;


namespace Libev {

	public class CheckWatcher : Watcher {

		private CheckWatcherCallback callback;
		private UnmanagedCheckWatcher unmanaged_watcher;
		
		public CheckWatcher (Loop loop, CheckWatcherCallback callback) : base (loop)
		{ 
			this.callback = callback;
			
			unmanaged_watcher = new UnmanagedCheckWatcher ();
			unmanaged_watcher.callback = CallbackFunctionPtr;

			InitializeUnmanagedWatcher (unmanaged_watcher);
		}
	
		protected override void StartImpl ()
		{			
			ev_check_start (Loop.Handle, WatcherPtr);
		}
		
		protected override void StopImpl ()
		{			
			ev_check_stop (Loop.Handle, WatcherPtr);	
		}
		
		protected override void UnmanagedCallbackHandler (IntPtr _loop, IntPtr _watcher, int revents)
		{
			// Maybe I should verify the pointers?
			
			callback (Loop, this, revents);
		}

        [DllImport ("libev", CallingConvention = CallingConvention.Cdecl)]
		private static extern void ev_check_start (IntPtr loop, IntPtr watcher);

        [DllImport ("libev", CallingConvention = CallingConvention.Cdecl)]
		private static extern void ev_check_stop (IntPtr loop, IntPtr watcher);
	}
	
    [UnmanagedFunctionPointer (System.Runtime.InteropServices.CallingConvention.Cdecl)]
	public delegate void CheckWatcherCallback (Loop loop, CheckWatcher watcher, int revents);
	
	[StructLayout (LayoutKind.Sequential)]
	internal struct UnmanagedCheckWatcher {
		
		public int active;
		public int pending;
		public int priority;
		
		public IntPtr data;
		public IntPtr callback;
		
		internal IntPtr next;
	}
}

