using System;
using Manos.Collections;
using System.Collections.Generic;
using Mono.Unix.Native;

namespace Manos.IO.Libev
{
	public class FileStream : Stream
	{
		byte [] readBuffer = new byte[4096];
		bool readEnabled, writeEnabled;
		long readLimit;
		long position;
		// write queue
		IEnumerator<ByteBuffer> currentWriter;
		Queue<IEnumerable<ByteBuffer>> writeQueue;

		internal FileStream (IntPtr handle)
		{
			this.Handle = handle;
			
			this.writeQueue = new Queue<IEnumerable<ByteBuffer>> ();
		}

		public IntPtr Handle {
			get;
			private set;
		}

		public override void Close ()
		{
			Libeio.close (Handle.ToInt32 (), OnCloseDone);
			Handle = IntPtr.Zero;
			if (currentWriter != null) {
				currentWriter.Dispose ();
				currentWriter = null;
			}
			writeQueue.Clear ();
			writeQueue = null;
			readBuffer = null;
			base.Close ();
		}

		void OnCloseDone (int result)
		{
		}

		public override void Write (IEnumerable<ByteBuffer> data)
		{
			if (data == null) {
				throw new ArgumentNullException ("data");
			}
			
			writeQueue.Enqueue (data);
			
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
			WriteNextBuffer ();
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
				RaiseError (new Exception (string.Format ("Error '{0}' reading from file '{1}'", error, Handle.ToInt32 ())));
			} else if (result > 0) {
				RaiseData (new ByteBuffer (buffer, 0, result));
				position += result;
			} else {
				readEnabled = false;
				RaiseClose ();
			}
			ReadNextBuffer ();
		}

		protected override void RaiseData (ByteBuffer data)
		{
			readLimit -= data.Length;
			if (readLimit <= 0) {
				PauseReading ();
			}
			base.RaiseData (data);
		}

		void WriteNextBuffer ()
		{
			if (!writeEnabled) {
				return;
			}
			
			while (EnsureActiveWriter() && !currentWriter.MoveNext()) {
				currentWriter.Dispose ();
				currentWriter = null;
			}
			
			if (currentWriter == null) {
				PauseWriting ();
			} else {
				var currentBuffer = currentWriter.Current;
				var bytes = currentBuffer.Bytes;
				if (currentBuffer.Position > 0) {
					bytes = new byte[currentBuffer.Length];
					Array.Copy (currentBuffer.Bytes, currentBuffer.Position, bytes, 0, currentBuffer.Length);
				}
				Libeio.write (Handle.ToInt32 (), bytes, position, currentBuffer.Length, OnWriteDone);
			}
		}

		void OnWriteDone (int result, int error)
		{
			if (result < 0) {
				throw new Exception (string.Format ("Error '{0}' writing to file '{1}'", error, Handle.ToInt32 ()));
			}
			WriteNextBuffer ();
		}

		bool EnsureActiveWriter ()
		{
			if (currentWriter == null && writeQueue.Count > 0) {
				currentWriter = writeQueue.Dequeue ().GetEnumerator ();
			}
			return currentWriter != null;
		}

		public static void OpenRead (string fileName, Action<Stream> onOpen, Action<Exception> onError)
		{
			Open (fileName, OpenFlags.O_RDONLY, FilePermissions.ACCESSPERMS, onOpen, onError);
		}

		static void Open (string fileName, OpenFlags openFlags, FilePermissions perms,
			Action<Stream> onOpen, Action<Exception> onError)
		{
			if (onOpen == null) 
				throw new ArgumentNullException ("onOpen");
			if (onError == null)
				throw new ArgumentNullException ("onError");
			
			var fd = Mono.Unix.Native.Syscall.open (fileName, openFlags, perms);
			onOpen (new FileStream (new IntPtr (fd)));
			
//			Libeio.open (fileName, openFlags, perms, (fd, err) => {
//				if (fd == -1) {
//					onError (new Exception (string.Format ("Error opening file '{0}' errno: '{1}'", fileName, err)));
//				} else {
//					onOpen (new FileStream (new IntPtr (fd)));
//				}
//			});
		}
	}
}

