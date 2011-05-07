using System;
using System.Runtime.InteropServices;
using System.Net.Sockets;

namespace Libev
{
	class IOWatcher : Watcher
	{
		private IntPtr fd;
		private Action<IOWatcher, EventTypes> callback;
		private static UnmanagedWatcherCallback watcherCallback;

		static IOWatcher ()
		{
			watcherCallback = StaticCallback;
		}

		public IOWatcher (IntPtr fd, EventTypes types, Loop loop, Action<IOWatcher, EventTypes> callback) : base(loop)
		{
			this.fd = fd;
			this.callback = callback;
			watcher_ptr = manos_io_watcher_create (fd.ToInt32 (), types, watcherCallback, GCHandle.ToIntPtr (gc_handle));
		}

		protected override void DestroyWatcher ()
		{
			manos_io_watcher_destroy (watcher_ptr);
		}

		private static void StaticCallback (IntPtr data, EventTypes revents)
		{
			try {
				var handle = GCHandle.FromIntPtr (data);
				var watcher = (IOWatcher) handle.Target;
				watcher.callback (watcher, revents);
			} catch (Exception e) {
				Console.Error.WriteLine ("Error handling IO readyness event: {0}", e.Message);
				Console.Error.WriteLine (e.StackTrace);
			}
		}

		public IntPtr FileHandle { get { return fd; } }

		protected override void StartImpl ()
		{
			ev_io_start (Loop.Handle, watcher_ptr);
		}

		protected override void StopImpl ()
		{
			ev_io_stop (Loop.Handle, watcher_ptr);
		}
		
		[DllImport("libev", CallingConvention = CallingConvention.Cdecl)]
		private static extern void ev_io_start (IntPtr loop, IntPtr watcher);
		
		[DllImport("libev", CallingConvention = CallingConvention.Cdecl)]
		private static extern void ev_io_stop (IntPtr loop, IntPtr watcher);
		
		[DllImport("libmanos", CallingConvention = CallingConvention.Cdecl)]
		private static extern IntPtr manos_io_watcher_create (int fd, EventTypes revents, UnmanagedWatcherCallback callback, IntPtr data);
		
		[DllImport("libmanos", CallingConvention = CallingConvention.Cdecl)]
		private static extern void manos_io_watcher_destroy (IntPtr watcher);
	}
}
