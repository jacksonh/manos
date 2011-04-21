using System;
using Manos.Collections;
using System.Collections.Generic;
using Mono.Unix.Native;

namespace Manos.IO.Libev
{
	public class FileStream : Stream
	{
		byte [] readBuffer;
		bool readEnabled, writeEnabled;
		long readLimit;
		long position;

		FileStream (IntPtr handle, int blockSize)
		{
			this.Handle = handle;
			this.readBuffer = new byte [blockSize];
		}

		public IntPtr Handle {
			get;
			private set;
		}

		public override void Close ()
		{
			if (Handle != IntPtr.Zero) {
				Libeio.close (Handle.ToInt32 (), OnCloseDone);
				Handle = IntPtr.Zero;
			}
			base.Close ();
		}

		public override void Flush ()
		{
		}

		void OnCloseDone (int result)
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
				RaiseClose ();
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

		static FileStream Open (string fileName, int blockSize, OpenFlags openFlags, FilePermissions perms)
		{
			var fd = Mono.Unix.Native.Syscall.open (fileName, openFlags, perms);
			return new FileStream (new IntPtr (fd), blockSize);
		}
	}
}

