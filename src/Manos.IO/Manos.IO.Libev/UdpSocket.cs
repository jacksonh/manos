using System;
using System.Net;

namespace Manos.IO.Libev
{
	class UdpSocket : IPSocket<UdpPacket, IStream<UdpPacket>>, IUdpSocket
	{
		UdpStream stream;
		
		class UdpStream : EventedStream<UdpPacket>
		{
			UdpSocket parent;
			byte [] buffer = new byte[64 * 1024];
			
			internal UdpStream (UdpSocket socket, IntPtr handle)
				: base (socket.Context, handle)
			{
				this.parent = socket;
			}
			
			public override long Position {
				get { throw new NotSupportedException (); }
				set { throw new NotSupportedException (); }
			}
			
			public override bool CanRead {
				get { return true; }
			}
			
			public override bool CanWrite {
				get { return true; }
			}
			
			public override void Flush ()
			{
			}
			
			protected override void Dispose (bool disposing)
			{
				if (parent != null) {
					RaiseEndOfStream ();
				
					parent = null;
					buffer = null;
				}
				base.Dispose (disposing);
			}
			
			protected override void HandleRead ()
			{
				int size, error;
				IPEndPoint source;
				
				if (parent.IsConnected) {
					size = SocketFunctions.manos_socket_receive (Handle.ToInt32 (), buffer, buffer.Length, out error);
					source = parent.RemoteEndpoint;
				} else {
					ManosIPEndpoint ep;
					size = SocketFunctions.manos_socket_receivefrom_ip (Handle.ToInt32 (), buffer, buffer.Length,
						out ep, out error);
					source = ep;
				}
				
				if (size < 0 && error != 0) {
					RaiseError (Errors.SocketStreamFailure ("Read failure", error));
					Close ();
				} else {
					RaiseData (buffer, size, source);
				}
			}
			
			void RaiseData (byte[] data, int dataLength, IPEndPoint source)
			{
				var copy = new byte[dataLength];
				Buffer.BlockCopy (data, 0, copy, 0, copy.Length);
				RaiseData (new UdpPacket (
					source,
					new ByteBuffer (copy)));
			}
			
			protected override WriteResult WriteSingleFragment (UdpPacket packet)
			{
				int len, error;
				
				if (parent.IsConnected) {
					len = SocketFunctions.manos_socket_send (Handle.ToInt32 (), packet.Buffer.Bytes,
						packet.Buffer.Position, packet.Buffer.Length, out error);
				} else {
					ManosIPEndpoint ep = packet.IPEndPoint;
					len = SocketFunctions.manos_socket_sendto_ip (Handle.ToInt32 (), packet.Buffer.Bytes,
						packet.Buffer.Position, packet.Buffer.Length, ref ep, out error);
				}
				
				if (len < 0) {
					RaiseError (Errors.SocketStreamFailure ("Write failure", error));
					return WriteResult.Error;
				}
				return WriteResult.Consume;
			}
			
			protected override long FragmentSize (UdpPacket packet)
			{
				return 1;
			}
		}
		
		public UdpSocket (Context context, AddressFamily addressFamily)
			: base (context, addressFamily, ProtocolFamily.Udp)
		{
		}
		
		public override void Connect (IPEndPoint endpoint, Action callback, Action<Exception> error)
		{
			CheckDisposed ();
			
			if (endpoint == null)
				throw new ArgumentNullException ("endpoint");
			if (callback == null)
				throw new ArgumentNullException ("callback");
			if (error == null)
				throw new ArgumentNullException ("error");
			
			int err;
			ManosIPEndpoint ep = endpoint;
			err = SocketFunctions.manos_socket_connect_ip (fd, ref ep, out err);
			if (err != 0) {
				throw Errors.SocketFailure ("Connect failure", err);
			} else {
				localname = endpoint;
			}
			IsConnected = true;
			callback ();
		}
		
		protected override void Dispose (bool disposing)
		{
			if (stream != null) {
				stream.Close ();
				stream = null;
			}
			base.Dispose (disposing);
		}
		
		public override IStream<UdpPacket> GetSocketStream ()
		{
			CheckDisposed ();
			
			if (stream == null) {
				stream = new UdpStream (this, new IntPtr (fd));
			}
			return stream;
		}
	}
}

