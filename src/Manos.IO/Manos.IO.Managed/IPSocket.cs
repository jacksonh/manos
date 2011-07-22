using System;
using System.Net;

namespace Manos.IO.Managed
{
	abstract class IPSocket<TFragment, TStream> : Socket<TFragment, TStream, IPEndPoint>
		where TFragment : class
		where TStream : IStream<TFragment>
	{
		protected System.Net.Sockets.Socket socket;
		protected bool disposed;
		
		protected IPSocket (Context context, AddressFamily addressFamily, ProtocolFamily protocolFamily)
			: base (context, addressFamily)
		{
			var family = addressFamily == AddressFamily.InterNetwork
				? System.Net.Sockets.AddressFamily.InterNetwork
				: System.Net.Sockets.AddressFamily.InterNetworkV6;
				
			var type = protocolFamily == ProtocolFamily.Tcp
				? System.Net.Sockets.SocketType.Stream
				: System.Net.Sockets.SocketType.Dgram;
				
			var protocol = protocolFamily == ProtocolFamily.Tcp
				? System.Net.Sockets.ProtocolType.Tcp
				: System.Net.Sockets.ProtocolType.Udp;
			
			this.socket = new System.Net.Sockets.Socket (family, type, protocol);
		}
		
		protected IPSocket (Context context, AddressFamily addressFamily, System.Net.Sockets.Socket socket)
			: base (context, addressFamily)
		{
			this.socket = socket;
		}
		
		public AddressFamily AddressFamily {
			get {
				return socket.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork
					? AddressFamily.InterNetwork
					: AddressFamily.InterNetworkV6;
			}
		}
		
		IPEndPoint localname;

		public override IPEndPoint LocalEndpoint {
			get {
				if (localname == null) {
					var ep = (System.Net.IPEndPoint) socket.LocalEndPoint;
					localname = new IPEndPoint (new IPAddress (ep.Address), ep.Port);
				}
				return localname;
			}
		}
		
		IPEndPoint peername;

		public override IPEndPoint RemoteEndpoint {
			get {
				if (peername == null) {
					var ep = (System.Net.IPEndPoint) socket.RemoteEndPoint;
					peername = new IPEndPoint (new IPAddress (ep.Address), ep.Port);
				}
				return peername;
			}
		}
		
		public bool IsConnected {
			get { return socket.Connected; }
		}
		
		public new Context Context {
			get { return (Context) base.Context; }
		}
		
		public override void Bind (IPEndPoint endpoint)
		{
			socket.Bind (new System.Net.IPEndPoint (endpoint.Address.address, endpoint.Port));
		}
		
		protected virtual void CheckDisposed ()
		{
			if (disposed)
				throw new ObjectDisposedException (GetType ().Name);
		}
		
		protected override void Dispose (bool disposing)
		{
			socket.Dispose ();
			disposed = true;
			base.Dispose (disposing);
		}
	}
}

