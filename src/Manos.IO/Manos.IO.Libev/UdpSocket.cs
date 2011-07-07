using System;
using Libev;
using System.Runtime.InteropServices;

namespace Manos.IO.Libev
{
	class UdpSocket : Manos.IO.UdpSocket
	{
		Context Context { get; set; }
		
		byte [] buffer = new byte[64 * 1024];
		IOWatcher readWatcher;
		IntPtr readFd;
		Action<UdpPacket> readCallback;
		
		internal UdpSocket (Manos.IO.Context context)
		{
			Context = (Context) context;
		}
		
		public override void Listen (string host, int port, Action<UdpPacket> readCallback)
		{
			int error;
			
			readFd = new IntPtr (manos_dgram_socket_listen(host, port, out error));
			
			if (readFd.ToInt32() < 0) {
				throw new Exception (string.Format ("An error ocurred while trying to connect to {0}:{1} errno {2}", host, port, error));
			}
			
			readWatcher = new IOWatcher (readFd, EventTypes.Read, Context.Loop, HandleReadReady);
			readWatcher.Start();
			
			this.readCallback = readCallback;
		}
		
		void HandleReadReady (IOWatcher watcher, EventTypes revents)
		{
			int size;
			int error;
			SocketInfo socketInfo;
			size = manos_socket_receive_from (readFd.ToInt32(), buffer, buffer.Length, 0, out socketInfo, out error);
			
			if (size < 0 && error != 0 || size == 0) {
				Close ();
			}
			
			var info = new UdpPacket();
			
			info.Address = socketInfo.Address.ToString();
			info.Port = socketInfo.port;
			info.Buffer = new ByteBuffer(buffer, 0, size);
			
			readCallback (info);
		}
		
		public override void Close ()
		{
			buffer = null;
			
			if (readFd == IntPtr.Zero) {
				return;
			}
			
			int error;
			int res = manos_socket_close (readFd.ToInt32 (), out error);

			if (res < 0) {
				Console.Error.WriteLine ("Error '{0}' closing socket: {1}", error, readFd.ToInt32 ());
				Console.Error.WriteLine (Environment.StackTrace);
			}
		}
		
		[DllImport ("libmanos", CallingConvention = CallingConvention.Cdecl)]
		private static extern int manos_dgram_socket_listen (string host, int port, out int err);
		
		[DllImport ("libmanos", CallingConvention = CallingConvention.Cdecl)]
		private static extern int manos_socket_close (int fd, out int err);
		
		[DllImport ("libmanos", CallingConvention = CallingConvention.Cdecl)]
		private static extern int manos_socket_receive_from (int fd, byte [] buffer, int max, int flags, out SocketInfo info, out int err);	
		
	}
}

