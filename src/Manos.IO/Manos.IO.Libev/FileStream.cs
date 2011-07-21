using System;
using System.Collections.Generic;
using Mono.Unix.Native;
using System.IO;

namespace Manos.IO.Libev
{
	class FileStream : FragmentStream<ByteBuffer>, IByteStream
	{
		byte [] readBuffer;
		bool readEnabled, writeEnabled;
		bool canRead, canWrite;
		long readLimit;
		long position;

		FileStream (Context context, IntPtr handle, int blockSize, bool canRead, bool canWrite)
			: base (context)
		{
			this.Handle = handle;
			this.readBuffer = new byte [blockSize];
			this.canRead = canRead;
			this.canWrite = canWrite;
		}
		
		public new Context Context {
			get { return (Context) base.Context; }
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
		
		protected override void Dispose (bool disposing)
		{
			if (Handle != IntPtr.Zero) {
				Syscall.close (Handle.ToInt32 ());
				Handle = IntPtr.Zero;
			}
			base.Dispose (disposing);
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
		
		public void Write (byte[] data)
		{
			CheckDisposed ();
			
			Write (new ByteBuffer (data));
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
			CheckDisposed ();
			
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
			CheckDisposed ();
			
			if (!canWrite)
				throw new InvalidOperationException ();
			
			if (!writeEnabled) {
				writeEnabled = true;
				HandleWrite ();
			}
		}

		public override void PauseReading ()
		{
			CheckDisposed ();
			
			readEnabled = false;
		}

		public override void PauseWriting ()
		{
			CheckDisposed ();
			
			writeEnabled = false;
		}

		void ReadNextBuffer ()
		{
			if (!readEnabled) {
				return;
			}
			
			var length = (int) Math.Min (readBuffer.Length, readLimit);
			Context.Eio.Read (Handle.ToInt32 (), readBuffer, position, length, OnReadDone);
		}

		void OnReadDone (int result, byte[] buffer, int error)
		{
			if (result < 0) {
				PauseReading ();
				RaiseError (new IOException (string.Format ("Error reading from file: {0}", Errors.ErrorToString (error))));
			} else if (result > 0) {
				position += result;
				byte [] newBuffer = new byte [result];
				Buffer.BlockCopy (readBuffer, 0, newBuffer, 0, result);
				RaiseData (new ByteBuffer (newBuffer));
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

		protected override WriteResult WriteSingleFragment (ByteBuffer buffer)
		{
			var bytes = buffer.Bytes;
			if (buffer.Position > 0) {
				bytes = new byte[buffer.Length];
				Array.Copy (buffer.Bytes, buffer.Position, bytes, 0, buffer.Length);
			}
			Context.Eio.Write (Handle.ToInt32 (), bytes, position, buffer.Length, OnWriteDone);
			return WriteResult.Consume;
		}

		void OnWriteDone (int result, int error)
		{
			if (result < 0) {
				RaiseError (new IOException (string.Format ("Error writing to file: {0}", Errors.ErrorToString (error))));
			}
			HandleWrite ();
		}
		
		protected override long FragmentSize (ByteBuffer fragment)
		{
			return fragment.Length;
		}

		public static FileStream Open (Context context, string fileName, int blockSize,
			OpenFlags openFlags)
		{
			var fd = Syscall.open (fileName, openFlags,
				FilePermissions.S_IRUSR | FilePermissions.S_IWUSR | FilePermissions.S_IROTH);
			var mask = OpenFlags.O_RDONLY | OpenFlags.O_RDWR | OpenFlags.O_WRONLY;
			var canRead = (openFlags & mask) == OpenFlags.O_RDONLY
				|| (openFlags & mask) == OpenFlags.O_RDWR;
			var canWrite = (openFlags & mask) == OpenFlags.O_WRONLY
				|| (openFlags & mask) == OpenFlags.O_RDWR;
			return new FileStream (context, new IntPtr (fd), blockSize, canRead, canWrite);
		}

		public static FileStream Create (Context context, string fileName, int blockSize)
		{
			return Open (context, fileName, blockSize,
				OpenFlags.O_RDWR | OpenFlags.O_CREAT | OpenFlags.O_TRUNC);
		}
	}
}

