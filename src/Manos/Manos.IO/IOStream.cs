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


namespace Manos.IO {

	public delegate void ReadCallback (IOStream stream, byte [] data, int offset, int count);
	public delegate void WriteCallback ();

	public abstract class IOStream {

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
			read_watcher.Start ();
		}

		public void EnableWriting ()
		{
			write_watcher.Start ();
		}

		public void DisableReading ()
		{
			read_watcher.Stop ();
		}

		public void DisableWriting ()
		{
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

			handle = IntPtr.Zero;

			foreach (IWriteOperation op in write_ops) {
				op.Dispose ();
			}
			
			if (Closed != null)
				Closed (this, EventArgs.Empty);
		}

		private void HandleIOReadEvent (Loop loop, IOWatcher watcher, EventTypes revents)
		{
			Expires = DateTime.UtcNow + TimeOut;

			// Happens after a close
			if (handle == IntPtr.Zero)
				return;
			
			HandleRead ();
		}

		
		private void HandleIOWriteEvent (Loop loop, IOWatcher watcher, EventTypes revents)
		{
			// Happens after a close
			if (handle == IntPtr.Zero)
				return;

			Expires = DateTime.UtcNow + TimeOut;
			HandleWrite ();
		}

		private void HandleTimeoutEvent (Loop loop, TimerWatcher watcher, EventTypes revents)
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

			current_write_op.HandleWrite (this);

			if (current_write_op.IsComplete)
				FinishCurrentWrite ();
		}

		/// This could use some tuning, but the basic idea is that we need to remove
		/// all of the data that has been sent already.
		public static void AdjustSegments (int len, IList<ByteBuffer> write_data)
		{
			var remove = new List<ByteBuffer>  ();
			int total = 0;
			for (int i = 0; i < write_data.Count; i++) {
				int seg_len = write_data [i].Length;
				if (total + seg_len <= len) {
					// The entire segment was written so we can pop it 
					remove.Add (write_data [i]);

					// If we finished exactly at the end of this segment we are done adjusting
					if (total + seg_len == len)
						break;
				} else if (total + seg_len > len) {
					// Move to the point in the segment where we stopped writing

					int offset = write_data [i].Position + (len - total);
					write_data [i].Position = offset;
					write_data [i].Length = write_data [i].Bytes.Length - offset;
					break;
				}
					
				total += seg_len;
			}

			foreach (var segment in remove) {
				write_data.Remove (segment);
			}
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

