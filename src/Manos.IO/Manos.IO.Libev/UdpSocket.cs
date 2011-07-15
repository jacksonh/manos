using System;
using System.Collections.Generic;
using Libev;
using System.Runtime.InteropServices;

namespace Manos.IO.Libev
{
	class UdpSocket : Manos.IO.UdpSocket
	{
		Context Context { get; set; }
		
		byte [] buffer = new byte[64 * 1024];
		IOWatcher readWatcher, writeWatcher;
		IntPtr handle;
		Action<UdpPacket> readCallback;
		
		internal UdpSocket (Manos.IO.Context context, AddressFamily addressFamily)
		{
			int err;
			
			Context = (Context) context;
			AddressFamily = addressFamily;
			handle = new IntPtr (manos_dgram_socket_create (addressFamily, out err));
			if (handle.ToInt32() < 0) {
				throw new Exception (string.Format ("An error occured while trying to create socket: {0} {1}", err, StringError(err)));	
			}
			
			readWatcher = new IOWatcher (handle, EventTypes.Read, Context.Loop, HandleReadReady);
			writeWatcher = new IOWatcher (handle, EventTypes.Write, Context.Loop, HandleWriteReady);
		}
		
		public override void Receive (Action<UdpPacket> readCallback)
		{
			readWatcher.Start();
			
			this.readCallback = readCallback;
		}
		
		void HandleWriteReady (IOWatcher watcher, EventTypes revents)
		{
			HandleWrite ();
		}
		
		void HandleReadReady (IOWatcher watcher, EventTypes revents)
		{
			int size;
			int error;
			SocketInfo socketInfo;
			size = manos_socket_receive_from (handle.ToInt32(), buffer, buffer.Length, 0, out socketInfo, out error);
			
			if (size < 0 && error != 0 || size == 0) {
				Close ();
			}
			
			var info = new UdpPacket();
			
			info.Address = socketInfo.Address.ToString();
			info.Port = socketInfo.port;
			info.Buffer = new ByteBuffer(buffer, 0, size);
			
			readCallback (info);
		}
		
		public override void Bind (string host, int port)
		{
			int ret = manos_dgram_socket_bind(handle.ToInt32(), host, port, AddressFamily);
			if (ret != 0) {
				throw new Exception (string.Format ("{0}:{1}", ret, StringError(ret)));
			}
		}
		
		public override void Send (IEnumerable<UdpPacket> packet)
		{
			base.Send (packet);
			ResumeWriting();
		}
		
		public override void ResumeWriting ()
		{
			if (!writeWatcher.IsRunning) {
				writeWatcher.Start ();	
			}
			HandleWrite ();
		}
		
		public override void PauseWriting ()
		{
			if (writeWatcher.IsRunning) {
				writeWatcher.Stop ();	
			}
		}
		
		public override void Close ()
		{
			buffer = null;
			
			if (handle == IntPtr.Zero) {
				return;
			}
			
			int error;
			int res = manos_socket_close (handle.ToInt32 (), out error);

			if (res < 0) {
				Console.Error.WriteLine ("Error '{0}' closing socket: {1}", StringError(error), handle.ToInt32 ());
				Console.Error.WriteLine (Environment.StackTrace);
			}
		}
		
		protected override int WriteSinglePacket (UdpPacket packet)
		{
			int len, error;
			len = manos_dgram_socket_sendto (handle.ToInt32(), packet.Address, packet.Port, AddressFamily, packet.Buffer.Bytes, packet.Buffer.Position, packet.Buffer.Length, out error);
			if (len < 0) {
				throw new Exception (string.Format ("{0}:{1}", error, StringError(error)));
			}
			return len;
		}
		
		
		private string StringError (int errorNumber)
		{
			return Marshal.PtrToStringAnsi (strerror(errorNumber));
		}
		
		[DllImport("__Internal")]
		private static extern IntPtr strerror (int errno);
		
		[DllImport ("libmanos", CallingConvention = CallingConvention.Cdecl)]
		private static extern int manos_dgram_socket_create (AddressFamily addressFamily, out int err);
		
		[DllImport ("libmanos", CallingConvention = CallingConvention.Cdecl)]
		private static extern int manos_dgram_socket_bind (int fd, string host, int port, AddressFamily addressFamily);
		
		[DllImport ("libmanos", CallingConvention = CallingConvention.Cdecl)]
		private static extern int manos_dgram_socket_listen (int fd, /*string host, int port, AddressFamily family,*/ int backlog, out int err);
		
		[DllImport ("libmanos", CallingConvention = CallingConvention.Cdecl)]
		private static extern int manos_socket_close (int fd, out int err);
		
		[DllImport ("libmanos", CallingConvention = CallingConvention.Cdecl)]
		private static extern int manos_socket_receive_from (int fd, byte [] buffer, int max, int flags, out SocketInfo info, out int err);	
		
		[DllImport ("libmanos", CallingConvention = CallingConvention.Cdecl)]
		private static extern int manos_dgram_socket_sendto (int fd, string host, int port, AddressFamily family, byte [] buffer, int offset, int length, out int err);
		
		
	}
}

