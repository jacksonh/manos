

using System;
using System.Runtime.InteropServices;
using System.Net.Sockets;


namespace Libev {

	public class IOWatcher : Watcher {

		private IntPtr fd;
		private IOWatcherCallback callback;

		private UnmanagedIOWatcher unmanaged_watcher;

		public IOWatcher (IntPtr fd, EventTypes types, Loop loop, IOWatcherCallback callback) : base (loop)
		{
			this.fd = fd;
			this.callback = callback;
			
			unmanaged_watcher = new UnmanagedIOWatcher ();
			
			unmanaged_watcher.fd = fd.ToInt32 ();
			unmanaged_watcher.events = types | EventTypes.EV__IOFDSET;

			unmanaged_watcher.callback = CallbackFunctionPtr;

			InitializeUnmanagedWatcher (unmanaged_watcher);
		}

        
        public static IntPtr GetHandle(Socket handle) {
            if (Loop.IsWindows) {
                return EV_WIN32_HANDLE_TO_FD (handle.Handle, 0);
            } else
                return handle.Handle;
        }

        public static void ReleaseHandle(Socket socket, IntPtr handle)
        {
            if (Loop.IsWindows)
            {
                EV_WIN32_CLOSE_FD(handle);
            }
            else
                socket.Close();
        }

		public IntPtr FileHandle {
			get { return fd; }
		}

		protected override void StartImpl ()
		{
			ev_io_start (Loop.Handle, WatcherPtr);
		}
		
		protected override void StopImpl ()
		{
			ev_io_stop (Loop.Handle, WatcherPtr);	
		}
		
		protected override void UnmanagedCallbackHandler (IntPtr _loop, IntPtr _watcher, EventTypes revents)
		{
			// Maybe I should verify the pointers?
			callback (Loop, this, revents);
		}

        [DllImport ("libev", CallingConvention = CallingConvention.Cdecl)]
		private static extern void ev_io_start (IntPtr loop, IntPtr watcher);

        [DllImport ("libev", CallingConvention = CallingConvention.Cdecl)]
		private static extern void ev_io_stop (IntPtr loop, IntPtr watcher);

        [DllImport ("libev", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr EV_WIN32_HANDLE_TO_FD (IntPtr handle, int flags);

        [DllImport ("libev", CallingConvention = CallingConvention.Cdecl)]
        private static extern void EV_WIN32_CLOSE_FD (IntPtr handle);
	}
	
    [UnmanagedFunctionPointer (System.Runtime.InteropServices.CallingConvention.Cdecl)]
	public delegate void IOWatcherCallback (Loop loop, IOWatcher watcher, EventTypes revents);
	
	[StructLayout (LayoutKind.Sequential)]
	internal struct UnmanagedIOWatcher {
		
		public int active;
		public int pending;
		public int priority;
		
		public IntPtr data;
		public IntPtr callback;
		
		internal IntPtr next;
		
		public int fd;
		public EventTypes events;
	}
}

