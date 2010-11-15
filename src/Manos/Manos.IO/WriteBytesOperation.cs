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

namespace Manos.IO {

	public class WriteBytesOperation : IWriteOperation {

		private IList<ArraySegment<byte>> bytes;
		private WriteCallback callback;
		
		public WriteBytesOperation (IList<ArraySegment<byte>> bytes, WriteCallback callback)
		{
			this.bytes = bytes;
			this.callback = callback;
		}

		public IList<ArraySegment<byte>> Bytes {
			get { return bytes; }
			set {
				if (value == null)
					throw new ArgumentNullException ("value");
				bytes = value;
			}
		}

		public WriteCallback Callback {
			get { return callback; }
			set {
				if (value == null)
					throw new ArgumentNullException ("value");
				callback = value;
			}
		}

		public bool IsComplete {
			get;
			private set;
		}

		public bool Combine (IWriteOperation other)
		{
			WriteBytesOperation write_op = other as WriteBytesOperation;
			if (write_op == null)
				return false;
			foreach (var op in write_op.Bytes) {
				Bytes.Add (op);
			}
			return true;
		}

		public void BeginWrite (IOStream stream)
		{
		}

		public void HandleWrite (IOStream stream)
		{
			while (bytes.Count > 0) {
				int len = -1;
				try {
					len = stream.socket.Send (bytes);
					Console.WriteLine ("wrote:  '{0}'", len);
				} catch (SocketException se) {
					if (se.SocketErrorCode == SocketError.WouldBlock || se.SocketErrorCode == SocketError.TryAgain)
						return;
					stream.Close ();
				} catch (Exception e) {
					stream.Close ();
				} finally {
					if (len != -1)
						IOStream.AdjustSegments (len, bytes);
				}
			}

			IsComplete = (bytes.Count == 0);
		}

		public void EndWrite (IOStream stream)
		{
		}
	}
}

