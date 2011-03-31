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

		private ByteBuffer[] buffers;
		private int bufferOffset;
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

		public SendBytesOperation (ByteBuffer[] buffers, WriteCallback callback)
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
			return false;
		}

		public void BeginWrite (IOStream stream)
		{
		}

		private ByteBufferS [] CreateBufferSArray ()
		{
			ByteBufferS [] b = new ByteBufferS [buffers.Length - bufferOffset];

			for (int i = bufferOffset; i < buffers.Length; i++) {
				b [i - bufferOffset] = buffers [i].buffer;
			}

			return b;
		}

		public void HandleWrite (IOStream stream)
		{
			SocketStream sstream = (SocketStream) stream;
			
			while (this.buffers.Length > bufferOffset) {
				int len = -1;
				int error;
				ByteBufferS [] bs = CreateBufferSArray ();
				len = sstream.Send (bs, bs.Length, out error);

				if (len < 0 && error == 0)
					return;

				if (len != -1) {
					int num_segments = buffers.Length - bufferOffset;
					AdjustSegments (len);
					segments_written = num_segments - buffers.Length - bufferOffset;
				}
			}

			FireCallbacks ();
			IsComplete = (buffers.Length == bufferOffset);
		}

		void AdjustSegments (int len)
		{
			while (len > 0 && bufferOffset < buffers.Length) {
				int seg_len = buffers [bufferOffset].Length;
				if (seg_len <= len) {
					buffers [bufferOffset] = null;
					bufferOffset++;
				} else {
					int offset = buffers [bufferOffset].Position + len;
					buffers [bufferOffset].Position = offset;
					buffers [bufferOffset].Length = buffers [bufferOffset].Bytes.Length - offset;
					break;
				}
				len -= seg_len;
			}
		}

		public void EndWrite (IOStream stream)
		{
		}

		private void FireCallbacks ()
		{
			if (buffers.Length == bufferOffset) {
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

