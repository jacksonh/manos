using System;
using System.Collections.Generic;
using System.Linq;

namespace Manos.IO
{
	/// <summary>
	/// Base class for asynchronous streams. Other than synchronous streams,
	/// asynchronous streams do not block calls to their Read or Write methods.
	/// <para>Calls to Read replace the current set of reader callbacks (which need
	/// not yet exist) with a new set of callbacks. These callbacks are invoked
	/// whenever the corresponding event occured.</para>
	/// <para>Calls to Write place the data to be written into a write queue.
	/// Whenever the stream becomes writeable, data from the queue is written.</para>
	/// <para>The reading and writing parts of the stream may be paused and resumed
	/// individually. Pausing and resuming is a set-reset process, so multiple
	/// calls to Pause methods can be undone by a single call to a Resume method.</para>
	/// </summary>
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
		
		/// <summary>
		/// Initializes a new instance of the <see cref="Manos.IO.Stream"/> class.
		/// </summary>
		protected Stream ()
		{
		}

		/// <summary>
		/// Cancels the current reader and clears the set of reader callbacks.
		/// </summary>
		protected virtual void CancelReader ()
		{
			onData = null;
			onError = null;
			onEndOfStream = null;
			currentReader = null;
		}
		
		/// <summary>
		/// A <see cref="ReaderHandle"/> represents a stream users' handle on the
		/// set of reader callbacks. If the user wishes to not receive further
		/// callbacks, he must call <see cref="Dispose"/> on the handle to cancel
		/// his set of callbacks.
		/// </summary>
		protected class ReaderHandle : IDisposable
		{
			Stream parent;
			
			/// <summary>
			/// Initializes a new instance of the <see cref="Manos.IO.Stream.ReaderHandle"/> class.
			/// </summary>
			public ReaderHandle (Stream parent)
			{
				this.parent = parent;
			}
			
			/// <summary>
			/// Cancels the set of reader callbacks in the parent.
			/// </summary>
			public void Dispose ()
			{
				if (parent.currentReader == this)
					parent.CancelReader ();
			}
		}
		
		/// <summary>
		/// Replaces the current set of reader callbacks with the given set of reader callbacks.
		/// The returned value may be used to indicate that no further callback invocations should
		/// occur.
		/// </summary>
		/// <exception cref='ArgumentNullException'>
		/// Is thrown when any argument passed to the method is <see langword="null" /> .
		/// </exception>
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

		/// <summary>
		/// Places a sequence of buffers into the write queue.
		/// The sequence is not touched, only when the first piece of data in the
		/// sequence may be written to the stream, the enumeration is started.
		/// This allows for data generators that produce large amounts of data, but
		/// have a very small memory footprint.
		/// <para>The sequence may return an arbitrary number of byte buffers.</para>
		/// <para>Byte buffers of length 0 are interpreted literally, as writes
		/// of length 0. Writing continues after the buffer has been written, unless the
		/// stream has become congested.</para>
		/// <para><c>null</c> byte buffers pause writing.</para>
		/// </summary>
		/// <exception cref='ArgumentNullException'>
		/// Is thrown when an argument passed to a method is invalid because it is <see langword="null" /> .
		/// </exception>
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
		
		/// <summary>
		/// Places a single buffer into the write queue.
		/// </summary>
		public virtual void Write (ByteBuffer data)
		{
			Write (SingleBuffer (data));
		}
		
		/// <summary>
		/// Places a single byte array into the write queue.
		/// </summary>
		public virtual void Write (byte[] data)
		{
			Write (new ByteBuffer (data, 0, data.Length));
		}
		
		/// <summary>
		/// Releases unmanaged resources and performs other cleanup operations before the <see cref="Manos.IO.Stream"/> is
		/// reclaimed by garbage collection.
		/// </summary>
		~Stream ()
		{
			Dispose (false);
		}

		/// <summary>
		/// Gets or sets the position.
		/// </summary>
		public abstract long Position {
			get;
			set;
		}
		
		/// <summary>
		/// Gets a value indicating whether this instance can read.
		/// </summary>
		/// <value>
		/// <c>true</c> if this instance can read; otherwise, <c>false</c>.
		/// </value>
		public abstract bool CanRead {
			get;
		}
		
		/// <summary>
		/// Gets a value indicating whether this instance can write.
		/// </summary>
		/// <value>
		/// <c>true</c> if this instance can write; otherwise, <c>false</c>.
		/// </value>
		public abstract bool CanWrite {
			get;
		}
		
		/// <summary>
		/// Gets a value indicating whether this instance can seek.
		/// </summary>
		/// <value>
		/// <c>true</c> if this instance can seek; otherwise, <c>false</c>.
		/// </value>
		public virtual bool CanSeek {
			get { return false; }
		}
		
		/// <summary>
		/// Gets a value indicating whether this instance can timeout.
		/// </summary>
		/// <value>
		/// <c>true</c> if this instance can timeout; otherwise, <c>false</c>.
		/// </value>
		public virtual bool CanTimeout {
			get { return false; }
		}
		
		/// <summary>
		/// Gets or sets the read timeout.
		/// </summary>
		public virtual TimeSpan ReadTimeout {
			get { throw new NotSupportedException (); }
			set { throw new NotSupportedException (); }
		}

		/// <summary>
		/// Gets or sets the write timeout.
		/// </summary>
		public virtual TimeSpan WriteTimeout {
			get { throw new NotSupportedException (); }
			set { throw new NotSupportedException (); }
		}
		
		/// <summary>
		/// Instructs the stream to resume reading when it is not reading yet.
		/// </summary>
		public abstract void ResumeReading ();

		/// <summary>
		/// Instructs the stream to resume reading when it is not reading yet.
		/// After <paramref name="forBytes"/> bytes have been read, the stream
		/// automatically pauses itself again.
		/// </summary>
		public abstract void ResumeReading (long forBytes);
		
		/// <summary>
		/// Resumes writing.
		/// </summary>
		public abstract void ResumeWriting ();
		
		/// <summary>
		/// Pauses reading.
		/// </summary>
		public abstract void PauseReading ();
		
		/// <summary>
		/// Pauses writing.
		/// </summary>
		public abstract void PauseWriting ();
		
		/// <summary>
		/// Seeks by <paramref name="delta"/> bytes. A positive <paramref name="delta"/>
		/// will seek forward, a negative <paramref name="delta"/> will seek backwards.
		/// </summary>
		public virtual void SeekBy (long delta)
		{
			throw new NotSupportedException ();
		}
		
		/// <summary>
		/// Seeks to absolute position <paramref name="position"/> in the stream.
		/// </summary>
		public virtual void SeekTo (long position)
		{
			throw new NotSupportedException ();
		}
		
		/// <summary>
		/// Flush all buffers held by this instance, if applicable. This need not
		/// flush the write queue, it must however place equivalents for all
		/// semantically written data into the write queue.
		/// <para>For example, a block cipher stream might operate an 16 byte blocks.
		/// A call to <see cref="Flush"/> on this stream would pad an incomplete block
		/// to 16 bytes, encrypt it, and queue it for writing.</para>
		/// </summary>
		public abstract void Flush ();
		
		/// <summary>
		/// Close this instance. The currently active reader is cancelled,
		/// the write queue is cleared.
		/// </summary>
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

		/// <summary>
		/// Releases all resource used by the <see cref="Manos.IO.Stream"/> object.
		/// </summary>
		/// <remarks>
		/// Call <see cref="Dispose()"/> when you are finished using the <see cref="Manos.IO.Stream"/>. The
		/// <see cref="Dispose()"/> method leaves the <see cref="Manos.IO.Stream"/> in an unusable state. After calling
		/// <see cref="Dispose()"/>, you must release all references to the <see cref="Manos.IO.Stream"/> so the garbage
		/// collector can reclaim the memory that the <see cref="Manos.IO.Stream"/> was occupying.
		/// </remarks>
		public void Dispose ()
		{
			Dispose (true);
			GC.SuppressFinalize (this);
		}

		/// <summary>
		/// Dispose the current instance.
		/// </summary>
		/// <param name='disposing'>
		/// <c>true</c>, if the method was called by <see cref="Dispose()"/>,
		/// <c>false</c> if it was called from a finalizer.
		/// </param>
		protected virtual void Dispose (bool disposing)
		{
			Close ();
		}
		
		/// <summary>
		/// Raises the data callback, if set.
		/// </summary>
		protected virtual void RaiseData (ByteBuffer data)
		{
			if (onData != null) {
				onData (data);
			}
		}
		
		/// <summary>
		/// Raises the error callback, if set.
		/// </summary>
		protected virtual void RaiseError (Exception exception)
		{
			if (onError != null) {
				onError (exception);
			}
		}
		
		/// <summary>
		/// Raises the end of stream callback, if set.
		/// </summary>
		protected virtual void RaiseEndOfStream ()
		{
			if (onEndOfStream != null) {
				onEndOfStream ();
			}
		}
		
		/// <summary>
		/// Writes a single buffer to the stream. Must return a positive value or <c>0</c>
		/// for successful writes, and a negative value for unsuccessful writes.
		/// Unsuccessful write pause the writing process, successful writes consume the
		/// returned number of bytes from the write queue.
		/// </summary>
		/// <returns>
		/// The number of bytes written, or a negative value on unsuccessful write.
		/// </returns>
		protected abstract int WriteSingleBuffer (ByteBuffer buffer);
		
		/// <summary>
		/// Handles one write operation. If the write queue is empty, or the buffer
		/// produced by the currently writing sequence is <c>null</c>, the writing
		/// process is paused.
		/// </summary>
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
		
		/// <summary>
		/// Writes the current buffer to the stream via <see cref="WriteSingleBuffer"/>.
		/// A non-negative value returned by <see cref="WriteSingleBuffer"/> consumes that
		/// number of bytes from the write queue, a negative value pauses the writing
		/// process.
		/// </summary>
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
		
		/// <summary>
		/// Ensures that a buffer to be written to the stream exists.
		/// </summary>
		/// <returns>
		/// <c>true</c>, iff there is a buffer that can be written to the stream.
		/// </returns>
		protected virtual bool EnsureActiveBuffer ()
		{
			if (currentBuffer == null && EnsureActiveWriter ()) {
				if (currentWriter.MoveNext ()) {
					currentBuffer = currentWriter.Current;
					return true;
				} else {
					currentWriter.Dispose ();
					currentWriter = null;
					return EnsureActiveBuffer ();
				}
			}
			return false;
		}
		
		/// <summary>
		/// Ensures that a sequence to be written to the stream exists.
		/// </summary>
		/// <returns>
		/// <c>true</c>, iff there is a sequence that can be written to the stream.
		/// </returns>
		protected virtual bool EnsureActiveWriter ()
		{
			if (currentWriter == null && writeQueue.Count > 0) {
				currentWriter = writeQueue.Dequeue ().GetEnumerator ();
			}
			return currentWriter != null;
		}
	}
}

