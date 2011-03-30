using System;
using Mono.Unix.Native;
using System.Text;
using Manos.Collections;
using System.Collections.Generic;

namespace Manos.IO
{
	public abstract class SendFileOperation : IWriteOperation
	{
		string fileName;
		WriteCallback callback;
		protected int fd;
		protected SocketStream stream;
		protected long position = 0;
		protected IWriteOperation currentPrefixBlock;

		public SendFileOperation (string filename, WriteCallback callback)
		{
			this.fileName = filename;
			this.callback = callback;

			Length = -1;
			fd = -1;
		}

		public bool Chunked {
			get;
			set;
		}

		public long Length {
			get;
			set;
		}

		public void Dispose ()
		{
		}

		public void BeginWrite (IOStream stream)
		{
		}

		public bool Combine (IWriteOperation other)
		{
			return false;
		}

		public void EndWrite (IOStream stream)
		{
			Libeio.Libeio.close (fd, err => { });
			if (callback != null) {
				callback ();
			}
		}

		void OpenFile ()
		{
			stream.DisableWriting ();
			Libeio.Libeio.open (fileName, OpenFlags.O_RDONLY, FilePermissions.ALLPERMS, (fd, err) => {
				if (fd == -1) {
					OnComplete (-1, err);
				} else {
					this.fd = fd;
					stream.EnableWriting ();
					HandleWrite (stream);
				}
			});
		}

		void InitializeTransfer ()
		{
			stream.DisableWriting ();
			Libeio.Libeio.fstat (fd, (r, stat, err) => {
				if (r == -1) {
					OnComplete (-1, err);
				} else {
					Length = stat.st_size;
					var chunkHeader = string.Format ("{0:x}\r\n", Length);
					var headerBytes = Encoding.ASCII.GetBytes (chunkHeader);
					currentPrefixBlock = new SendBytesOperation (new List<ByteBuffer> {
						new ByteBuffer (headerBytes, 0, headerBytes.Length)
					}, null);
					stream.EnableWriting ();
					currentPrefixBlock.BeginWrite (stream);
				}
			});
		}

		protected abstract void SendNextBlock ();

		public void HandleWrite (IOStream stream)
		{
			this.stream = (SocketStream) stream;
			if (currentPrefixBlock != null && !currentPrefixBlock.IsComplete) {
				currentPrefixBlock.HandleWrite (stream);
				if (currentPrefixBlock.IsComplete) {
					currentPrefixBlock.EndWrite (stream);
					currentPrefixBlock.Dispose ();
					currentPrefixBlock = null;
				}
			}
			if (fd == -1) {
				OpenFile ();
			} else if (Length == -1) {
				if (!Chunked) {
					stream.DisableWriting ();
				} else {
					InitializeTransfer ();
				}
			} else if (position != Length) {
				SendNextBlock ();
			} else {
				OnComplete (0, 0);
			}
		}

		public bool IsComplete {
			get;
			private set;
		}

		public void SetLength (long l)
		{
			Length = l;
			if (stream != null)
				stream.EnableWriting ();
		}

		protected void OnComplete (long length, int error)
		{
			if (length == -1) {
				// hmmm, how best to propogate this?
				Console.Error.WriteLine ("Error sending file '{0}' errno: '{1}'", fileName, error);
				// Let it at least complete.
			}
			
			IsComplete = true;

			if (Completed != null)
				Completed (this, EventArgs.Empty);
		}

		public event EventHandler Completed;
	}
}

