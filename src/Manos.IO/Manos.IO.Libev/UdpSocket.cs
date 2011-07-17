using System;
using System.Collections.Generic;
using Libev;
using System.Runtime.InteropServices;

namespace Manos.IO.Libev
{
	class UdpSocket : Manos.IO.UdpSocket
	{
		class UdpStream : EventedStream<UdpPacket>
		{
			UdpSocket socket;
			byte [] buffer = new byte[64 * 1024];
			
			internal UdpStream (UdpSocket socket, IntPtr handle)
				: base (socket.Context, handle)
			{
				this.socket = socket;
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
			
			protected override void HandleRead ()
			{
				int size;
				int error;
				SocketInfo socketInfo;
				size = manos_socket_receive_from (Handle.ToInt32 (), buffer, buffer.Length, 0, out socketInfo, out error);
				
				if (size < 0 && error != 0 || size == 0) {
					Close ();
				}
				
				byte [] newBuffer = new byte [size];
				Buffer.BlockCopy (buffer, 0, newBuffer, 0, size);
				
				var info = new UdpPacket (
					socketInfo.Address.ToString (),
					socketInfo.port,
					new ByteBuffer (newBuffer, 0, size));
				
				RaiseData (info);
			}
			
			protected override WriteResult WriteSingleFragment (UdpPacket packet)
			{
				int len, error;
				len = manos_dgram_socket_sendto (Handle.ToInt32 (), packet.Address, packet.Port, (int) socket.AddressFamily, packet.Buffer.Bytes, packet.Buffer.Position, packet.Buffer.Length, out error);
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
		
			[DllImport ("libmanos", CallingConvention = CallingConvention.Cdecl)]
			private static extern int manos_socket_receive_from (int fd, byte [] buffer, int max, int flags, out SocketInfo info, out int err);
		
			[DllImport ("libmanos", CallingConvention = CallingConvention.Cdecl)]
			private static extern int manos_dgram_socket_sendto (int fd, string host, int port, int family, byte [] buffer, int offset, int length, out int err);
		}
		
		IntPtr handle;
		UdpStream stream;
		
		internal UdpSocket (Context context, AddressFamily addressFamily)
			: base (context)
		{
			int err;
			
			AddressFamily = addressFamily;
			handle = new IntPtr (manos_dgram_socket_create (addressFamily, out err));
			if (handle.ToInt32 () < 0) {
				throw new Exception (string.Format ("An error occured while trying to create socket: {0} {1}", err, Errors.ErrorToString (err)));
			}
		}
		
		public new Context Context {
			get { return (Context) base.Context; }
		}
		
		public override IStream<UdpPacket> GetSocketStream()
		{
			if (stream == null) {
				stream = new UdpStream (this, handle);
			}
			return stream;
		}
		
		public override void Bind (string host, int port)
		{
			int ret = manos_dgram_socket_bind (handle.ToInt32 (), host, port, AddressFamily);
			if (ret != 0) {
				throw new Exception (string.Format ("{0}:{1}", ret, Errors.ErrorToString (ret)));
			}
		}
		
		public override void Close ()
		{
			if (handle == IntPtr.Zero) {
				return;
			}
			
			int error;
			int res = manos_socket_close (handle.ToInt32 (), out error);

			if (res < 0) {
				Console.Error.WriteLine ("Error '{0}' closing socket: {1}", Errors.ErrorToString (error), handle.ToInt32 ());
				Console.Error.WriteLine (Environment.StackTrace);
			}
		}
		
		[DllImport ("libmanos", CallingConvention = CallingConvention.Cdecl)]
		private static extern int manos_dgram_socket_create (AddressFamily addressFamily, out int err);
		
		[DllImport ("libmanos", CallingConvention = CallingConvention.Cdecl)]
		private static extern int manos_dgram_socket_bind (int fd, string host, int port, AddressFamily addressFamily);
		
		[DllImport ("libmanos", CallingConvention = CallingConvention.Cdecl)]
		private static extern int manos_socket_close (int fd, out int err);
	}
}

