using System;
using Manos.IO;
using System.Collections.Generic;
using System.IO;
using Mono.Unix.Native;

namespace Manos.IO.Managed
{
	class FileStream : ManagedByteStream
	{
		System.IO.FileStream stream;

		public FileStream (Context loop, System.IO.FileStream stream, int blockSize)
			: base (loop, blockSize)
		{
			if (loop == null)
				throw new ArgumentNullException ("loop");
			if (stream == null)
				throw new ArgumentNullException ("stream");
			
			this.stream = stream;
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
			}
			base.Close ();
		}

		public override void Write (IEnumerable<ByteBuffer> data)
		{
			base.Write (data);
			ResumeWriting ();
		}
		
		protected override void DoRead ()
		{
			var length = (int) Math.Min (buffer.Length, readLimit ?? long.MaxValue);
			stream.BeginRead (buffer, 0, length, OnReadDone, null);
		}

		void OnReadDone (IAsyncResult ar)
		{
			Context.Enqueue (delegate {
				if (stream != null) {
					ResetReadTimeout ();
					int result = stream.EndRead (ar);
			
					if (result > 0) {
						byte [] newBuffer = new byte [result];
						Buffer.BlockCopy (buffer, 0, newBuffer, 0, result);
						
						RaiseData (new ByteBuffer (newBuffer));
						DispatchRead ();
					} else {
						PauseReading ();
						RaiseEndOfStream ();
					}
				}
			});
		}
		
		protected override WriteResult WriteSingleFragment (ByteBuffer fragment)
		{
			stream.BeginWrite (fragment.Bytes, fragment.Position, fragment.Length, OnWriteDone, null);
			return WriteResult.Consume;
		}

		void OnWriteDone (IAsyncResult ar)
		{
			Context.Enqueue (delegate {
				if (stream != null) {
					ResetWriteTimeout ();
					stream.EndWrite (ar);
					HandleWrite ();
				}
			});
		}
	}
}

