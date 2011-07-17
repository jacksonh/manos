using System;

namespace Manos.IO
{
	/// <summary>
	/// Address families for use with sockets.
	/// </summary>
	public enum AddressFamily
	{
		/// <summary>
		/// Version 4 of the Internet Protocol.
		/// </summary>
		InterNetwork = 0,
		/// <summary>
		/// Version 6 of the Internet Protocol.
		/// </summary>
		InterNetworkV6 = 1
	}
}

