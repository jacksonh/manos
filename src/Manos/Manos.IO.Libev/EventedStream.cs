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
		ByteBuffer currentBuffer;
		IEnumerator<ByteBuffer> currentWriter;
		Queue<IEnumerable<ByteBuffer>> writeQueue;

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

		public void EnableReading ()
		{
			readWatcher.Start ();
		}

		public void EnableWriting ()
		{
			writeWatcher.Start ();
		}

		public void DisableReading ()
		{
			readWatcher.Stop ();
		}

		public void DisableWriting ()
		{
			writeWatcher.Stop ();
		}

		protected override void CancelReader ()
		{
			DisableReading ();
			base.CancelReader ();
		}

		public override IDisposable Read (Action<ByteBuffer> onData, Action<Exception> onError, Action onClose)
		{
			EnableReading ();
			
			return base.Read (onData, onError, onClose);
		}

		public override void Write (IEnumerable<ByteBuffer> data)
		{
			writeQueue.Enqueue (data);
		}

		public override void Close ()
		{
			DisableReading ();
			DisableWriting ();

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
			var sendBuffer = GetActiveBuffer ();
			if (sendBuffer == null) {
				DisableWriting ();
			} else {
				var sent = WriteSingleBuffer (sendBuffer);
				if (sent > 0) {
					sendBuffer.Position += sent;
					sendBuffer.Length -= sent;
					if (sendBuffer.Length == 0) {
						sendBuffer = null;
					}
				} else {
					DisableWriting ();
				}
			}
		}

		ByteBuffer GetActiveBuffer ()
		{
			if (currentBuffer == null) {
				var writer = GetActiveWriter ();
				if (writer.MoveNext ()) {
					currentBuffer = writer.Current;
				} else {
					writer.Dispose ();
					writer = null;
				}
			}
			return currentBuffer;
		}

		IEnumerator<ByteBuffer> GetActiveWriter ()
		{
			if (currentWriter == null) {
				if (writeQueue.Count > 0) {
					currentWriter = writeQueue.Dequeue ().GetEnumerator ();
				}
			}
			return currentWriter;
		}
	}
}

