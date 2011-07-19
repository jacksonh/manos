using System;
using System.Threading;

namespace Manos.IO.Managed
{
	abstract class ManagedStream<TFragment> : FragmentStream<TFragment>
		where TFragment : class
	{
		protected byte [] buffer;
		protected long? readLimit;
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
		
		public override void ResumeReading ()
		{
			readLimit = null;
			if (!readAllowed) {
				readAllowed = true;
				DispatchRead ();
			}
		}
			
		public override void ResumeReading (long forFragments)
		{
			if (forFragments < 0)
				throw new ArgumentException ("forFragments");

			readLimit = forFragments;
			if (!readAllowed) {
				readAllowed = true;
				DispatchRead ();
			}
		}
		
		public override void ResumeWriting ()
		{
			if (!writeAllowed) {
				writeAllowed = true;
				HandleWrite ();
			}
		}

		public override void PauseReading ()
		{
			readAllowed = false;
		}

		public override void PauseWriting ()
		{
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
		
		protected override void RaiseData (TFragment data)
		{
			readLimit -= FragmentSize (data);
			if (readLimit < 0) {
				PauseReading ();
			}
			base.RaiseData (data);
		}
		
		protected abstract void DoRead ();
		
		protected override void Dispose (bool disposing)
		{
			buffer = null;
			base.Dispose (disposing);
		}
		
		public override void Close ()
		{
			if (readTimer != null) {
				readTimer.Dispose ();
			}
			if (writeTimer != null) {
				writeTimer.Dispose ();
			}
			readTimer = null;
			writeTimer = null;
			base.Close ();
		}
	}
}

