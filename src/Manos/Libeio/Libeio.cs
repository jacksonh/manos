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

using Libev;
using Manos;

namespace Libeio {

	public class Libeio : IDisposable {

		private IdleWatcher idle_watcher;
		private AsyncWatcher want_poll_watcher;
		private AsyncWatcher done_poll_watcher;
		
		public void Initialize (LibEvLoop loop)
		{
/*
			idle_watcher = new IdleWatcher (loop, OnIdle);
			want_poll_watcher = new AsyncWatcher (loop, OnWantPoll);
			done_poll_watcher = new AsyncWatcher (loop, OnDonePoll);

			idle_watcher.Start ();
			want_poll_watcher.Start ();
			done_poll_watcher.Start ();
*/
		}

		public void Dispose ()
		{
			if (idle_watcher != null) {
				idle_watcher.Dispose ();
				idle_watcher = null;
			}

			if (want_poll_watcher != null) {
				want_poll_watcher.Dispose ();
				want_poll_watcher = null;
			}

			if (done_poll_watcher != null) {
				want_poll_watcher.Dispose ();
				done_poll_watcher = null;
			}
		}

		private void OnIdle (Loop loop, IdleWatcher watcher, EventTypes revents)
		{
			Console.WriteLine ("ON IDLE");

			if (eio_poll () != -1) {
				Console.WriteLine ("OnIdle: Stopping idle watcher");
				idle_watcher.Stop ();
			}
		}

        private void OnWantPoll(Loop loop, AsyncWatcher watcher, EventTypes revents)
		{
			if (eio_poll () == -1) {
				Console.WriteLine ("OnWantPoll: starting idle watcher");
				idle_watcher.Start ();
			}
		}

        private void OnDonePoll(Loop loop, AsyncWatcher watcher, EventTypes revents)
		{
			if (eio_poll () != -1) {
				Console.WriteLine ("OnDonePoll: starting idle watcher");
				idle_watcher.Stop ();
			}
		}

		private void EIOWantPoll ()
		{
			Console.WriteLine ("want poll");
			want_poll_watcher.Send ();
		}

		private void EIODonePoll ()
		{
			Console.WriteLine ("done poll");
			done_poll_watcher.Send ();
		}

		public void stat (string path, Action callback)
		{
			eio_stat (path, 1, unmanaged_eio_callback, IntPtr.Zero);
		}

		public void rename (string path, string new_path, Action<bool> callback)
		{
			eio_rename (path, new_path, 1, unmanaged_eio_callback, IntPtr.Zero);
		}

		public void symlink (string path, string new_path, Action<bool> callback)
		{
			eio_symlink (path, new_path, 1, unmanaged_eio_callback, IntPtr.Zero);
		}

		public void unlink (string path, Action<bool> callback)
		{
			eio_unlink (path, 1, unmanaged_eio_callback, IntPtr.Zero);
		}

		private static int unmanaged_eio_callback (IntPtr req)
		{
			Console.WriteLine ("GOT THE EIO CALLBACK!");

			return 0;
		}

		[DllImport ("libeio", CallingConvention = CallingConvention.Cdecl)]
		private static extern int eio_poll ();

		[DllImport ("libeio", CallingConvention = CallingConvention.Cdecl)]
		private static extern int eio_init (eio_want_poll want_poll, eio_done_poll done_poll);

		[DllImport ("libeio", CallingConvention = CallingConvention.Cdecl)]
		private static extern IntPtr eio_stat (string path, int pri, eio_cb cb, IntPtr data); /* stat buffer=ptr2 allocated dynamically */

		[DllImport ("libeio", CallingConvention = CallingConvention.Cdecl)]
		private static extern IntPtr eio_rename (string path, string new_path, int pri, eio_cb cb, IntPtr data);

		[DllImport ("libeio", CallingConvention = CallingConvention.Cdecl)]
		private static extern IntPtr eio_symlink (string path, string new_path, int pri, eio_cb cb, IntPtr data);

		[DllImport ("libeio", CallingConvention = CallingConvention.Cdecl)]
		private static extern IntPtr eio_unlink (string path, int pri, eio_cb cb, IntPtr data);
	}

	internal delegate int eio_cb (IntPtr req);
	internal delegate void eio_req_destroy (ref eio_req req);

	internal delegate void eio_poll ();
	internal delegate void eio_want_poll ();
	internal delegate void eio_done_poll ();
	
	[StructLayout (LayoutKind.Sequential)]
	internal struct eio_req {

		public IntPtr next;  /* private ETP */

		public int result;  /* result of syscall, e.g. result = read (... */
		public long offs;      /* read, write, truncate, readahead, sync_file_range: file offset */
		public uint size;     /* read, write, readahead, sendfile, msync, mlock, sync_file_range: length */
		public IntPtr ptr1;      /* all applicable requests: pathname, old name; readdir: optional eio_dirents */
		public IntPtr ptr2;      /* all applicable requests: new name or memory buffer; readdir: name strings */
		public double nv1;  /* utime, futime: atime; busy: sleep time */
		public double nv2;  /* utime, futime: mtime */

		public int type;        /* EIO_xxx constant ETP */
		public int int1;        /* all applicable requests: file descriptor; sendfile: output fd; open, msync, mlockall, readdir: flags */
		public long int2;       /* chown, fchown: uid; sendfile: input fd; open, chmod, mkdir, mknod: file mode, sync_file_range: flags */
		public long int3;       /* chown, fchown: gid; mknod: dev_t */
		public int errorno;     /* errno value on syscall return */

		public byte flags; /* private */
		public byte pri;     /* the priority */

		public IntPtr data;
		public eio_cb finish;
		public eio_req_destroy destroy;
		public IntPtr feed; // void (*feed)(eio_req *req);    /* only used for group requests */

		public IntPtr grp;
		public IntPtr grp_prev;
		public IntPtr grp_next;
		public IntPtr grp_first;
	};
}

