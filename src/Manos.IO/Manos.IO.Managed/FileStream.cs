using System;
using Manos.IO;
using System.Collections.Generic;
using System.IO;
using Mono.Unix.Native;

namespace Manos.IO.Managed
{
	class FileStream : ManagedStream
	{
		System.IO.FileStream stream;
		byte [] readBuffer;
		bool readEnabled, writeEnabled;
		long readLimit;

		public FileStream (Context loop, System.IO.FileStream stream, int blockSize)
			: base (loop)
		{
			if (loop == null)
				throw new ArgumentNullException ("loop");
			if (stream == null)
				throw new ArgumentNullException ("stream");
			
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
			stream.BeginRead (readBuffer, 0, length, OnReadDone, null);
		}

		void OnReadDone (IAsyncResult ar)
		{
			Enqueue (delegate {
				if (stream != null) {
					int result = stream.EndRead (ar);
			
					if (result > 0) {
						RaiseData (new ByteBuffer (readBuffer, 0, result));
						ReadNextBuffer ();
					} else {
						PauseReading ();
						RaiseEndOfStream ();
					}
				}
			});
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
			Enqueue (delegate {
				if (stream != null) {
					stream.EndWrite (ar);
					HandleWrite ();
				}
			});
		}
	}
}

