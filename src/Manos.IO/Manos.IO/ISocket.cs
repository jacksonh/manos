using System;
using System.Net;

namespace Manos.IO
{
	/// <summary>
	/// Every socket is bound to an event loop and has it's end points identified by an appropriate
	/// implementing class of <see cref="System.Net.EndPoint"/>.
	/// <seealso cref="Manos.IO.IStream{TFragment}"/>
	/// </summary>
	public interface ISocket<TEndPoint> : IDisposable
		where TEndPoint : EndPoint
	{
		/// <summary>
		/// Gets the address family of the socket.
		/// </summary>
		/// <value>
		/// The address family.
		/// </value>
		AddressFamily AddressFamily {
			get;
		}
		
		/// <summary>
		/// Gets whether this socket is connected or not.
		/// </summary>
		/// <value>
		/// <c>true</c> if this socket is connected; otherwise, <c>false</c>.
		/// </value>
		bool IsConnected {
			get;
		}
		
		/// <summary>
		/// Gets whether this socket is bound to a local address.
		/// </summary>
		/// <value>
		/// <c>true</c> if this socket is bound; otherwise, <c>false</c>.
		/// </value>
		bool IsBound {
			get;
		}
		
		/// <summary>
		/// Gets the local endpoint of the socket.
		/// </summary>
		/// <value>
		/// The local endpoint.
		/// </value>
		TEndPoint LocalEndpoint {
			get;
		}
		
		/// <summary>
		/// Gets the remote endpoint of the socket. This may be <c>null</c> for
		/// unconnected or connectionless sockets.
		/// </summary>
		/// <value>
		/// The remote endpoint.
		/// </value>
		TEndPoint RemoteEndpoint {
			get;
		}
		
		/// <summary>
		/// Gets the context the socket is bound to.
		/// </summary>
		/// <value>
		/// The context.
		/// </value>
		Context Context {
			get;
		}
		
		/// <summary>
		/// Bind the socket to the specified local endpoint.
		/// </summary>
		/// <param name='endpoint'>
		/// Endpoint to bind to.
		/// </param>
		void Bind (TEndPoint endpoint);
		
		/// <summary>
		/// Connect the socket to the specified remote endpoint and invoke callback on
		/// success.
		/// </summary>
		/// <param name="endpoint">
		/// Endpoint to connect to.
		/// </param>
		/// <param name="callback">
		/// Callback to invoke on success.
		/// </param>
		/// <param name="error">
		/// Callback to invoke on error.
		/// </param>
		void Connect (TEndPoint endpoint, Action callback, Action<Exception> error);
		
		/// <summary>
		/// Close this socket and release all resources associated with it.
		/// </summary>
		void Close ();
	}
}

