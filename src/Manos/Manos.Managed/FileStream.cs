using System;
using Manos.Collections;
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
		long position;
		IOLoop loop;
		// write queue
		IEnumerator<ByteBuffer> currentWrite;
		Queue<IEnumerable<ByteBuffer>> writeQueue;

		FileStream (IOLoop loop, System.IO.FileStream stream, int blockSize)
		{
			if (loop == null)
				throw new ArgumentNullException ("loop");
			if (stream == null)
				throw new ArgumentNullException ("stream");
			
			this.loop = loop;
			this.stream = stream;
			this.readBuffer = new byte [blockSize];
			
			this.writeQueue = new Queue<IEnumerable<ByteBuffer>> ();
		}

		public override void Close ()
		{
			if (stream != null) {
				stream.Dispose ();
				stream = null;
				if (currentWrite != null) {
					currentWrite.Dispose ();
					currentWrite = null;
				}
				writeQueue.Clear ();
				writeQueue = null;
				readBuffer = null;
			}
			base.Close ();
		}

		public override void Flush ()
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
					position += result;
					ReadNextBuffer ();
				});
			} else {
				loop.NonBlockInvoke (delegate {
					readEnabled = false;
					RaiseClose ();
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

		void WriteNextBuffer (ByteBuffer buffer)
		{
			stream.BeginWrite (buffer.Bytes, buffer.Position, buffer.Length, OnWriteDone, null);
		}

		void HandleWrite ()
		{
			if (!writeEnabled) {
				return;
			}
				
			if (currentWrite == null) {
				if (writeQueue.Count > 0) {
					currentWrite = writeQueue.Dequeue ().GetEnumerator ();
					HandleWrite ();
				} else {
					PauseWriting ();
				}
			} else {
				if (currentWrite.MoveNext ()) {
					WriteNextBuffer (currentWrite.Current);
					PauseWriting ();
				} else {
					currentWrite.Dispose ();
					currentWrite = null;
					HandleWrite ();
				}
			}
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
			if ((openFlags & OpenFlags.O_RDWR) != 0) {
				access = FileAccess.ReadWrite;
			} else if ((openFlags & OpenFlags.O_RDONLY) != 0) {
				access = FileAccess.Read;
			} else if ((openFlags & OpenFlags.O_WRONLY) != 0) {
				access = FileAccess.Write;
			} 
			var fs = new System.IO.FileStream (fileName, FileMode.Open, access);
			return new FileStream ((IOLoop) IOLoop.Instance, fs, blockSize);
		}
	}
}

