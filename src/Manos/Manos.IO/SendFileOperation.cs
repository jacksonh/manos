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
		private string filename;

		public SendFileOperation (string filename, WriteCallback callback)
		{
			this.filename = filename;
			this.callback = callback;

			Length = -1;
		}

		~SendFileOperation ()
		{
			Dispose ();
		}

		public void Dispose ()
		{
		}

		public bool Chunked {
			get;
			set;
		}

		public long Length {
			get;
			set;
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

		private SocketStream waiting_stream = null;

		public void HandleWrite (IOStream stream)
		{
			SocketStream sstream = (SocketStream) stream;

			if (!Chunked && Length == -1) {
				waiting_stream = sstream;
				sstream.DisableWriting ();
				return;
			}
			
			sstream.SendFile (filename, Chunked, Length, (length, error) => {
				IsComplete = true;
				sstream.EnableWriting ();
				OnComplete (length, error);
			});

			sstream.DisableWriting ();
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

		private void OnComplete (long length, int error)
		{
			if (length == -1) {
				// hmmm, how best to propogate this?
				Console.Error.WriteLine ("Error sending file '{0}' errno: '{1}'", filename, error);
				// Let it at least complete.
			} else
				Length = length;

			if (Completed != null)
				Completed (this, EventArgs.Empty);
		}

		public void SetLength (long l)
		{
			Length = l;
			if (waiting_stream != null)
				waiting_stream.EnableWriting ();
			waiting_stream = null;
		}

		public event EventHandler Completed;
	}
#endif
}

