using System;
using System.Runtime.InteropServices;
using System.Net.Sockets;
using Manos.IO;

namespace Libev
{
	class IdleWatcher : Watcher, IIdleWatcher
	{
		private Action<IdleWatcher, EventTypes> callback;
		private static UnmanagedWatcherCallback unmanaged_callback;

		static IdleWatcher ()
		{
			unmanaged_callback = StaticCallback;
		}

		public IdleWatcher (Loop loop, Action<IdleWatcher, EventTypes> callback) : base (loop)
		{
			this.callback = callback;
			
			watcher_ptr = manos_idle_watcher_create (unmanaged_callback, GCHandle.ToIntPtr (gc_handle));
		}

		protected override void DestroyWatcher ()
		{
			manos_idle_watcher_destroy (watcher_ptr);
		}

		private static void StaticCallback (IntPtr data, EventTypes revents)
		{
			try {
				var handle = GCHandle.FromIntPtr (data);
				var watcher = (IdleWatcher) handle.Target;
				watcher.callback (watcher, revents);
			} catch (Exception e) {
				Console.Error.WriteLine ("Error handling idle event: {0}", e.Message);
				Console.Error.WriteLine (e.StackTrace);
			}
		}

		protected override void StartImpl ()
		{
			ev_idle_start (Loop.Handle, watcher_ptr);
		}

		protected override void StopImpl ()
		{
			ev_idle_stop (Loop.Handle, watcher_ptr);
		}

		[DllImport ("libev", CallingConvention = CallingConvention.Cdecl)]
		private static extern void ev_idle_start (IntPtr loop, IntPtr watcher);

		[DllImport ("libev", CallingConvention = CallingConvention.Cdecl)]
		private static extern void ev_idle_stop (IntPtr loop, IntPtr watcher);

		[DllImport ("libmanos", CallingConvention = CallingConvention.Cdecl)]
		private static extern IntPtr manos_idle_watcher_create (UnmanagedWatcherCallback callback, IntPtr data);

		[DllImport ("libmanos", CallingConvention = CallingConvention.Cdecl)]
		private static extern void manos_idle_watcher_destroy (IntPtr watcher);
	}
}

