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


// WARNING: This whole file is just a hack so people can develop/debug
// on windows. Once there is a async API for loading / streaming files
// we need to use that instead of this file, which blocks to load the
// entire file and queues it.

namespace Manos.IO {

#if DISABLE_POSIX
	public class WriteFileOperation : IWriteOperation {

		private WriteCallback callback;

		private byte [] bytes;
		private int bytes_index;

		private FileStream file;
		private int file_length;
		
		public WriteFileOperation (FileStream file, WriteCallback callback)
		{
			this.file = file;
			this.callback = callback;

			file_length = (int) file.Length;
			ReadFile ();
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
			SocketStream sstream = (SocketStream) stream;

			while (bytes_index < file_length) {
				int len = -1;
				try {
					len = sstream.socket.Send (bytes, bytes_index, file_length, SocketFlags.None);
				} catch (SocketException se) {
					if (se.SocketErrorCode == SocketError.WouldBlock || se.SocketErrorCode == SocketError.TryAgain)
						return;
					sstream.Close ();
				} catch (Exception e) {
					sstream.Close ();
				} finally {
					if (len != -1)
						bytes_index += len;
				}
			}

			IsComplete = (bytes_index == file_length);
		}

		public void EndWrite (IOStream stream)
		{
			if (Callback != null)
				Callback ();
		}

		public bool Combine (IWriteOperation other)
		{
			return false;
		}

		private void ReadFile ()
		{
			bytes = new byte [file_length];

			file.Read (bytes, 0, bytes.Length);
		}
	}

#endif
}

