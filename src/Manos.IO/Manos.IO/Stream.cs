using System;
using System.Collections.Generic;
using System.Linq;

namespace Manos.IO
{
	public abstract class Stream : IDisposable
	{
		Action<ByteBuffer> onData;
		Action<Exception> onError;
		Action onEndOfStream;
		IDisposable currentReader;
		// write queue handling
		ByteBuffer currentBuffer;
		IEnumerator<ByteBuffer> currentWriter;
		Queue<IEnumerable<ByteBuffer>> writeQueue;

		protected Stream ()
		{
		}

		protected virtual void CancelReader ()
		{
			onData = null;
			onError = null;
			onEndOfStream = null;
			currentReader = null;
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
		
		public virtual IDisposable Read (Action<ByteBuffer> onData, Action<Exception> onError, Action onEndOfStream)
		{
			if (onData == null)
				throw new ArgumentNullException ("onData");
			if (onError == null)
				throw new ArgumentNullException ("onError");
			if (onEndOfStream == null)
				throw new ArgumentNullException ("onClose");
			
			this.onData = onData;
			this.onError = onError;
			this.onEndOfStream = onEndOfStream;
			
			currentReader = new ReaderHandle (this);
			
			return currentReader;
		}

		public virtual void Write (IEnumerable<ByteBuffer> data)
		{
			if (data == null)
				throw new ArgumentNullException ("data");
			
			if (writeQueue == null) {
				writeQueue = new Queue<IEnumerable<ByteBuffer>> ();
			}
			
			writeQueue.Enqueue (data);
		}

		static IEnumerable<ByteBuffer> SingleBuffer (ByteBuffer buffer)
		{
			yield return buffer;
		}

		public virtual void Write (ByteBuffer data)
		{
			Write (SingleBuffer (data));
		}

		public virtual void Write (byte[] data)
		{
			Write (new ByteBuffer (data, 0, data.Length));
		}

		~Stream ()
		{
			Dispose (false);
		}

		public abstract long Position {
			get;
			set;
		}

		public abstract bool CanRead {
			get;
		}

		public abstract bool CanWrite {
			get;
		}

		public virtual bool CanSeek {
			get { return false; }
		}

		public virtual bool CanTimeout {
			get { return false; }
		}

		public virtual TimeSpan ReadTimeout {
			get { throw new NotSupportedException (); }
			set { throw new NotSupportedException (); }
		}

		public virtual TimeSpan WriteTimeout {
			get { throw new NotSupportedException (); }
			set { throw new NotSupportedException (); }
		}

		public abstract void ResumeReading ();

		public abstract void ResumeReading (long forBytes);

		public abstract void ResumeWriting ();

		public abstract void PauseReading ();

		public abstract void PauseWriting ();

		public virtual void SeekBy (long delta)
		{
			throw new NotSupportedException ();
		}

		public virtual void SeekTo (long position)
		{
			throw new NotSupportedException ();
		}

		public abstract void Flush ();

		public virtual void Close ()
		{
			if (currentReader != null) {
				currentReader.Dispose ();
				currentReader = null;
			}
			if (writeQueue != null) {
				if (currentWriter != null) {
					currentWriter.Dispose ();
					currentWriter = null;
				}
				currentBuffer = null;
				writeQueue.Clear ();
				writeQueue = null;
			}
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
			if (onData != null) {
				onData (data);
			}
		}

		protected virtual void RaiseError (Exception exception)
		{
			if (onError != null) {
				onError (exception);
			}
		}

		protected virtual void RaiseEndOfStream ()
		{
			if (onEndOfStream != null) {
				onEndOfStream ();
			}
		}

		protected abstract int WriteSingleBuffer (ByteBuffer buffer);

		protected virtual void HandleWrite ()
		{
			if (writeQueue == null) {
				throw new InvalidOperationException ();
			}
			if (!EnsureActiveBuffer () || currentBuffer == null) {
				PauseWriting ();
			} else {
				WriteCurrentBuffer ();
			}
		}

		protected virtual void WriteCurrentBuffer ()
		{
			var sent = WriteSingleBuffer (currentBuffer);
			if (sent >= 0) {
				currentBuffer.Skip (sent);
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

