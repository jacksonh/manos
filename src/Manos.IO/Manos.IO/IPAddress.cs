using System;

namespace Manos.IO
{
	public sealed class IPAddress
	{
		internal System.Net.IPAddress address;
		public static readonly IPAddress Any = new IPAddress (System.Net.IPAddress.Any);
		public static readonly IPAddress Broadcast = new IPAddress (System.Net.IPAddress.Broadcast);
		public static readonly IPAddress Loopback = new IPAddress (System.Net.IPAddress.Loopback);
		public static readonly IPAddress None = new IPAddress (System.Net.IPAddress.None);
		public static readonly IPAddress IPv6Any = new IPAddress (System.Net.IPAddress.IPv6Any);
		public static readonly IPAddress IPv6Loopback = new IPAddress (System.Net.IPAddress.IPv6Loopback);
		public static readonly IPAddress IPv6None = new IPAddress (System.Net.IPAddress.IPv6None);
		
		public IPAddress (byte[] address)
		{
			if (address == null)
				throw new ArgumentNullException ("address");
			
			this.address = new System.Net.IPAddress (address);
		}

		public IPAddress (byte[] address, long scopeId)
		{
			if (address == null)
				throw new ArgumentNullException ("address");
			if (address.Length != 16)
				throw new ArgumentException ("An invalid IP address was specified.", "address");
			
			this.address = new System.Net.IPAddress (address, scopeId);
		}
		
		internal IPAddress (System.Net.IPAddress address)
		{
			this.address = address;
		}

		public AddressFamily AddressFamily {
			get {
				return address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork
					? AddressFamily.InterNetwork
					: AddressFamily.InterNetworkV6;
			}
		}

		public bool IsIPv6LinkLocal {
			get { return address.IsIPv6LinkLocal; }
		}

		public bool IsIPv6SiteLocal {
			get { return address.IsIPv6SiteLocal; }
		}

		public bool IsIPv6Multicast {
			get { return address.IsIPv6Multicast; }
		}

		public long ScopeId {
			get {
				if (AddressFamily != AddressFamily.InterNetworkV6)
					throw new SocketException (System.Net.Sockets.SocketError.OperationNotSupported);
				return address.ScopeId;
			}
		}

		public byte[] GetAddressBytes ()
		{
			return address.GetAddressBytes ();
		}

		public static bool IsLoopback (IPAddress address)
		{
			return System.Net.IPAddress.IsLoopback (address.address);
		}

		public override string ToString ()
		{
			return address.ToString ();
		}

		public override bool Equals (object obj)
		{
			var other = obj as IPAddress;
			return other != null && other.address.Equals (address);
		}

		public override int GetHashCode ()
		{
			return address.GetHashCode ();
		}

		public static short HostToNetworkOrder (short host)
		{
			return System.Net.IPAddress.HostToNetworkOrder (host);
		}

		public static int HostToNetworkOrder (int host)
		{
			return System.Net.IPAddress.HostToNetworkOrder (host);
		}

		public static long HostToNetworkOrder (long host)
		{
			return System.Net.IPAddress.HostToNetworkOrder (host);
		}

		public static short NetworkToHostOrder (short network)
		{
			return System.Net.IPAddress.NetworkToHostOrder (network);
		}

		public static int NetworkToHostOrder (int network)
		{
			return System.Net.IPAddress.NetworkToHostOrder (network);
		}

		public static long NetworkToHostOrder (long network)
		{
			return System.Net.IPAddress.NetworkToHostOrder (network);
		}

		public static IPAddress Parse (string ipString)
		{
			IPAddress result;
			if (IPAddress.TryParse (ipString, out result)) {
				return result;
			}
			throw new FormatException ("An invalid IP address was specified.");
		}

		public static bool TryParse (string ipString, out IPAddress address)
		{
			if (ipString == null)
				throw new ArgumentNullException ("ipString");
			
			System.Net.IPAddress internalAddress;
			if (System.Net.IPAddress.TryParse (ipString, out internalAddress)) {
				address = new IPAddress (internalAddress);
				return true;
			}
			address = null;
			return false;
		}
	}
}

