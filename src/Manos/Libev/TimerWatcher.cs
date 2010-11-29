using System;
using System.Runtime.InteropServices;

namespace Libev {

	public class TimerWatcher : Watcher {

		private TimerWatcherCallback callback;
		private UnmanagedTimerWatcher unmanaged_watcher;
		
		public TimerWatcher (TimeSpan repeat, Loop loop, TimerWatcherCallback callback) : this (TimeSpan.Zero, repeat, loop, callback)
		{
		}
		
		public TimerWatcher (TimeSpan after, TimeSpan repeat, Loop loop, TimerWatcherCallback callback) : base (loop)
		{	
			this.callback = callback;
			
			unmanaged_watcher = new UnmanagedTimerWatcher ();
			
			unmanaged_watcher.callback = CallbackFunctionPtr;
			unmanaged_watcher.after = after.TotalSeconds;
			unmanaged_watcher.repeat = after.TotalSeconds;
			
			InitializeUnmanagedWatcher (unmanaged_watcher);
		}
		
		/*
		public TimeSpan Repeat {
			get {
				return TimeSpan.FromSeconds (unmanaged_watcher.repeat);	
			}
			set {
				unmanaged_watcher.repeat = value.TotalSeconds;	
			}
		}
		*/
		
		protected override void StartImpl ()
		{
			ev_timer_start (Loop.Handle, WatcherPtr);
		}
		
		protected override void StopImpl ()
		{
			ev_timer_stop (Loop.Handle, WatcherPtr);
		}
		
		protected override void UnmanagedCallbackHandler (IntPtr loop, IntPtr watcher, EventTypes revents)
		{
			callback (Loop, this, revents);
		}

        [DllImport ("libev", CallingConvention = CallingConvention.Cdecl)]
		private static extern void ev_timer_start (IntPtr loop, IntPtr watcher);

        [DllImport ("libev", CallingConvention = CallingConvention.Cdecl)]
		private static extern void ev_timer_stop (IntPtr loop, IntPtr watcher);
	}
	
    [UnmanagedFunctionPointer (System.Runtime.InteropServices.CallingConvention.Cdecl)]
	public delegate void TimerWatcherCallback (Loop loop, TimerWatcher watcher, EventTypes revents);
	
	[StructLayout (LayoutKind.Sequential)]
	internal struct UnmanagedTimerWatcher {
		
		public int active;
		public int pending;
		public int priority;
		
		public IntPtr data;
		public IntPtr callback;
				
		public double after;
		public double repeat;
	}
}

