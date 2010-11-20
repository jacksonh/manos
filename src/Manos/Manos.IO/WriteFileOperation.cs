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
using System.Net;
using System.Net.Sockets;

namespace Manos.IO {

	public class WriteFileOperation : IWriteOperation {

		private WriteCallback callback;

		private FileStream file;
		private long file_offset;
		private long file_length;
		
		public WriteFileOperation (FileStream file, WriteCallback callback)
		{
			this.file = file;
			this.callback = callback;

			// TODO: async.  Don't think there is a good reason to do any locking here.
			file_length = file.Length;
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

		public void BeginWrite (IOStream stream)
		{
		}

		public void HandleWrite (IOStream stream)
		{
			while (file_offset < file_length) {
			      try {
				      Mono.Unix.Native.Syscall.sendfile (stream.socket.Handle.ToInt32 (), 
						      file.Handle.ToInt32 (), ref file_offset,
						      (ulong) (file_length - file_offset));
			      } catch (SocketException se) {
				      if (se.SocketErrorCode == SocketError.WouldBlock || se.SocketErrorCode == SocketError.TryAgain)
					      return;
				      stream.Close ();
			      } catch (Exception e) {
				      stream.Close ();
			      }
			}

			if (file_offset >= file_length)
				IsComplete = true;
		}

		public void EndWrite (IOStream stream)
		{
		}

		public bool Combine (IWriteOperation other)
		{
			return false;
		}
	}
}

