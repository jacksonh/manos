

using System;
using System.Runtime.InteropServices;
using Manos;


namespace Libev {

	public class CheckWatcher : Watcher {

		private CheckWatcherCallback callback;
		private UnmanagedCheckWatcher unmanaged_watcher;

		
		private static IntPtr unmanaged_callback_ptr;
		private static UnmanagedWatcherCallback unmanaged_callback;

		static CheckWatcher ()
		{
			unmanaged_callback = new UnmanagedWatcherCallback (StaticCallback);
			unmanaged_callback_ptr = Marshal.GetFunctionPointerForDelegate (unmanaged_callback);
		}

		public CheckWatcher (Loop loop, CheckWatcherCallback callback) : base (loop)
		{ 
			this.callback = callback;
			
			unmanaged_watcher = new UnmanagedCheckWatcher ();
			unmanaged_watcher.callback = unmanaged_callback_ptr;

			InitializeUnmanagedWatcher (unmanaged_watcher);
		}

		
		private static void StaticCallback (IntPtr loop, IntPtr watcher, EventTypes revents)
		{
			UnmanagedCheckWatcher iow = (UnmanagedCheckWatcher) Marshal.PtrToStructure (watcher, typeof (UnmanagedCheckWatcher));

			GCHandle gchandle = GCHandle.FromIntPtr (iow.data);
			CheckWatcher w = (CheckWatcher) gchandle.Target;

			w.callback (w.Loop, w, revents);
		}

		protected override void StartImpl ()
		{
			unmanaged_watcher.data = GCHandle.ToIntPtr (gc_handle);
			Marshal.StructureToPtr (unmanaged_watcher, watcher_ptr, false);
			
			ev_check_start (Loop.Handle, WatcherPtr);
		}
		
		protected override void StopImpl ()
		{			
			ev_check_stop (Loop.Handle, WatcherPtr);	
		}
		
		protected override void UnmanagedCallbackHandler (IntPtr _loop, IntPtr _watcher, EventTypes revents)
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
	public delegate void CheckWatcherCallback (LibEvLoop loop, CheckWatcher watcher, EventTypes revents);
	
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

