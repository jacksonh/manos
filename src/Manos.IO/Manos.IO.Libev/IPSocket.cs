using System;
using System.Net;

namespace Manos.IO.Libev
{
	abstract class IPSocket<TFragment, TStream> : Socket<TFragment, TStream, IPEndPoint>
		where TFragment : class
		where TStream : IStream<TFragment>
	{
		protected IPEndPoint localname, peername;
		protected int fd;
		
		protected IPSocket (Context context, AddressFamily addressFamily, ProtocolFamily protocolFamily)
			: base (context, addressFamily)
		{
			int err;
			fd = SocketFunctions.manos_socket_create ((int) addressFamily, (int) protocolFamily, out err);
			if (fd < 0) {
				throw new Exception ();
			}
		}
		
		public override IPEndPoint LocalEndpoint {
			get {
				if (localname == null) {
					int err;
					ManosIPEndpoint ep;
					err = SocketFunctions.manos_socket_localname_ip (fd, out ep, out err);
					if (err != 0) {
						throw new Exception ();
					}
					localname = ep;
				}
				return localname;
			}
		}
			
		public override IPEndPoint RemoteEndpoint {
			get {
				if (peername == null) {
					int err;
					ManosIPEndpoint ep;
					err = SocketFunctions.manos_socket_peername_ip (fd, out ep, out err);
					if (err != 0) {
						throw new Exception ();
					}
					peername = ep;
				}
				return peername;
			}
		}

		public new Context Context {
			get { return (Context) base.Context; }
		}
		
		public override void Bind (IPEndPoint endpoint)
		{
			int err;
			ManosIPEndpoint ep = endpoint;
			err = SocketFunctions.manos_socket_bind_ip (fd, ref ep, out err);
			if (err != 0) {
				throw new Exception ();
			} else {
				localname = endpoint;
			}
		}
		
		public override void Close ()
		{
			int err;
			SocketFunctions.manos_socket_close (fd, out err);
			base.Close ();
		}
	}
}

