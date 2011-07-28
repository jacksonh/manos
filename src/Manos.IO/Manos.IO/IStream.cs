using System;
using System.Collections.Generic;

namespace Manos.IO
{
	/// <summary>
	/// Represent an asynchronous stream. Unlike synchronous streams,
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
	public interface IStream<TFragment> : IDisposable
	{
		/// <summary>
		/// Gets the context this stream is bound to.
		/// </summary>
		Context Context {
			get;
		}

		/// <summary>
		/// Replaces the current set of reader callbacks with the given set of reader callbacks.
		/// The returned value may be used to indicate that no further callback invocations should
		/// occur.
		/// </summary>
		/// <exception cref='ArgumentNullException'>
		/// Is thrown when any argument passed to the method is <see langword="null" /> .
		/// </exception>
		IDisposable Read (Action<TFragment> onData, Action<Exception> onError, Action onEndOfStream);

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
		void Write (IEnumerable<TFragment> data);
		
		/// <summary>
		/// Places a single buffer into the write queue.
		/// </summary>
		void Write (TFragment data);

		/// <summary>
		/// Gets or sets the position of the stream in fragment units.
		/// </summary>
		long Position {
			get;
			set;
		}
		
		/// <summary>
		/// Gets a value indicating whether this instance can read.
		/// </summary>
		/// <value>
		/// <c>true</c> if this instance can read; otherwise, <c>false</c>.
		/// </value>
		bool CanRead {
			get;
		}
		
		/// <summary>
		/// Gets a value indicating whether this instance can write.
		/// </summary>
		/// <value>
		/// <c>true</c> if this instance can write; otherwise, <c>false</c>.
		/// </value>
		bool CanWrite {
			get;
		}
		
		/// <summary>
		/// Gets a value indicating whether this instance can seek.
		/// </summary>
		/// <value>
		/// <c>true</c> if this instance can seek; otherwise, <c>false</c>.
		/// </value>
		bool CanSeek {
			get;
		}
		
		/// <summary>
		/// Gets a value indicating whether this instance can timeout.
		/// </summary>
		/// <value>
		/// <c>true</c> if this instance can timeout; otherwise, <c>false</c>.
		/// </value>
		bool CanTimeout {
			get;
		}
		
		/// <summary>
		/// Gets or sets the read timeout.
		/// </summary>
		TimeSpan ReadTimeout {
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the write timeout.
		/// </summary>
		TimeSpan WriteTimeout {
			get;
			set;
		}
		
		/// <summary>
		/// Instructs the stream to resume reading when it is not reading yet.
		/// </summary>
		void ResumeReading ();
		
		/// <summary>
		/// Resumes writing.
		/// </summary>
		void ResumeWriting ();
		
		/// <summary>
		/// Pauses reading.
		/// </summary>
		void PauseReading ();
		
		/// <summary>
		/// Pauses writing.
		/// </summary>
		void PauseWriting ();
		
		/// <summary>
		/// Seeks by <paramref name="delta"/> fragment units. A positive <paramref name="delta"/>
		/// will seek forward, a negative <paramref name="delta"/> will seek backwards.
		/// </summary>
		void SeekBy (long delta);
		
		/// <summary>
		/// Seeks to absolute position <paramref name="position"/> fragment units in the stream.
		/// </summary>
		void SeekTo (long position);
		
		/// <summary>
		/// Flush all buffers held by this instance, if applicable. This need not
		/// flush the write queue, it must however place equivalents for all
		/// semantically written data into the write queue.
		/// <para>For example, a block cipher stream might operate an 16 byte blocks.
		/// A call to <see cref="Flush"/> on this stream would pad an incomplete block
		/// to 16 bytes, encrypt it, and queue it for writing.</para>
		/// </summary>
		void Flush ();
		
		/// <summary>
		/// Close this instance. The currently active reader is cancelled,
		/// the write queue is cleared.
		/// </summary>
		void Close ();
	}
}

