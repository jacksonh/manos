using System;
using Manos.Collections;
using Libev;
using System.Collections.Generic;

namespace Manos.IO.Libev
{
	public abstract class EventedStream : Stream
	{
		// readiness watchers
		IOWatcher readWatcher, writeWatcher;
		// write queue handling
		protected ByteBuffer currentBuffer;
		protected IEnumerator<ByteBuffer> currentWriter;
		protected Queue<IEnumerable<ByteBuffer>> writeQueue;

		protected EventedStream (IOLoop loop, IntPtr handle)
		{
			if (loop == null)
				throw new ArgumentNullException ("loop");
			if (handle == IntPtr.Zero)
				throw new ArgumentException ("handle");
			
			this.Loop = loop;
			this.Handle = handle;
			
			this.readWatcher = new IOWatcher (Handle, EventTypes.Read, Loop.EVLoop, HandleReadReady);
			this.writeWatcher = new IOWatcher (Handle, EventTypes.Write, Loop.EVLoop, HandleWriteReady);
			
			this.writeQueue = new Queue<IEnumerable<ByteBuffer>> ();
		}

		void HandleWriteReady (Loop loop, IOWatcher watcher, EventTypes revents)
		{
			HandleWrite ();
		}

		void HandleReadReady (Loop loop, IOWatcher watcher, EventTypes revents)
		{
			HandleRead ();
		}

		public IOLoop Loop {
			get;
			private set;
		}

		public IntPtr Handle {
			get;
			private set;
		}

		public override void ResumeReading ()
		{
			readWatcher.Start ();
		}

		public override void ResumeWriting ()
		{
			writeWatcher.Start ();
		}

		public override void PauseReading ()
		{
			readWatcher.Stop ();
		}

		public override void PauseWriting ()
		{
			writeWatcher.Stop ();
		}

		protected override void CancelReader ()
		{
			PauseReading ();
			base.CancelReader ();
		}

		public override IDisposable Read (Action<ByteBuffer> onData, Action<Exception> onError, Action onClose)
		{
			ResumeReading ();
			
			return base.Read (onData, onError, onClose);
		}

		public override void Write (IEnumerable<ByteBuffer> data)
		{
			writeQueue.Enqueue (data);
			
			ResumeWriting ();
		}

		public override void Close ()
		{
			PauseReading ();
			PauseWriting ();

			readWatcher.Dispose ();
			writeWatcher.Dispose ();
			if (currentWriter != null) {
				currentWriter.Dispose ();
			}

			readWatcher = null;
			writeWatcher = null;
			
			currentWriter = null;
			currentBuffer = null;
			
			Handle = IntPtr.Zero;
		
			base.Close ();
		}

		protected abstract void HandleRead ();

		protected abstract int WriteSingleBuffer (ByteBuffer buffer);

		protected virtual void HandleWrite ()
		{
			if (!EnsureActiveBuffer ()) {
				PauseWriting ();
			} else {
				SendCurrentBuffer ();
			}
		}

		protected virtual void SendCurrentBuffer ()
		{
			var sent = WriteSingleBuffer (currentBuffer);
			if (sent > 0) {
				currentBuffer.Position += sent;
				currentBuffer.Length -= sent;
				if (currentBuffer.Length == 0) {
					currentBuffer = null;
				}
			} else {
				PauseWriting ();
			}
		}

		protected virtual bool EnsureActiveBuffer ()
		{
			if (currentBuffer == null && EnsureActiveWriter ()) {
				if (currentWriter.MoveNext ()) {
					currentBuffer = currentWriter.Current;
				} else {
					currentWriter.Dispose ();
					currentWriter = null;
					return EnsureActiveBuffer ();
				}
			}
			return currentBuffer != null;
		}

		protected virtual bool EnsureActiveWriter ()
		{
			if (currentWriter == null && writeQueue.Count > 0) {
				currentWriter = writeQueue.Dequeue ().GetEnumerator ();
			}
			return currentWriter != null;
		}
	}
}

