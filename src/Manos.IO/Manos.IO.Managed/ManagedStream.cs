using System;
using System.Threading;
using System.Collections.Generic;

namespace Manos.IO.Managed
{
	abstract class ManagedStream<TFragment> : FragmentStream<TFragment>
		where TFragment : class
	{
		protected byte [] buffer;
		protected bool readAllowed, writeAllowed;
		Timer readTimer, writeTimer;
		int readTimeoutInterval = -1;
		int writeTimeoutInterval = -1;
		
		protected ManagedStream (Context ctx, int bufferSize)
			: base (ctx)
		{
			this.buffer = new byte[bufferSize];
		}
		
		public new Context Context {
			get { return (Context) base.Context; }
		}
			
		public override bool CanTimeout {
			get { return true; }
		}
			
		public override TimeSpan ReadTimeout {
			get { return readTimer == null ? TimeSpan.Zero : TimeSpan.FromMilliseconds (readTimeoutInterval); }
			set {
				if (value < TimeSpan.Zero)
					throw new ArgumentException ("value");
					
				readTimeoutInterval = value == TimeSpan.Zero ? -1 : (int) value.TotalMilliseconds;
					
				if (readTimer == null) {
					readTimer = new Timer (HandleReadTimerElapsed);
				}
				readTimer.Change (readTimeoutInterval, readTimeoutInterval);
			}
		}

		public override TimeSpan WriteTimeout {
			get { return writeTimer == null ? TimeSpan.Zero : TimeSpan.FromMilliseconds (writeTimeoutInterval); }
			set {
				if (value < TimeSpan.Zero)
					throw new ArgumentException ("value");
					
				writeTimeoutInterval = value == TimeSpan.Zero ? -1 : (int) value.TotalMilliseconds;
					
				if (writeTimer == null) {
					writeTimer = new Timer (HandleWriteTimerElapsed);
				}
				writeTimer.Change (writeTimeoutInterval, writeTimeoutInterval);
			}
		}
		
		protected void ResetReadTimeout ()
		{
			if (readTimer != null) {
				readTimer.Change (readTimeoutInterval, readTimeoutInterval);
			}
		}
		
		protected void ResetWriteTimeout ()
		{
			if (writeTimer != null) {
				writeTimer.Change (writeTimeoutInterval, writeTimeoutInterval);
			}
		}

		void HandleReadTimerElapsed (object state)
		{
			if (readAllowed) {
				RaiseError (new TimeoutException ());
				PauseReading ();
			}
		}

		void HandleWriteTimerElapsed (object state)
		{
			if (writeAllowed) {
				RaiseError (new TimeoutException ());
				PauseWriting ();
			}
		}

		public override IDisposable Read (Action<TFragment> onData, Action<Exception> onError, Action onClose)
		{
			var result = base.Read (onData, onError, onClose);
			ResumeReading ();
			return result;
		}
		
		public override void Write(IEnumerable<TFragment> data)
		{
			base.Write(data);
			ResumeWriting ();
		}
		
		public override void ResumeReading ()
		{
			CheckDisposed ();
			
			if (!readAllowed) {
				readAllowed = true;
				DispatchRead ();
			}
		}
		
		public override void ResumeWriting ()
		{
			CheckDisposed ();
			
			if (!writeAllowed) {
				writeAllowed = true;
				HandleWrite ();
			}
		}

		public override void PauseReading ()
		{
			CheckDisposed ();
			
			readAllowed = false;
		}

		public override void PauseWriting ()
		{
			CheckDisposed ();
			
			writeAllowed = false;
		}

		public override void Flush ()
		{
		}
		
		protected virtual void DispatchRead ()
		{
			if (readAllowed) {
				DoRead ();
			}
		}
		
		protected override void HandleWrite ()
		{
			if (writeAllowed) {
				base.HandleWrite ();
			}
		}
		
		protected abstract void DoRead ();
		
		protected override void Dispose (bool disposing)
		{
			buffer = null;
			if (readTimer != null) {
				readTimer.Dispose ();
			}
			if (writeTimer != null) {
				writeTimer.Dispose ();
			}
			readTimer = null;
			writeTimer = null;
			base.Dispose (disposing);
		}
	}
}

