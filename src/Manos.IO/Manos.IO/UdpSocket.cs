using System;

namespace Manos.IO
{
	/// <summary>
	/// Udp Packet representation for handling udp sockets
	/// </summary>
	public class UdpPacket
	{
		/// <summary>
		/// The IP address in string format of the sender/receiver depending on UdpSocket function
		/// </summary>
		public string Address { get; set; }
		/// <summary>
		/// The port of the sender/receiver depending on UdpSocket functino
		/// </summary>
		public int Port { get; set; }
		/// <summary>
		/// The received buffer
		/// </summary>
		public ByteBuffer Buffer { get; set; }
	}
	
	/// <summary>
	/// Base class for asynchronous udp handling.
	/// </summary>
	public abstract class UdpSocket : IDisposable
	{
		/// <summary>
		/// Bind the socket to listen on a host and port
		/// </summary>
		/// <param name="host">
		/// The ip address on which to bind <see cref="System.String"/>
		/// </param>
		/// <param name="port">
		/// The port on which to bind <see cref="System.Int32"/>
		/// </param>
		/// <param name="readCallback">
		/// A callback which receives an instande of the UdpPacket class <see cref="Action<UdpPacket>"/>
		/// </param>
		public abstract void Listen (string host, int port, Action<UdpPacket> readCallback);
		/// <summary>
		/// The 
		/// </summary>
		public abstract void Close ();
		
		/// <summary>
		/// Releases all resource used by the <see cref="Manos.IO.UdpSocket"/> object.
		/// </summary>
		/// <remarks>
		/// Call <see cref="Dispose()"/> when you are finished using the <see cref="Manos.IO.UdpSocket"/>. The
		/// <see cref="Dispose()"/> method leaves the <see cref="Manos.IO.UdpSocket"/> in an unusable state. After calling
		/// <see cref="Dispose()"/>, you must release all references to the <see cref="Manos.IO.UdpSocket"/> so the garbage
		/// collector can reclaim the memory that the <see cref="Manos.IO.UdpSocket"/> was occupying.
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
		protected virtual void Dispose (bool disposing)
		{
			Close ();
		}
	}
}

