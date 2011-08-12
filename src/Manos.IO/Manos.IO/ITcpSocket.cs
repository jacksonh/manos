using System;
using System.Net;

namespace Manos.IO
{
	public interface ITcpSocket : IStreamSocket<ByteBuffer, IByteStream, IPEndPoint>
	{
	}
}

