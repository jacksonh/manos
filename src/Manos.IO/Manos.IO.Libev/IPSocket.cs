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
				throw Errors.SocketFailure ("Could not create socket", err);
			}
		}
		
		protected IPSocket (Context context, AddressFamily addressFamily, int fd)
			: base (context, addressFamily)
		{
			this.fd = fd;
		}
		
		public override IPEndPoint LocalEndpoint {
			get {
				CheckDisposed ();
				
				if (localname == null) {
					int err;
					ManosIPEndpoint ep;
					var result = SocketFunctions.manos_socket_localname_ip (fd, out ep, out err);
					if (err != 0) {
						throw Errors.SocketFailure ("Could not get local address", err);
					}
					localname = ep;
				}
				return localname;
			}
		}
			
		public override IPEndPoint RemoteEndpoint {
			get {
				CheckDisposed ();
				
				if (peername == null) {
					int err;
					ManosIPEndpoint ep;
					var result = SocketFunctions.manos_socket_peername_ip (fd, out ep, out err);
					if (err != 0) {
						throw Errors.SocketFailure ("Could not get remote address", err);
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
			CheckDisposed ();
			
			if (endpoint == null)
				throw new ArgumentNullException ("endpoint");
			
			int err;
			ManosIPEndpoint ep = endpoint;
			var result = SocketFunctions.manos_socket_bind_ip (fd, ref ep, out err);
			if (err != 0) {
				throw Errors.SocketFailure ("Could not bind to address", err);
			} else {
				localname = endpoint;
			}
			IsBound = true;
		}
			
		protected void CheckDisposed ()
		{
			if (fd == 0)
				throw new ObjectDisposedException (GetType ().ToString ());
		}
		
		protected override void Dispose (bool disposing)
		{
			if (fd != 0) {
				int err;
				SocketFunctions.manos_socket_close (fd, out err);
				fd = 0;
			}
			base.Dispose (disposing);
		}
	}
}

