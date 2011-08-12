using System;
using System.Runtime.InteropServices;

namespace Manos.IO.Libev
{
	static class SocketFunctions
	{
		[DllImport ("libmanos", CallingConvention = CallingConvention.Cdecl)]
		public static extern int manos_socket_localname_ip (int fd, out ManosIPEndpoint ep, out int err);
		
		[DllImport ("libmanos", CallingConvention = CallingConvention.Cdecl)]
		public static extern int manos_socket_peername_ip (int fd, out ManosIPEndpoint ep, out int err);
		
		[DllImport ("libmanos", CallingConvention = CallingConvention.Cdecl)]
		public static extern int manos_socket_bind_ip (int fd, ref ManosIPEndpoint ep, out int err);
		
		[DllImport ("libmanos", CallingConvention = CallingConvention.Cdecl)]
		public static extern int manos_socket_connect_ip (int fd, ref ManosIPEndpoint ep, out int err);
		
		[DllImport ("libmanos", CallingConvention = CallingConvention.Cdecl)]
		public static extern int manos_socket_listen (int fd, int backlog, out int err);
		
		[DllImport ("libmanos", CallingConvention = CallingConvention.Cdecl)]
		public static extern int manos_socket_accept (int fd, out ManosIPEndpoint remote, out int err);
		
		[DllImport ("libmanos", CallingConvention = CallingConvention.Cdecl)]
		public static extern int manos_socket_create (int addressFamily, int protocolFamily, out int err);
		
		[DllImport ("libmanos", CallingConvention = CallingConvention.Cdecl)]
		public static extern int manos_socket_send (int fd, byte[] buffer, int offset, int length, out int err);
		
		[DllImport ("libmanos", CallingConvention = CallingConvention.Cdecl)]
		public static extern int manos_socket_receive (int fd, byte[] buffer, int length, out int err);
		
		[DllImport ("libmanos", CallingConvention = CallingConvention.Cdecl)]
		public static extern int manos_socket_sendto_ip (int fd, byte[] buffer, int offset, int length,
			ref ManosIPEndpoint to, out int err);
		
		[DllImport ("libmanos", CallingConvention = CallingConvention.Cdecl)]
		public static extern int manos_socket_receivefrom_ip (int fd, byte[] buffer, int length,
			out ManosIPEndpoint source, out int err);
		
		[DllImport ("libmanos", CallingConvention = CallingConvention.Cdecl)]
		public static extern int manos_socket_close (int fd, out int err);
	}
}

