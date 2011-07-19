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
			
			public override void Close ()
			{
				if (parent == null) {
					return;
				}
				
				RaiseEndOfStream ();
				
				parent = null;
				buffer = null;
				
				base.Close ();
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
					RaiseError (new Exception ());
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
					source.Address.ToString (),
					source.Port,
					new ByteBuffer (copy)));
			}
			
			protected override WriteResult WriteSingleFragment (UdpPacket packet)
			{
				int len, error;
				
				if (parent.IsConnected) {
					len = SocketFunctions.manos_socket_send (Handle.ToInt32 (), packet.Buffer.Bytes,
						packet.Buffer.Position, packet.Buffer.Length, out error);
				} else {
					ManosIPEndpoint ep = new IPEndPoint (IPAddress.Parse (packet.Address), packet.Port);
					len = SocketFunctions.manos_socket_sendto_ip (Handle.ToInt32 (), packet.Buffer.Bytes,
						packet.Buffer.Position, packet.Buffer.Length, ref ep, out error);
				}
				
				if (len < 0) {
					RaiseError (new Exception (string.Format ("{0}:{1}", error, Errors.ErrorToString (error))));
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
		
		public override void Connect (IPEndPoint endpoint, Action callback)
		{
			int err;
			ManosIPEndpoint ep = endpoint;
			err = SocketFunctions.manos_socket_connect_ip (fd, ref ep, out err);
			if (err != 0) {
				throw new Exception ();
			} else {
				localname = endpoint;
			}
			IsConnected = true;
			callback ();
		}
		
		public override void Close ()
		{
			if (stream != null) {
				stream.Close ();
				stream = null;
			}
			base.Close ();
		}
		
		public override IStream<UdpPacket> GetSocketStream ()
		{
			if (stream == null) {
				stream = new UdpStream (this, new IntPtr (fd));
			}
			return stream;
		}
	}
}

