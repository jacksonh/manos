using System;

namespace Manos.IO
{
	/// <summary>
	/// Udp Packet representation for handling udp sockets
	/// </summary>
	public class UdpPacket
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="Manos.IO.UdpPacket"/> class.
		/// </summary>
		/// <param name='address'>
		/// Address the packet was received from/is to be sent to.
		/// </param>
		/// <param name='port'>
		/// Port the packet was received from/is to be sent to.
		/// </param>
		/// <param name='buffer'>
		/// Buffer received/to be sent.
		/// </param>
		public UdpPacket (IPEndPoint endPoint, ByteBuffer buffer)
		{
			this.IPEndPoint = endPoint;
			this.Buffer = buffer;
		}
		
		/// <summary>
		/// The IP endpoint of the sender/receiver depending on UdpSocket function
		/// </summary>
		public IPEndPoint IPEndPoint {
			get;
			private set;
		}
		
		/// <summary>
		/// The received buffer
		/// </summary>
		public ByteBuffer Buffer {
			get;
			private set;
		}
	}
}

