using System;
using Manos.IO;
using System.Collections.Generic;
using System.IO;
using Mono.Unix.Native;

namespace Manos.Managed
{
	public class FileStream : Manos.IO.Stream
	{
		System.IO.FileStream stream;
		byte [] readBuffer;
		bool readEnabled, writeEnabled;
		long readLimit;
		IOLoop loop;

		FileStream (IOLoop loop, System.IO.FileStream stream, int blockSize)
		{
			if (loop == null)
				throw new ArgumentNullException ("loop");
			if (stream == null)
				throw new ArgumentNullException ("stream");
			
			this.loop = loop;
			this.stream = stream;
			this.readBuffer = new byte [blockSize];
		}

		public override long Position {
			get { return stream.Position; }
			set { SeekTo (value); }
		}

		public override bool CanRead {
			get { return stream.CanRead; }
		}

		public override bool CanSeek {
			get { return stream.CanSeek; }
		}

		public override bool CanWrite {
			get { return stream.CanWrite; }
		}

		public override void SeekBy (long delta)
		{
			stream.Seek (delta, SeekOrigin.Current);
		}

		public override void SeekTo (long position)
		{
			stream.Seek (position, SeekOrigin.Begin);
		}

		public override void Close ()
		{
			if (stream != null) {
				stream.Dispose ();
				stream = null;
				readBuffer = null;
			}
			base.Close ();
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
			ResumeReading ();
			return base.Read (onData, onError, onClose);
		}

		public override void ResumeReading ()
		{
			ResumeReading (long.MaxValue);
		}

		public override void ResumeReading (long forBytes)
		{
			if (forBytes < 0) {
				throw new ArgumentException ("forBytes");
			}
			readEnabled = true;
			readLimit = forBytes;
			ReadNextBuffer ();
		}

		public override void ResumeWriting ()
		{
			writeEnabled = true;
			HandleWrite ();
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
			stream.BeginRead (readBuffer, 0, length, OnReadDone, null);
		}

		void OnReadDone (IAsyncResult ar)
		{
			int result = stream.EndRead (ar);
			
			if (result > 0) {
				loop.NonBlockInvoke (delegate {
					RaiseData (new ByteBuffer (readBuffer, 0, result));
					ReadNextBuffer ();
				});
			} else {
				loop.NonBlockInvoke (delegate {
					PauseReading ();
					RaiseEndOfStream ();
				});
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
			stream.BeginWrite (buffer.Bytes, buffer.Position, buffer.Length, OnWriteDone, null);
			return buffer.Length;
		}

		void OnWriteDone (IAsyncResult ar)
		{
			stream.EndWrite (ar);
			loop.NonBlockInvoke (HandleWrite);
		}

		public static long GetLength (string fileName)
		{
			return new FileInfo (fileName).Length;
		}

		public static FileStream OpenRead (string fileName, int blockSize)
		{
			return Open (fileName, blockSize, OpenFlags.O_RDONLY, FilePermissions.ACCESSPERMS);
		}

		static FileStream Open (string fileName, int blockSize, OpenFlags openFlags, FilePermissions perms)
		{
			FileAccess access = FileAccess.ReadWrite;
			OpenFlags mask = OpenFlags.O_RDONLY | OpenFlags.O_RDWR | OpenFlags.O_WRONLY;
			if ((openFlags & mask) == OpenFlags.O_RDWR) {
				access = FileAccess.ReadWrite;
			} else if ((openFlags & mask) == OpenFlags.O_RDONLY) {
				access = FileAccess.Read;
			} else if ((openFlags & mask) == OpenFlags.O_WRONLY) {
				access = FileAccess.Write;
			} 
			var fs = new System.IO.FileStream (fileName, FileMode.Open, access);
			return new FileStream ((IOLoop) IOLoop.Instance, fs, blockSize);
		}
	}
}

