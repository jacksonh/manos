using System;

namespace Manos.IO
{
	/// <summary>
	/// Abstract base class for sockets bound to event loops.
	/// </summary>
	/// <exception cref='InvalidOperationException'>
	/// Is thrown when an operation cannot be performed.
	/// </exception>
	public abstract class Socket : IDisposable
	{
		/// <summary>
		/// States a socket must support. More states may be supported
		/// by specific implementations.
		/// </summary>
		protected enum SocketState
		{
			/// <summary>
			/// The socket is not connected to any peer.
			/// </summary>
			Invalid,
			/// <summary>
			/// The socket is waiting for connections.
			/// </summary>
			Listening,
			/// <summary>
			/// The socket has established a connection to a peer.
			/// </summary>
			Open
		}
		
		/// <summary>
		/// The state of the socket. This should initially be
		/// <see cref="SocketState.Invalid"/>.
		/// </summary>
		protected SocketState state;
		/// <summary>
		/// The address of the peer for open sockets, the address of the
		/// local socket for listening sockets.
		/// </summary>
		protected string address;
		/// <summary>
		/// The port of the peer for open sockets, the port of the
		/// local socket for listening sockets.
		/// </summary>
		protected int port;
		
		/// <summary>
		/// The address of the peer for open sockets, the address of the
		/// local socket for listening sockets.
		/// </summary>
		/// <exception cref='InvalidOperationException'>
		/// Is thrown when the socket is neither connected nor listening.
		/// </exception>
		public string Address {
			get {
				if (state == Socket.SocketState.Invalid)
					throw new InvalidOperationException ();
				return address;
			}
		}
		
		/// <summary>
		/// The port of the peer for open sockets, the port of the
		/// local socket for listening sockets.
		/// </summary>
		/// <exception cref='InvalidOperationException'>
		/// Is thrown when the socket is neither connected nor listening.
		/// </exception>
		public int Port {
			get {
				if (state == Socket.SocketState.Invalid)
					throw new InvalidOperationException ();
				return port;
			}
		}
		
		/// <summary>
		/// Gets the socket stream associated with the current socket.
		/// This stream will be bound to the event loop the current socket
		/// is bound to.
		/// </summary>
		/// <returns>
		/// The socket stream.
		/// </returns>
		public abstract Stream GetSocketStream ();
		
		/// <summary>
		/// Connect to the specified host at the specified port.
		/// Invoke <paramref name="callback"/> when the connection is established.
		/// </summary>
		/// <param name='host'>
		/// Host to connect to. This should be an IP address.
		/// </param>
		/// <param name='port'>
		/// Port to connect to.
		/// </param>
		/// <param name='callback'>
		/// Callback to invoke when the connection is established. May be <c>null</c>.
		/// </param>
		/// <exception cref="System.ArgumentException">
		/// <paramref name="host"/> is <c>null</c> or empty, or <paramref name="port"/>
		/// is not larger than <c>0</c>.</exception>
		public abstract void Connect (string host, int port, Action callback);
		
		/// <summary>
		/// Listen at the specified address under the specified port.
		/// Invoke <paramref name="callback"/> for every connection that has been
		/// accepted.
		/// </summary>
		/// <param name='host'>
		/// Host to listen at. This should be an IP address.
		/// </param>
		/// <param name='port'>
		/// Port to listen at.
		/// </param>
		/// <param name='callback'>
		/// Callback to invoke for every accepted connection.
		/// </param>
		/// <exception cref="System.ArgumentException">
		/// <paramref name="host"/> is <c>null</c> or empty,
		/// <paramref name="port"/> is not larger than <c>0</c>,
		/// or <paramref name="callback"/> is null.</exception>
		public abstract void Listen (string host, int port, Action<Socket> callback);
		
		/// <summary>
		/// Releases unmanaged resources and performs other cleanup operations before the <see cref="Manos.IO.Socket"/> is
		/// reclaimed by garbage collection.
		/// </summary>
		~Socket ()
		{
			Dispose (false);
		}
		
		/// <summary>
		/// Closes this instance and releases all resources associated with it.
		/// </summary>
		public virtual void Close ()
		{
		}
		
		/// <summary>
		/// Releases all resource used by the <see cref="Manos.IO.Socket"/> object.
		/// </summary>
		/// <remarks>
		/// Call <see cref="Dispose()"/> when you are finished using the <see cref="Manos.IO.Socket"/>. The
		/// <see cref="Dispose()"/> method leaves the <see cref="Manos.IO.Socket"/> in an unusable state. After calling
		/// <see cref="Dispose()"/>, you must release all references to the <see cref="Manos.IO.Socket"/> so the garbage
		/// collector can reclaim the memory that the <see cref="Manos.IO.Socket"/> was occupying.
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
		protected void Dispose (bool disposing)
		{
			Close ();
		}
	}
}

