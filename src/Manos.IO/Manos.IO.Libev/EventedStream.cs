using System;
using Libev;
using System.Collections.Generic;

namespace Manos.IO.Libev
{
	abstract class EventedStream<TFragment> : FragmentStream<TFragment>
		where TFragment : class
	{
		// readiness watchers
		IOWatcher readWatcher, writeWatcher;
		TimerWatcher readTimeoutWatcher, writeTimeoutWatcher;
		TimeSpan readTimeout, writeTimeout;
		DateTime? readTimeoutContinuation, writeTimeoutContinuation;

		protected EventedStream (Context context, IntPtr handle)
			: base (context)
		{
			if (handle == IntPtr.Zero)
				throw new ArgumentException ("handle");
			
			this.Handle = handle;
			
			this.readWatcher = new IOWatcher (Handle, EventTypes.Read, Context.Loop, HandleReadReady);
			this.writeWatcher = new IOWatcher (Handle, EventTypes.Write, Context.Loop, HandleWriteReady);
		}

		public override bool CanTimeout {
			get { return true; }
		}

		public override TimeSpan ReadTimeout {
			get { return readTimeout; }
			set {
				if (value < TimeSpan.Zero) 
					throw new ArgumentException ("value");
				readTimeout = value;
				if (readTimeoutWatcher == null) {
					readTimeoutWatcher = new TimerWatcher (readTimeout, Context.Loop, HandleReadTimeout);
				}
				readTimeoutWatcher.Repeat = readTimeout;
				readTimeoutWatcher.Again ();
			}
		}

		public override TimeSpan WriteTimeout {
			get { return writeTimeout; }
			set {
				if (value < TimeSpan.Zero) 
					throw new ArgumentException ("value");
				writeTimeout = value;
				if (writeTimeoutWatcher == null) {
					writeTimeoutWatcher = new TimerWatcher (writeTimeout, Context.Loop, HandleWriteTimeout);
				}
				writeTimeoutWatcher.Repeat = writeTimeout;
				writeTimeoutWatcher.Again ();
			}
		}

		void HandleReadTimeout (TimerWatcher watcher, EventTypes revents)
		{
			if (readTimeoutContinuation != null) {
				readTimeoutWatcher.Repeat = DateTime.Now - readTimeoutContinuation.Value;
				readTimeoutWatcher.Again ();
				readTimeoutContinuation = null;
			} else {
				RaiseError (new TimeoutException ());
				PauseReading ();
			}
		}

		void HandleWriteTimeout (TimerWatcher watcher, EventTypes revents)
		{
			if (writeTimeoutContinuation != null) {
				writeTimeoutWatcher.Repeat = DateTime.Now - writeTimeoutContinuation.Value;
				writeTimeoutWatcher.Again ();
				writeTimeoutContinuation = null;
			} else {
				RaiseError (new TimeoutException ());
				PauseWriting ();
			}
		}

		void HandleWriteReady (IOWatcher watcher, EventTypes revents)
		{
			if (writeTimeoutContinuation == null) {
				writeTimeoutContinuation = DateTime.Now;
			}
			HandleWrite ();
		}

		void HandleReadReady (IOWatcher watcher, EventTypes revents)
		{
			if (readTimeoutContinuation == null) {
				readTimeoutContinuation = DateTime.Now;
			}
			HandleRead ();
		}

		public new Context Context {
			get { return (Context) base.Context; }
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

		public override IDisposable Read (Action<TFragment> onData, Action<Exception> onError, Action onClose)
		{
			ResumeReading ();
			
			return base.Read (onData, onError, onClose);
		}

		public override void Write (IEnumerable<TFragment> data)
		{
			base.Write (data);
			ResumeWriting ();
		}
		
		protected override void Dispose(bool disposing)
		{
			if (Handle != IntPtr.Zero) {
				PauseReading ();
				PauseWriting ();

				readWatcher.Dispose ();
				writeWatcher.Dispose ();
				
				if (readTimeoutWatcher != null)
					readTimeoutWatcher.Dispose ();
				if (writeTimeoutWatcher != null)
					writeTimeoutWatcher.Dispose ();

				readWatcher = null;
				writeWatcher = null;
				readTimeoutWatcher = null;
				writeTimeoutWatcher = null;
			
				Handle = IntPtr.Zero;
			}
			base.Dispose (disposing);
		}

		protected abstract void HandleRead ();
	}
}

