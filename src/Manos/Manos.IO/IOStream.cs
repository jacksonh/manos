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
using System.Net.Sockets;
using System.Collections;
using System.Collections.Generic;

using Libev;


namespace Manos.IO {

	public delegate void CloseCallback (IOStream stream);
	public delegate void ReadCallback (IOStream stream, byte [] data, int offset, int count);
	public delegate void WriteCallback ();

	public class IOStream {

		private static int ReadChunkSize = 3072;
		private static byte [] ReadChunk = new byte [3072];

		internal Socket socket;
		private IOLoop ioloop;

		private CloseCallback close_callback;
		private ReadCallback read_callback;

		private IOWatcher read_watcher;
		private IOWatcher write_watcher;
		private IntPtr handle;

		private IWriteOperation current_write_op;
		private Queue<IWriteOperation> write_ops = new Queue<IWriteOperation> ();

		public IOStream (Socket socket, IOLoop ioloop)
		{
			this.socket = socket;
			this.ioloop = ioloop;

			TimeOut = TimeSpan.FromMinutes (2);
			Expires = DateTime.UtcNow + TimeOut;

			socket.Blocking = false;

			handle = IOWatcher.GetHandle (socket);
			read_watcher = new IOWatcher (handle, EventTypes.Read, ioloop.EventLoop, HandleIOReadEvent);
			write_watcher = new IOWatcher (handle, EventTypes.Write, ioloop.EventLoop, HandleIOWriteEvent);
		}

		~IOStream ()
		{
			Close ();
		}

		public IOLoop IOLoop {
			get { return ioloop; }
		}

		public DateTime Expires {
			get;
			set;
		}

		public TimeSpan TimeOut {
			get;
			private set;
		}

		public bool IsClosed {
			get { return socket == null || !socket.Connected; }
		}

		public void OnClose (CloseCallback callback)
		{
			this.close_callback = callback;
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

		public void Close ()
		{			
			if (socket == null)
				return;			

			DisableReading ();
			DisableWriting ();

			IOWatcher.ReleaseHandle (socket, handle);

			handle = IntPtr.Zero;
			socket = null;

			if (close_callback != null)
				close_callback (this);
		}

		private void HandleIOReadEvent (Loop loop, IOWatcher watcher, EventTypes revents)
		{
			// Happens after a close
			if (socket == null)
				return;

			Expires = DateTime.UtcNow + TimeOut;
			HandleRead ();
		}

		
		private void HandleIOWriteEvent (Loop loop, IOWatcher watcher, EventTypes revents)
		{
			// Happens after a close
			if (socket == null)
				return;

			Expires = DateTime.UtcNow + TimeOut;
			HandleWrite ();
		}
		
		private void HandleWrite ()
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

		private void HandleRead ()
		{
			int size;

			try {
				size = socket.Receive (ReadChunk);
			} catch (SocketException se) {
				if (se.SocketErrorCode == SocketError.WouldBlock || se.SocketErrorCode == SocketError.TryAgain)
					return;
				Close ();
				return;
			} catch (Exception e) {
			  	Console.WriteLine (e);
				Close ();
				return;
			}

			if (size == 0) {
				Close ();
				return;
			}

			read_callback (this, ReadChunk, 0, size);
		}

		/// This could use some tuning, but the basic idea is that we need to remove
		/// all of the data that has been sent already.
		public static void AdjustSegments (int len, IList<ArraySegment<byte>> write_data)
		{
			var remove = new List<ArraySegment<byte>>  ();
			int total = 0;
			for (int i = 0; i < write_data.Count; i++) {
				int seg_len = write_data [i].Count;
				if (total + seg_len <= len) {
					// The entire segment was written so we can pop it 
					remove.Add (write_data [i]);

					// If we finished exactly at the end of this segment we are done adjusting
					if (total + seg_len == len)
						break;
				} else if (total + seg_len > len) {
					// Move to the point in the segment where we stopped writing

					int offset = write_data [i].Offset + (len - total);
					write_data [i] = new ArraySegment<byte> (write_data [i].Array,
							offset,
							write_data [i].Array.Length - offset);
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
	}

}

