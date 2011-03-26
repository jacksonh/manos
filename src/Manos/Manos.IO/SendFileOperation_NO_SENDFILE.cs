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
using Manos.Collections;


// WARNING: This whole file is just a hack so people can develop/debug
// on windows. Once there is a async API for loading / streaming files
// we need to use that instead of this file, which blocks to load the
// entire file and queues it.

namespace Manos.IO {

#if DISABLE_POSIX
	public class SendFileOperation : IWriteOperation {

		private WriteCallback callback;

		private byte [] bytes;
		private int bytes_index;

		private FileStream file;
		private int file_length;
		
		public SendFileOperation (string filename, WriteCallback callback)
		{
            this.file = new FileStream (filename, FileMode.Open, FileAccess.Read);
			this.callback = callback;

			file_length = (int) file.Length;
			ReadFile ();
		}

		
		public void Dispose ()
		{
			if (file != null) {
				file.Close ();
				file = null;
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

		public bool Chunked {
			get;
			set;
		}
				
		public long Length {
			get { return file_length; }
			set { /* NOOP on windows because we set in the ctor */ }
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
			ISocketStream sstream = (ISocketStream) stream;

			while (bytes_index < file_length) {
				int len = -1;
				try {
                    int err;
                    
					len = sstream.Send (new ByteBufferS[] {new ByteBufferS(bytes, bytes_index, file_length)}, 1, out err);
                    if (err != 0)
                    {

                        if (err == (int)SocketError.WouldBlock || err == (int)SocketError.TryAgain)
                            return;
                        sstream.Close();
                    }
				} finally {
					if (len != -1)
						bytes_index += len;
				}
			}

			IsComplete = (bytes_index == file_length);

			if (IsComplete)
				OnComplete ();
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

        public void SetLength(long l)
        {
            Length = l;
        }
		
		private void OnComplete ()
		{
			if (Completed != null)
				Completed (this, EventArgs.Empty);
		}

		public event EventHandler Completed;
	}

#endif
}

