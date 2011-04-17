using System;
using System.Runtime.InteropServices;
using System.Net;

namespace Manos.IO.Libev
{
	[StructLayout(LayoutKind.Sequential)]
	public struct SocketInfo
	{
		public int fd;
		public int port;
		public int is_ipv4;
		public byte a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15, a16;

		public IPAddress Address {
			get {
				if (is_ipv4 != 0) {
					return new IPAddress (new byte[] { a1, a2, a3, a4 });
				} else {
					return new IPAddress (new byte[] { a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15, a16 });
				}
			}
		}
	}
}

