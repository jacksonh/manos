using System;
using System.Net;

namespace Manos.IO.Libev
{
	struct ManosIPEndpoint
	{
		int port;
		int is_ipv4;
		byte a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15, a16;
		
		public ManosIPEndpoint (IPEndPoint ep)
		{
			this.port = ep.Port;
			this.is_ipv4 = ep.AddressFamily == AddressFamily.InterNetwork ? 1 : 0;
			
			var b = ep.Address.GetAddressBytes ();
			
			this.a1 = ValueOrZero (b, 0);
			this.a2 = ValueOrZero (b, 1);
			this.a3 = ValueOrZero (b, 2);
			this.a4 = ValueOrZero (b, 3);
			this.a5 = ValueOrZero (b, 4);
			this.a6 = ValueOrZero (b, 5);
			this.a7 = ValueOrZero (b, 6);
			this.a8 = ValueOrZero (b, 7);
			this.a9 = ValueOrZero (b, 8);
			this.a10 = ValueOrZero (b, 9);
			this.a11 = ValueOrZero (b, 10);
			this.a12 = ValueOrZero (b, 11);
			this.a13 = ValueOrZero (b, 12);
			this.a14 = ValueOrZero (b, 13);
			this.a15 = ValueOrZero (b, 14);
			this.a16 = ValueOrZero (b, 15);
		}
		
		static byte ValueOrZero (byte[] b, int index)
		{
			if (index >= b.Length) {
				return 0;
			} else {
				return b [index];
			}
		}
		
		public IPAddress Address {
			get {
				if (is_ipv4 != 0) {
					return new IPAddress (new byte[] { a1, a2, a3, a4 });
				} else {
					return new IPAddress (new byte[] { a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15, a16 });
				}
			}
		}
		
		public int Port {
			get { return port; }
		}
		
		public static implicit operator ManosIPEndpoint (IPEndPoint ep)
		{
			return new ManosIPEndpoint (ep);
		}
		
		public static implicit operator IPEndPoint (ManosIPEndpoint ep)
		{
			return new IPEndPoint (ep.Address, ep.Port);
		}
	}
}

