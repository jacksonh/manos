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

#if !DISABLE_POSIX
	public class SendFileOperation : IWriteOperation {

		private WriteCallback callback;

		private FileStream file;
		private long file_offset;
		private long file_length;
		
		string filename;

		public SendFileOperation (string filename, long size, WriteCallback callback)
		{
			this.filename = filename;
			this.callback = callback;

			file_length = size;
		}

		~SendFileOperation ()
		{
			Dispose ();
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

			/*
			while (file_offset < file_length) {
				int fdin = -1;
				try {
					int error;
					fdin = Mono.Unix.Native.Syscall.open (filename,
							Mono.Unix.Native.OpenFlags.O_RDONLY);
					if (fdin == -1)
						return;
					int res = sstream.SendFile (fdin, file_offset, (int) file_length, out error);
					if (error != 0)
						throw new Exception (String.Format ("Send file error {0}.", error));

					file_offset += res;
					
				} finally {
					if (fdin != -1)
						Mono.Unix.Native.Syscall.close (fdin);
				}
			}
			*/
			int error;
			sstream.SendFile (filename, out error);
			sstream.DisableWriting ();

			Console.WriteLine ("FILE SENT:  " + error);
			if (file_offset >= file_length)
				IsComplete = true;
		}

		public void EndWrite (IOStream stream)
		{
			if (Callback != null)
				Callback ();

			if (file != null) {
				file.Close ();
				file = null;
			}
		}

		public bool Combine (IWriteOperation other)
		{
			return false;
		}
	}
#endif
}

