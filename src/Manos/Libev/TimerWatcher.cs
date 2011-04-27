using System;
using System.Runtime.InteropServices;

namespace Libev
{
	public class TimerWatcher : Watcher
	{
		private TimerWatcherCallback callback;
		private TimeSpan repeat;
		private static UnmanagedWatcherCallback unmanaged_callback;

		static TimerWatcher ()
		{
			unmanaged_callback = StaticCallback;
		}

		public TimerWatcher (TimeSpan repeat, LibEvLoop loop, TimerWatcherCallback callback)
			: this (TimeSpan.Zero, repeat, loop, callback)
		{
		}

		public TimerWatcher (TimeSpan after, TimeSpan repeat, LibEvLoop loop, TimerWatcherCallback callback)
			: base (loop)
		{
			this.callback = callback;
			this.repeat = repeat;
			watcher_ptr = manos_timer_watcher_create (after.TotalSeconds, repeat.TotalSeconds, unmanaged_callback, GCHandle.ToIntPtr (gc_handle));
		}

		public TimeSpan Repeat {
			get { return repeat; }
			set {
				if (value < TimeSpan.Zero)
					throw new ArgumentException ("value");
				repeat = value;
				manos_timer_watcher_set (watcher_ptr, 0, repeat.TotalSeconds);
			}
		}

		public void Again ()
		{
			ev_timer_again (Loop.Handle, watcher_ptr);
		}

		protected override void DestroyWatcher ()
		{
			manos_timer_watcher_destroy (watcher_ptr);
		}

		private static void StaticCallback (IntPtr data, EventTypes revents)
		{
			try {
				var handle = GCHandle.FromIntPtr (data);
				var watcher = (TimerWatcher) handle.Target;
				watcher.callback (watcher.Loop, watcher, revents);
			} catch (Exception e) {
				Console.Error.WriteLine ("Error handling timer event: {0}", e.Message);
				Console.Error.WriteLine (e.StackTrace);
			}
		}

		protected override void StartImpl ()
		{
			ev_timer_start (Loop.Handle, watcher_ptr);
		}

		protected override void StopImpl ()
		{
			ev_timer_stop (Loop.Handle, watcher_ptr);
		}

		[DllImport("libev", CallingConvention = CallingConvention.Cdecl)]
		private static extern void ev_timer_start (IntPtr loop, IntPtr watcher);

		[DllImport("libev", CallingConvention = CallingConvention.Cdecl)]
		private static extern void ev_timer_again (IntPtr loop, IntPtr watcher);

		[DllImport("libev", CallingConvention = CallingConvention.Cdecl)]
		private static extern void ev_timer_stop (IntPtr loop, IntPtr watcher);

		[DllImport("libmanos", CallingConvention = CallingConvention.Cdecl)]
		private static extern IntPtr manos_timer_watcher_create (double after, double repeat, UnmanagedWatcherCallback callback, IntPtr data);

		[DllImport("libmanos", CallingConvention = CallingConvention.Cdecl)]
		private static extern IntPtr manos_timer_watcher_set (IntPtr watcher, double after, double repeat);

		[DllImport("libmanos", CallingConvention = CallingConvention.Cdecl)]
		private static extern void manos_timer_watcher_destroy (IntPtr watcher);
	}

	[UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
	public delegate void TimerWatcherCallback (Manos.Loop loop, TimerWatcher watcher, EventTypes revents);
}
