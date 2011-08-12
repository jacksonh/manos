using System;
using System.Net;

namespace Manos.IO
{
	public interface ITcpServerSocket : ISocket<IPEndPoint>
	{
		/// <summary>
		/// Listen for new connections, invoke the specified delegate for each accepted
		/// connection.
		/// </summary>
		void Listen (int backlog, Action<ITcpSocket> callback);
	}
}

