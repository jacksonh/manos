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

		protected virtual void CancelReader ()
		{
			onData = null;
			onError = null;
			onClose = null;
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

		public abstract void Write (IEnumerable<ByteBuffer> data);

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
	}
}

