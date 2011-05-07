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
using Manos.IO;

namespace Libev
{
	class AsyncWatcher : Watcher, IAsyncWatcher
	{
		private Action<AsyncWatcher, EventTypes> callback;
		private static UnmanagedWatcherCallback unmanaged_callback;

		static AsyncWatcher ()
		{
			unmanaged_callback = StaticCallback;
		}

		public AsyncWatcher (Loop loop, Action<AsyncWatcher, EventTypes> callback)
			: base(loop)
		{
			this.callback = callback;
			watcher_ptr = manos_async_watcher_create (unmanaged_callback, GCHandle.ToIntPtr (gc_handle));
		}

		protected override void DestroyWatcher ()
		{
			manos_async_watcher_destroy (watcher_ptr);
		}

		private static void StaticCallback (IntPtr data, EventTypes revents)
		{
			try {
				var handle = GCHandle.FromIntPtr (data);
				var watcher = (AsyncWatcher) handle.Target;
				watcher.callback (watcher, revents);
			} catch (Exception e) {
				Console.Error.WriteLine ("Error handling async event: {0}", e.Message);
				Console.Error.WriteLine (e.StackTrace);
			}
		}

		public void Send ()
		{
			ev_async_send (Loop.Handle, watcher_ptr);
		}

		protected override void StartImpl ()
		{
			ev_async_start (Loop.Handle, watcher_ptr);
		}

		protected override void StopImpl ()
		{
			ev_async_stop (Loop.Handle, watcher_ptr);
		}

		[DllImport("libev", CallingConvention = CallingConvention.Cdecl)]
		private static extern void ev_async_start (IntPtr loop, IntPtr watcher);

		[DllImport("libev", CallingConvention = CallingConvention.Cdecl)]
		private static extern void ev_async_stop (IntPtr loop, IntPtr watcher);

		[DllImport("libev", CallingConvention = CallingConvention.Cdecl)]
		private static extern void ev_async_send (IntPtr loop, IntPtr watcher);

		[DllImport("libmanos", CallingConvention = CallingConvention.Cdecl)]
		private static extern IntPtr manos_async_watcher_create (UnmanagedWatcherCallback callback, IntPtr data);

		[DllImport("libmanos", CallingConvention = CallingConvention.Cdecl)]
		private static extern void manos_async_watcher_destroy (IntPtr watcher);
	}
}
