using System;
using System.Collections.Generic;
using Manos.Collections;
using System.Linq;

namespace Manos.IO
{
	public abstract class Stream : IDisposable
	{
		Action<ByteBuffer> onData;
		Action<Exception> onError;
		Action onClose;
		IDisposable currentReader;
		// write queue handling
		ByteBuffer currentBuffer;
		IEnumerator<ByteBuffer> currentWriter;
		Queue<IEnumerable<ByteBuffer>> writeQueue;
		
		protected Stream ()
		{
			this.writeQueue = new Queue<IEnumerable<ByteBuffer>> ();
		}

		protected virtual void CancelReader ()
		{
			onData = null;
			onError = null;
			onClose = null;
			if (currentWriter != null) {
				currentWriter.Dispose ();
				currentWriter = null;
			}
			currentBuffer = null;
			writeQueue.Clear ();
			writeQueue = null;
		}
		
		protected class ReaderHandle : IDisposable
		{
			Stream parent;

			public ReaderHandle (Stream parent)
			{
				this.parent = parent;
			}

			public void Dispose ()
			{
				if (parent.currentReader == this)
					parent.CancelReader ();
			}
		}
		
		public virtual IDisposable Read (Action<ByteBuffer> onData, Action<Exception> onError, Action onClose)
		{
			if (onData == null)
				throw new ArgumentNullException ("onData");
			if (onError == null)
				throw new ArgumentNullException ("onError");
			if (onClose == null)
				throw new ArgumentNullException ("onClose");
			
			this.onData = onData;
			this.onError = onError;
			this.onClose = onClose;
			
			currentReader = new ReaderHandle (this);
			
			return currentReader;
		}

		public virtual void Write (IEnumerable<ByteBuffer> data)
		{
			if (data == null)
				throw new ArgumentNullException ("data");
			
			writeQueue.Enqueue (data);
		}

		public virtual void Write (ByteBuffer data)
		{
			Write (Enumerable.Repeat (data, 1));
		}

		public virtual void Write (byte[] data)
		{
			Write (new ByteBuffer (data, 0, data.Length));
		}

		~Stream ()
		{
			Dispose (false);
		}

		public abstract void ResumeReading ();

		public abstract void ResumeReading (long forBytes);

		public abstract void ResumeWriting ();

		public abstract void PauseReading ();

		public abstract void PauseWriting ();

		public abstract void Flush ();

		public virtual void Close ()
		{
			onData = null;
			onError = null;
			onClose = null;
		}

		public void Dispose ()
		{
			Dispose (true);
			GC.SuppressFinalize (this);
		}

		protected virtual void Dispose (bool disposing)
		{
			Close ();
		}

		protected virtual void RaiseData (ByteBuffer data)
		{
			onData (data);
		}

		protected virtual void RaiseError (Exception exception)
		{
			onError (exception);
		}

		protected virtual void RaiseClose ()
		{
			onClose ();
		}

		protected abstract int WriteSingleBuffer (ByteBuffer buffer);

		protected virtual void HandleWrite ()
		{
			if (!EnsureActiveBuffer ()) {
				PauseWriting ();
			} else {
				WriteCurrentBuffer ();
			}
		}

		protected virtual void WriteCurrentBuffer ()
		{
			var sent = WriteSingleBuffer (currentBuffer);
			if (sent > 0) {
				currentBuffer.Position += sent;
				currentBuffer.Length -= sent;
			} else {
				PauseWriting ();
			}
			if (currentBuffer.Length == 0) {
				currentBuffer = null;
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

