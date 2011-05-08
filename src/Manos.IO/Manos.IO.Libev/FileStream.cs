using System;
using System.Collections.Generic;
using Mono.Unix.Native;

namespace Manos.IO.Libev
{
	class FileStream : Stream
	{
		byte [] readBuffer;
		bool readEnabled, writeEnabled;
		bool canRead, canWrite;
		long readLimit;
		long position;

		FileStream (IntPtr handle, int blockSize, bool canRead, bool canWrite)
		{
			this.Handle = handle;
			this.readBuffer = new byte [blockSize];
			this.canRead = canRead;
			this.canWrite = canWrite;
		}

		public IntPtr Handle {
			get;
			private set;
		}

		public override long Position {
			get { return position; }
			set { SeekTo (value); }
		}

		public override bool CanRead {
			get { return canRead; }
		}

		public override bool CanWrite {
			get { return canWrite; }
		}

		public override bool CanSeek {
			get { return true; }
		}

		public override void Close ()
		{
			if (Handle != IntPtr.Zero) {
				Syscall.close (Handle.ToInt32 ());
				Handle = IntPtr.Zero;
			}
			base.Close ();
		}

		public override void SeekBy (long delta)
		{
			if (position + delta < 0)
				throw new ArgumentException ("delta");
			
			this.position += delta;
		}

		public override void SeekTo (long position)
		{
			if (position < 0)
				throw new ArgumentException ("position");
			
			this.position = position;
		}

		public override void Flush ()
		{
		}

		public override void Write (IEnumerable<ByteBuffer> data)
		{
			base.Write (data);
			ResumeWriting ();
		}

		public override IDisposable Read (Action<ByteBuffer> onData, Action<Exception> onError, Action onClose)
		{
			var result = base.Read (onData, onError, onClose);
			ResumeReading ();
			return result;
		}

		public override void ResumeReading ()
		{
			ResumeReading (long.MaxValue);
		}

		public override void ResumeReading (long forBytes)
		{
			if (!canRead)
				throw new InvalidOperationException ();
			if (forBytes < 0) {
				throw new ArgumentException ("forBytes");
			}
			
			readLimit = forBytes;
			if (!readEnabled) {
				readEnabled = true;
				ReadNextBuffer ();
			}
		}

		public override void ResumeWriting ()
		{
			if (!canWrite)
				throw new InvalidOperationException ();
			
			if (!writeEnabled) {
				writeEnabled = true;
				HandleWrite ();
			}
		}

		public override void PauseReading ()
		{
			readEnabled = false;
		}

		public override void PauseWriting ()
		{
			writeEnabled = false;
		}

		void ReadNextBuffer ()
		{
			if (!readEnabled) {
				return;
			}
			
			var length = (int) Math.Min (readBuffer.Length, readLimit);
			Libeio.read (Handle.ToInt32 (), readBuffer, position, length, OnReadDone);
		}

		void OnReadDone (int result, byte[] buffer, int error)
		{
			if (result < 0) {
				PauseReading ();
				RaiseError (new Exception (string.Format ("Error '{0}' reading from file '{1}'", error, Handle.ToInt32 ())));
			} else if (result > 0) {
				position += result;
				RaiseData (new ByteBuffer (buffer, 0, result));
				ReadNextBuffer ();
			} else {
				PauseReading ();
				RaiseEndOfStream ();
			}
		}

		protected override void RaiseData (ByteBuffer data)
		{
			readLimit -= data.Length;
			if (readLimit <= 0) {
				PauseReading ();
			}
			base.RaiseData (data);
		}

		protected override void HandleWrite ()
		{
			if (writeEnabled) {
				base.HandleWrite ();
			}
		}

		protected override int WriteSingleBuffer (ByteBuffer buffer)
		{
			var bytes = buffer.Bytes;
			if (buffer.Position > 0) {
				bytes = new byte[buffer.Length];
				Array.Copy (buffer.Bytes, buffer.Position, bytes, 0, buffer.Length);
			}
			Libeio.write (Handle.ToInt32 (), bytes, position, buffer.Length, OnWriteDone);
			return buffer.Length;
		}

		void OnWriteDone (int result, int error)
		{
			if (result < 0) {
				throw new Exception (string.Format ("Error '{0}' writing to file '{1}'", error, Handle.ToInt32 ()));
			}
			HandleWrite ();
		}

		public static long GetLength (string fileName)
		{
			Stat stat;
			Mono.Unix.Native.Syscall.stat (fileName, out stat);
			return stat.st_size;
		}

		public static FileStream OpenRead (string fileName, int blockSize)
		{
			return Open (fileName, blockSize, OpenFlags.O_RDONLY, FilePermissions.ACCESSPERMS);
		}

		public static FileStream Open (string fileName, int blockSize, OpenFlags openFlags, FilePermissions perms)
		{
			var fd = Mono.Unix.Native.Syscall.open (fileName, openFlags, perms);
			var mask = OpenFlags.O_RDONLY | OpenFlags.O_RDWR | OpenFlags.O_WRONLY;
			var canRead = (openFlags & mask) == OpenFlags.O_RDONLY
				|| (openFlags & mask) == OpenFlags.O_RDWR;
			var canWrite = (openFlags & mask) == OpenFlags.O_WRONLY
				|| (openFlags & mask) == OpenFlags.O_RDWR;
			return new FileStream (new IntPtr (fd), blockSize, canRead, canWrite);
		}
	}
}

