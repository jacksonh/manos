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
using System.IO;
using System.Linq;
using System.Text;
using System.Net;
using System.Collections;
using System.Collections.Generic;

using Libev;
using Manos.Collections;


namespace Manos.IO.Libev {

	public abstract class IOStream: Manos.IO.IOStream {

		protected static int ReadChunkSize = 3072;
		protected static byte [] ReadChunk = new byte [3072];

		protected ReadCallback read_callback;

		private IOLoop ioloop;
		private IOWatcher read_watcher;
		private IOWatcher write_watcher;
		private TimerWatcher timeout_watcher;
		private IntPtr handle;

		private IWriteOperation current_write_op;
		private Queue<IWriteOperation> write_ops = new Queue<IWriteOperation> ();

		public IOStream (IOLoop ioloop)
		{
			this.ioloop = ioloop;

			TimeOut = TimeSpan.FromMinutes (1);
			Expires = DateTime.UtcNow + TimeOut;
		}

		~IOStream ()
		{
			Close ();
		}

		public IOLoop IOLoop {
			get { return ioloop; }
		}

		public IntPtr Handle {
			get { return handle; }
		}

		public DateTime Expires {
			get;
			set;
		}

		public TimeSpan TimeOut {
			get;
			private set;
		}

		public void SetHandle (IntPtr handle)
		{
			if (this.handle != IntPtr.Zero && this.handle != handle)
				Close ();

			this.handle = handle;
			read_watcher = new IOWatcher (handle, EventTypes.Read, ioloop.EventLoop, HandleIOReadEvent);
			write_watcher = new IOWatcher (handle, EventTypes.Write, ioloop.EventLoop, HandleIOWriteEvent);
			timeout_watcher = new TimerWatcher (TimeOut, TimeOut, ioloop.EventLoop, HandleTimeoutEvent);

			timeout_watcher.Start ();
		}

		public void DisableTimeout ()
		{
			if (timeout_watcher != null)
				timeout_watcher.Stop ();
		}

		public void ReadBytes (ReadCallback callback)
		{
			EnableReading ();

			read_callback = callback;

			UpdateExpires ();
		}

		public void QueueWriteOperation (IWriteOperation op)
		{
			EnableWriting ();

			UpdateExpires ();

			// We try to combine the op in case they are both byte buffers
			// that could be sent as a single scatter/gather operation
			if (write_ops.Count < 1 || !write_ops.Last ().Combine (op))
				write_ops.Enqueue (op);

			if (current_write_op == null)
				current_write_op = write_ops.Dequeue ();
		}

		public void EnableReading ()
		{
			if (read_watcher != null)
				read_watcher.Start ();
		}

		public void EnableWriting ()
		{
			if (write_watcher != null)
				write_watcher.Start ();
		}

		public void DisableReading ()
		{
			if (read_watcher != null)
				read_watcher.Stop ();
		}

		public void DisableWriting ()
		{
			if (write_watcher != null)
				write_watcher.Stop ();
		}

		private void UpdateExpires ()
		{
			Expires = DateTime.UtcNow + TimeOut;
		}

		public virtual void Close ()
		{
			if (handle == IntPtr.Zero)
				return;			

			DisableReading ();
			DisableWriting ();

			read_watcher.Dispose ();
			write_watcher.Dispose ();
			timeout_watcher.Dispose ();

			read_watcher = null;
			write_watcher = null;
			timeout_watcher = null;
			handle = IntPtr.Zero;

			foreach (IWriteOperation op in write_ops) {
				op.Dispose ();
			}
			write_ops.Clear ();
			
			if (Closed != null)
				Closed (this, EventArgs.Empty);
			Closed = null;
			read_callback = null;
		}

		private void HandleIOReadEvent (LibEvLoop loop, IOWatcher watcher, EventTypes revents)
		{
			Expires = DateTime.UtcNow + TimeOut;

			// Happens after a close
			if (handle == IntPtr.Zero)
				return;
			
			HandleRead ();
		}

		
		private void HandleIOWriteEvent (LibEvLoop loop, IOWatcher watcher, EventTypes revents)
		{
			// Happens after a close
			if (handle == IntPtr.Zero)
				return;

			Expires = DateTime.UtcNow + TimeOut;
			HandleWrite ();
		}

		private void HandleTimeoutEvent (LibEvLoop loop, TimerWatcher watcher, EventTypes revents)
		{
			if (Expires <= DateTime.UtcNow) {
				if (TimedOut != null)
					TimedOut (this, EventArgs.Empty);
				Close ();
			}
		}

		protected virtual void HandleWrite ()
		{
			if (current_write_op == null) {
				// Kinda shouldn't happen...
				DisableWriting ();
				return;
			}

			if (current_write_op.IsComplete) {
				FinishCurrentWrite ();
				return;
			}

			current_write_op.HandleWrite (this);

			if (current_write_op.IsComplete)
				FinishCurrentWrite ();
		}

		
		private void FinishCurrentWrite ()
		{
			if (current_write_op == null)
				return;

			current_write_op.EndWrite (this);

			if (write_ops.Count > 0) {
				IWriteOperation op = write_ops.Dequeue ();
				op.BeginWrite (this);
				current_write_op = op;
			} else {
				current_write_op = null;
				DisableWriting ();
			}
			
		}

		protected abstract void HandleRead ();

		public event EventHandler Error;
		public event EventHandler Closed;
		public event EventHandler TimedOut;
	}

}

