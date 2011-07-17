using System;
using System.Collections.Generic;

namespace Manos.IO
{
	abstract class FragmentStream<TFragment> : IStream<TFragment>
		where TFragment : class
	{
		protected enum WriteResult
		{
			Error,
			Consume,
			Continue
		}
		
		Action<TFragment> onData;
		Action<Exception> onError;
		Action onEndOfStream;
		IDisposable currentReader;
		Context context;
		// write queue handling
		TFragment currentFragment;
		IEnumerator<TFragment> currentWriter;
		Queue<IEnumerable<TFragment>> writeQueue;
		
		protected FragmentStream (Context context)
		{
			if (context == null)
				throw new ArgumentNullException ("context");
			
			this.context = context;
		}
		
		public virtual Context Context {
			get { return context; }
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
			FragmentStream<TFragment> parent;
			
			public ReaderHandle (FragmentStream<TFragment> parent)
			{
				this.parent = parent;
			}
			
			public void Dispose ()
			{
				if (parent.currentReader == this)
					parent.CancelReader ();
			}
		}
		
		public virtual IDisposable Read (Action<TFragment> onData, Action<Exception> onError, Action onEndOfStream)
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

		public virtual void Write (IEnumerable<TFragment> data)
		{
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
		
		public virtual void Write (TFragment data)
		{
			Write (SingleFragment (data));
		}
		
		~FragmentStream ()
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

		public abstract void ResumeReading (long forFragments);
		
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
				currentFragment = null;
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
		
		protected virtual void RaiseData (TFragment data)
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
		
		protected abstract WriteResult WriteSingleFragment (TFragment fragment);
		
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
		
		protected virtual bool EnsureActiveWriter ()
		{
			if (currentWriter == null && writeQueue.Count > 0) {
				currentWriter = writeQueue.Dequeue ().GetEnumerator ();
			}
			return currentWriter != null;
		}
		
		protected abstract long FragmentSize (TFragment fragment);
	}
}

