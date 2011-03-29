using System;
using Mono.Unix.Native;
using System.Text;
using Manos.Collections;
using System.Collections.Generic;

namespace Manos.IO
{
	public class CopyingSendFileOperation : IWriteOperation
	{
		string fileName;
		WriteCallback callback;
		int fd;
		byte [] transferBuffer = new byte[8192];
		SocketStream stream;
		long position = 0;
		IWriteOperation currentBlock;

		public CopyingSendFileOperation (string filename, WriteCallback callback)
		{
			this.fileName = filename;
			this.callback = callback;

			Length = -1;
			fd = -1;
		}

		~CopyingSendFileOperation ()
		{
			Dispose ();
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
					currentBlock = new SendBytesOperation (new List<ByteBuffer> {
						new ByteBuffer (headerBytes, 0, headerBytes.Length)
					}, null);
					stream.EnableWriting ();
					currentBlock.BeginWrite (stream);
				}
			});
		}

		public void HandleWrite (IOStream stream)
		{
			this.stream = (SocketStream) stream;
			if (currentBlock != null && !currentBlock.IsComplete) {
				currentBlock.HandleWrite (stream);
				if (currentBlock.IsComplete) {
					currentBlock.EndWrite (stream);
					currentBlock.Dispose ();
					currentBlock = null;
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
				stream.DisableWriting ();
				Libeio.Libeio.read (fd, transferBuffer, position, 8192, (len, buf, err) => {
					if (position == Length) {
						OnComplete (Length, err);
					}
					if (len > 0) {
						position += len;
						currentBlock = new SendBytesOperation (new List<ByteBuffer> {
							new ByteBuffer (transferBuffer, 0, len)
						}, null);
					} else {
						OnComplete (Length, err);
					}
					stream.EnableWriting ();
				});
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

		private void OnComplete (long length, int error)
		{
			if (length == -1) {
				// hmmm, how best to propogate this?
				Console.Error.WriteLine ("Error sending file '{0}' errno: '{1}'", fileName, error);
				// Let it at least complete.
			} else
				Length = length;
			
			IsComplete = true;

			if (Completed != null)
				Completed (this, EventArgs.Empty);
		}

		public event EventHandler Completed;
	}
}

