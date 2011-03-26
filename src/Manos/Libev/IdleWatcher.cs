//
// Copyright (C) 2010 Jackson Harper (jackson@manosdemono.com)
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//
//


using System;
using System.Runtime.InteropServices;
using System.Net.Sockets;
using Manos;


namespace Libev {

	public class IdleWatcher : Watcher {

		private IdleWatcherCallback callback;

		private UnmanagedIdleWatcher unmanaged_watcher;

		private static IntPtr unmanaged_callback_ptr;
		private static UnmanagedWatcherCallback unmanaged_callback;

		
		static IdleWatcher ()
		{
			unmanaged_callback = new UnmanagedWatcherCallback (StaticCallback);
			unmanaged_callback_ptr = Marshal.GetFunctionPointerForDelegate (unmanaged_callback);
		}

		public IdleWatcher (BaseLoop loop, IdleWatcherCallback callback) : base (loop)
		{
			this.callback = callback;
			
			unmanaged_watcher = new UnmanagedIdleWatcher ();
			unmanaged_watcher.callback = unmanaged_callback_ptr;

			InitializeUnmanagedWatcher (unmanaged_watcher);
		}

		private static void StaticCallback (IntPtr loop, IntPtr watcher, EventTypes revents)
		{
			UnmanagedIdleWatcher iow = (UnmanagedIdleWatcher) Marshal.PtrToStructure (watcher, typeof (UnmanagedIdleWatcher));

			GCHandle gchandle = GCHandle.FromIntPtr (iow.data);
			IdleWatcher w = (IdleWatcher) gchandle.Target;

			w.callback (w.Loop, w, revents);
		}

		protected override void StartImpl ()
		{
			unmanaged_watcher.data = GCHandle.ToIntPtr (gc_handle);
			Marshal.StructureToPtr (unmanaged_watcher, watcher_ptr, false);
			
			ev_idle_start (Loop.Handle, WatcherPtr);
		}
		
		protected override void StopImpl ()
		{
			ev_idle_stop (Loop.Handle, WatcherPtr);	
		}
		
		protected override void UnmanagedCallbackHandler (IntPtr _loop, IntPtr _watcher, EventTypes revents)
		{
			// Maybe I should verify the pointers?
			callback (Loop, this, revents);
		}

		[DllImport ("libev", CallingConvention = CallingConvention.Cdecl)]
		private static extern void ev_idle_start (IntPtr loop, IntPtr watcher);

		[DllImport ("libev", CallingConvention = CallingConvention.Cdecl)]
		private static extern void ev_idle_stop (IntPtr loop, IntPtr watcher);
	}
	
	[UnmanagedFunctionPointer (System.Runtime.InteropServices.CallingConvention.Cdecl)]
	public delegate void IdleWatcherCallback (BaseLoop loop, IdleWatcher watcher, EventTypes revents);
	
	[StructLayout (LayoutKind.Sequential)]
	internal struct UnmanagedIdleWatcher {
		
		public int active;
		public int pending;
		public int priority;
		
		public IntPtr data;
		public IntPtr callback;
	}
}

