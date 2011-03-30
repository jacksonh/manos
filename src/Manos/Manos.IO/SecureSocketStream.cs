using System;
using Manos.Collections;
using System.Runtime.InteropServices;
using Libev;

namespace Manos.IO
{
	public class SecureSocketStream : SocketStream
	{
		private IntPtr tls;

		public SecureSocketStream (string certFile, string keyFile, IOLoop ioloop) : base (ioloop)
		{
			int err = manos_tls_init (out tls, certFile, keyFile);
			if (err != 0) {
				throw new InvalidOperationException (
					string.Format ("Error {0}: failed to initialize TLS socket with keypair ({1}, {2})", 
						err, certFile, keyFile));
			}
		}

		SecureSocketStream (IntPtr tls, SocketInfo info, IOLoop ioloop) : base (info, ioloop)
		{
			this.tls = tls;
			
			SetHandle (new IntPtr (info.fd));
		}

		public override void Close ()
		{
			base.Close ();

			if (tls == IntPtr.Zero)
				return;

			int res = manos_tls_close (tls);

			if (res < 0) {
				Console.Error.WriteLine ("Error '{0}' closing socket: {1}", res, tls);
				Console.Error.WriteLine (Environment.StackTrace);
			}
			
			tls = IntPtr.Zero;
		}
		
		internal override SendFileOperation MakeSendFile(string file)
		{
			return new CopyingSendFileOperation (file, null);
		}

		public override void Listen (string host, int port)
		{
			int error, fd;
			fd = manos_tls_listen (tls, host, port, 128, out error);

			if (fd < 0) {
				if (error == 98)
					throw new Exception (String.Format ("Address {0}::{1} is already in use.", host, port));
				throw new Exception (String.Format ("An error occurred while trying to liste to {0}:{1} errno: {2}", host, port, error));
			}

			SetHandle (new IntPtr (fd));

			DisableTimeout ();
			EnableReading ();
			state = SocketState.AcceptingConnections;
		}

		protected override void AcceptConnections ()
		{
			int error = 0;
			
			while (error == 0) {
				IntPtr client;
				SocketInfo accept_info;
				
				error = manos_tls_accept (tls, out client, out accept_info);
				if (error == 0) {
					var clientStream = new SecureSocketStream (client, accept_info, IOLoop);
					OnConnectionAccepted (clientStream);
				}
			}
		}

		protected override int ReadOneChunk (out int error)
		{
			return manos_tls_receive (tls, ReadChunk, ReadChunk.Length, out error);
		}

		internal override int Send (ByteBufferS [] buffers, int length, out int error)
		{
			return manos_tls_send (tls, buffers, length, out error);
		}

		public void RedoHandshake ()
		{
			manos_tls_redo_handshake (tls);
		}

		[DllImport ("libmanos", CallingConvention = CallingConvention.Cdecl)]
		private static extern int manos_tls_init (out IntPtr tls, string certFile, string keyFile);

		[DllImport ("libmanos", CallingConvention = CallingConvention.Cdecl)]
		private static extern int manos_tls_listen (IntPtr tls, string host, int port, int backlog, out int error);

		[DllImport ("libmanos", CallingConvention = CallingConvention.Cdecl)]
		private static extern int manos_tls_accept (IntPtr tls, out IntPtr client, out SocketInfo info);

		[DllImport ("libmanos", CallingConvention = CallingConvention.Cdecl)]
		private static extern int manos_tls_receive (IntPtr tls, byte [] data, int len, out int error);

		[DllImport ("libmanos", CallingConvention = CallingConvention.Cdecl)]
		private static extern int manos_tls_redo_handshake (IntPtr tls);

		[DllImport ("libmanos", CallingConvention = CallingConvention.Cdecl)]
		private static extern int manos_tls_send (IntPtr tls, ByteBufferS [] buffers, int len, out int error);

		[DllImport ("libmanos", CallingConvention = CallingConvention.Cdecl)]
		private static extern int manos_tls_close (IntPtr tls);
	}
}

