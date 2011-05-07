using System;

namespace Manos.IO
{
	public abstract class Socket : IDisposable
	{
		protected enum SocketState
		{
			Invalid,
			Listening,
			Open
		}
		
		protected SocketState state;
		protected string address;
		protected int port;

		public string Address {
			get {
				if (state == Socket.SocketState.Invalid)
					throw new InvalidOperationException ();
				return address;
			}
		}

		public int Port {
			get {
				if (state == Socket.SocketState.Invalid)
					throw new InvalidOperationException ();
				return port;
			}
		}

		public abstract Stream GetSocketStream ();

		public abstract void Connect (string host, int port, Action callback);

		public virtual void Connect (int port, Action callback)
		{
			Connect ("127.0.0.1", port, callback);
		}

		public abstract void Listen (string host, int port, Action<Socket> callback);

		~Socket ()
		{
			Dispose (false);
		}

		public virtual void Close ()
		{
		}

		public void Dispose ()
		{
			Dispose (true);
			GC.SuppressFinalize (this);
		}

		protected void Dispose (bool disposing)
		{
			Close ();
		}
	}
}

