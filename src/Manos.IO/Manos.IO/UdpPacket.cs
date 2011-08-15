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
		public UdpPacket (string address, int port, ByteBuffer buffer)
		{
			this.Address = address;
			this.Port = port;
			this.Buffer = buffer;
		}
		
		/// <summary>
		/// The IP address in string format of the sender/receiver depending on UdpSocket function
		/// </summary>
		public string Address {
			get;
			private set;
		}
		
		/// <summary>
		/// The port of the sender/receiver depending on UdpSocket functino
		/// </summary>
		public int Port {
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

