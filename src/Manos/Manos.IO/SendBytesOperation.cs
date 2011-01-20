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
using System.Net.Sockets;
using System.Collections;
using System.Collections.Generic;

using Manos;
using Manos.Collections;


namespace Manos.IO {

	

	public class SendBytesOperation : IWriteOperation {

		private List<ByteBuffer> buffers;
		private WriteCallback callback;

		private class CallbackInfo {
			public int Index;
			public WriteCallback Callback;

			public CallbackInfo (int index, WriteCallback callback)
			{
				Index = index;
				Callback = callback;
			}
		}

		private int segments_written;
		private List<CallbackInfo> callbacks;

		public SendBytesOperation (List<ByteBuffer> buffers, WriteCallback callback)
		{
			this.buffers = buffers;
			this.callback = callback;
		}

		public void Dispose ()
		{
		}

		public bool IsComplete {
			get;
			private set;
		}

		public bool Combine (IWriteOperation other)
		{
			SendBytesOperation send_op = other as SendBytesOperation;
			if (send_op == null)
				return false;

			int offset = buffers.Count;
			foreach (var op in send_op.buffers) {
				buffers.Add (op);
			}

			if (send_op.callback != null) {
				if (callback == null && callbacks == null)
					callback = send_op.callback;
				else {
					if (callbacks == null) {
						callbacks = new List<CallbackInfo> ();
						callbacks.Add (new CallbackInfo (offset - 1, callback));
						callback = null;
					}
					callbacks.Add (new CallbackInfo (buffers.Count - 1, send_op.callback));
				}
			}

			return true;
		}

		public void BeginWrite (IOStream stream)
		{
		}

		private ByteBufferS [] CreateBufferSArray ()
		{
			ByteBufferS [] b = new ByteBufferS [buffers.Count];

			for (int i = 0; i < buffers.Count; i++) {
				b [i] = buffers [i].buffer;
			}

			return b;
		}

		public void HandleWrite (IOStream stream)
		{
			SocketStream sstream = (SocketStream) stream;
			
			while (this.buffers.Count > 0) {
				int len = -1;
				int error;
				ByteBufferS [] bs = CreateBufferSArray ();
				len = sstream.Send (bs, bs.Length, out error);

				if (len < 0 && error == 0)
					return;

				if (len != -1) {
					int num_segments = buffers.Count;
					IOStream.AdjustSegments (len, buffers);
					segments_written = num_segments - buffers.Count;
				}
			}

			FireCallbacks ();
			IsComplete = (buffers.Count == 0);
		}

		public void EndWrite (IOStream stream)
		{
		}

		private void FireCallbacks ()
		{
			if (buffers.Count == 0) {
				FireAllCallbacks ();
				return;
			}

			if (callbacks == null)
				return;

			while (callbacks.Count > 0) {
				CallbackInfo c = callbacks [0];
				if (c.Index < segments_written)
					break;
				c.Callback ();

				callbacks.RemoveAt (0);
			}
		}

		private void FireAllCallbacks ()
		{
			if (callback != null) {
				callback ();
				return;
			}

			if (callbacks == null)
				return;

			callbacks.ForEach (c => c.Callback ());
		}
	}
}

