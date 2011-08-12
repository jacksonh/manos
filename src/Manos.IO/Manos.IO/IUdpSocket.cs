using System;
using System.Net;

namespace Manos.IO
{
	public interface IUdpSocket : IStreamSocket<UdpPacket, IStream<UdpPacket>, IPEndPoint>
	{
	}
}

