using System;
using System.Collections.Generic;

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
	public abstract class FragmentStream<TFragment> : IStream<TFragment>
		where TFragment : class
	{
		/// <summary>
		/// Enumeration of results a write operation can produce.
		/// </summary>
		protected enum WriteResult
		{
			/// <summary>
			/// The write failed in some way. Pause the writing process.
			/// </summary>
			Error,
			/// <summary>
			/// The write succeeded and has written the entire fragment.
			/// Consume the fragment.
			/// </summary>
			Consume,
			/// <summary>
			/// The write succeeded and has not written the entire fragment.
			/// Continue writing the fragment as soon as possible.
			/// </summary>
			Continue
		}
		
		Action<TFragment> onData;
		Action<Exception> onError;
		Action onEndOfStream;
		IDisposable currentReader;
		Context context;
		bool disposed;
		// write queue handling
		TFragment currentFragment;
		IEnumerator<TFragment> currentWriter;
		Queue<IEnumerable<TFragment>> writeQueue;
		
		/// <summary>
		/// Initializes a new instance of the <see cref="Manos.IO.FragmentStream{TFragment}"/> class.
		/// </summary>
		/// <param name='context'>
		/// The context this instance will be bound to.
		/// </param>
		protected FragmentStream (Context context)
		{
			if (context == null)
				throw new ArgumentNullException ("context");
			
			this.context = context;
		}
		
		/// <summary>
		/// Gets the context this stream is bound to.
		/// </summary>
		public virtual Context Context {
			get { return context; }
		}

		/// <summary>
		/// Cancels the current reader and clears the set of reader callbacks.
		/// </summary>
		protected virtual void CancelReader ()
		{
			CheckDisposed ();
			
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
			FragmentStream<TFragment> parent;
			
			/// <summary>
			/// Initializes a new instance of the <see cref="Manos.IO.FragmentStream{TFragment}.ReaderHandle"/> class.
			/// </summary>
			public ReaderHandle (FragmentStream<TFragment> parent)
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
		public virtual IDisposable Read (Action<TFragment> onData, Action<Exception> onError, Action onEndOfStream)
		{
			CheckDisposed ();
			
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
		/// Places a sequence of fragments into the write queue.
		/// The sequence is not touched, only when the first piece of data in the
		/// sequence may be written to the stream, the enumeration is started.
		/// This allows for data generators that produce large amounts of data, but
		/// have a very small memory footprint.
		/// <para>The sequence may return an arbitrary number of fragments.</para>
		/// <para><c>null</c> fragments pause writing.</para>
		/// </summary>
		/// <exception cref='ArgumentNullException'>
		/// Is thrown when an argument passed to a method is invalid because it is <see langword="null" /> .
		/// </exception>
		public virtual void Write (IEnumerable<TFragment> data)
		{
			CheckDisposed ();
			
			if (data == null)
				throw new ArgumentNullException ("data");
			
			if (writeQueue == null) {
				writeQueue = new Queue<IEnumerable<TFragment>> ();
			}
			
			writeQueue.Enqueue (data);
		}
		
		static IEnumerable<TFragment> SingleFragment (TFragment fragment)
		{
			yield return fragment;
		}
		
		/// <summary>
		/// Places a single buffer into the write queue.
		/// </summary>
		public virtual void Write (TFragment data)
		{
			CheckDisposed ();
			
			Write (SingleFragment (data));
		}
		
		/// <summary>
		/// Releases unmanaged resources and performs other cleanup operations before the
		/// <see cref="Manos.IO.FragmentStream{TFragment}"/> is
		/// reclaimed by garbage collection.
		/// </summary>
		~FragmentStream ()
		{
			Dispose (false);
		}

		/// <summary>
		/// Gets or sets the position of the stream.
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
		/// Seeks by <paramref name="delta"/> fragments. A positive <paramref name="delta"/>
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
			Dispose ();
		}

		/// <summary>
		/// Releases all resource used by the <see cref="Manos.IO.FragmentStream{TFragment}"/> object.
		/// </summary>
		/// <remarks>
		/// Call <see cref="Dispose()"/> when you are finished using the <see cref="Manos.IO.FragmentStream{TFragment}"/>. The
		/// <see cref="Dispose()"/> method leaves the <see cref="Manos.IO.FragmentStream{TFragment}"/> in an unusable state. After calling
		/// <see cref="Dispose()"/>, you must release all references to the <see cref="Manos.IO.FragmentStream{TFragment}"/> so the garbage
		/// collector can reclaim the memory that the <see cref="Manos.IO.FragmentStream{TFragment}"/> was occupying.
		/// </remarks>
		public void Dispose ()
		{
			Dispose (true);
			GC.SuppressFinalize (this);
		}
		
		/// <summary>
		/// Checks whether the object has been disposed.
		/// </summary>
		/// <exception cref='ObjectDisposedException'>
		/// Is thrown when an operation is performed on a disposed object.
		/// </exception>
		protected virtual void CheckDisposed ()
		{
			if (disposed)
				throw new ObjectDisposedException (GetType ().Name);
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
			if (currentReader != null) {
				onData = null;
				onEndOfStream = null;
				onError = null;
				currentReader = null;
			}
			if (writeQueue != null) {
				if (currentWriter != null) {
					currentWriter.Dispose ();
					currentWriter = null;
				}
				currentFragment = null;
				writeQueue.Clear ();
				writeQueue = null;
			}
			disposed = true;
		}
		
		/// <summary>
		/// Raises the data callback, if set.
		/// </summary>
		protected virtual void RaiseData (TFragment data)
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
		/// Writes a single fragment.
		/// </summary>
		/// <returns>
		/// See <seealso cref="WriteResult"/> for result values.
		/// </returns>
		/// <param name='fragment'>
		/// The fragment to write.
		/// </param>
		protected abstract WriteResult WriteSingleFragment (TFragment fragment);
		
		/// <summary>
		/// Handles one write operation. If the write queue is empty, or the fragment
		/// produced by the currently writing sequence is <c>null</c>, the writing
		/// process is paused.
		/// </summary>
		protected virtual void HandleWrite ()
		{
			if (writeQueue == null) {
				throw new InvalidOperationException ();
			}
			if (!EnsureActiveFragment () || currentFragment == null) {
				PauseWriting ();
			} else {
				WriteCurrentFragment ();
			}
		}
		
		/// <summary>
		/// Writes the current fragment to the stream via <see cref="WriteSingleFragment"/>.
		/// </summary>
		protected virtual void WriteCurrentFragment ()
		{
			var sent = WriteSingleFragment (currentFragment);
			switch (sent) {
				case WriteResult.Consume:
					currentFragment = null;
					break;
					
				case WriteResult.Error:
					PauseWriting ();
					break;
					
				case WriteResult.Continue:
					// no error, continue
					break;
			}
		}
		
		/// <summary>
		/// Ensures that a fragment to be written exists.
		/// </summary>
		/// <returns>
		/// <c>true</c>, iff there is a fragment that can be written.
		/// </returns>
		protected virtual bool EnsureActiveFragment ()
		{
			if (currentFragment == null && EnsureActiveWriter ()) {
				if (currentWriter.MoveNext ()) {
					currentFragment = currentWriter.Current;
					return true;
				} else {
					currentWriter.Dispose ();
					currentWriter = null;
					return EnsureActiveFragment ();
				}
			}
			return currentFragment != null;
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
		
		/// <summary>
		/// Size of the fragment in fragment units.
		/// </summary>
		/// <returns>
		/// The size.
		/// </returns>
		/// <param name='fragment'>
		/// Fragment.
		/// </param>
		protected abstract long FragmentSize (TFragment fragment);
	}
}

