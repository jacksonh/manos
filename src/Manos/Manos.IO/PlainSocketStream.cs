using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;


using Libev;
using Manos.Collections;
using System.Net;

namespace Manos.IO
{
	public class PlainSocketStream : SocketStream
	{
		int fd;
		string host;
		private static readonly int MAX_ACCEPT = 100;
		private SocketInfo [] accept_infos;

		public PlainSocketStream (IOLoop ioloop) : base (ioloop)
		{
		}

		PlainSocketStream (SocketInfo info, IOLoop ioloop) : base (info, ioloop)
		{
			fd = info.fd;

			if (fd > 0) {
				SetHandle (fd);
			}
		}

		private void SetHandle (int fd)
		{
			SetHandle (new IntPtr (fd));
		}

		public override void Close ()
		{

			//		IOWatcher.ReleaseHandle (socket, Handle);

			base.Close ();

			if (fd == -1)
				return;

			int error;
			int res = manos_socket_close (fd, out error);

			if (res < 0) {
				Console.Error.WriteLine ("Error '{0}' closing socket: {1}", error, fd);
				Console.Error.WriteLine (Environment.StackTrace);
			}

			fd = -1;
		}
		
		internal override SendFileOperation MakeSendFile(string file)
		{
#if !DISABLE_POSIX
			return new PosixSendFileOperation (file, null);
#else
			return new CopyingSendFileOperation (file, null);
#endif
		}

		public void Connect (string host, int port)
		{
			int error;
			fd = manos_socket_connect (host, port, out error);

			if (fd < 0)
				throw new Exception (String.Format ("An error occurred while trying to connect to {0}:{1} errno: {2}", host, port, error));
			
			
			IOWatcher iowatcher = new IOWatcher (new IntPtr (fd), EventTypes.Write, IOLoop.EventLoop, (l, w, r) => {
				w.Stop ();

				this.host = host;
				this.port = port;
				OnConnected ();
			});
			iowatcher.Start ();
		}

		public void Connect (int port)
		{
			Connect ("127.0.0.1", port);
		}

		public override void Listen (string host, int port)
		{
			int error;
			fd = manos_socket_listen (host, port, 128, out error);

			if (fd < 0) {
				if (error == 98)
					throw new Exception (String.Format ("Address {0}::{1} is already in use.", host, port));
				throw new Exception (String.Format ("An error occurred while trying to liste to {0}:{1} errno: {2}", host, port, error));
			}

			SetHandle (fd);

			DisableTimeout ();
			EnableReading ();
			state = SocketState.AcceptingConnections;
			accept_infos = new SocketInfo [MAX_ACCEPT];
		}

		private void OnConnected ()
		{
			SetHandle (fd);
			state = SocketState.Open;

			if (Connected != null)
				Connected (this);
		}

		protected override void AcceptConnections ()
		{
			int error;

			int amount = manos_socket_accept_many (fd, accept_infos, MAX_ACCEPT, out error);
			if (amount < 0)
				throw new Exception (String.Format ("Exception while accepting. errno: {0}", error));

//			Console.WriteLine ("Accepted: '{0}' connections.", amount);
			for (int i = 0; i < amount; i++) {
//				Console.WriteLine ("Accepted: '{0}'", accept_infos [i]);
				SocketStream iostream = new PlainSocketStream (accept_infos [i], IOLoop);
				OnConnectionAccepted (iostream);
			}
		}
		
		protected override int ReadOneChunk(out int error)
		{
			return manos_socket_receive (fd, ReadChunk, ReadChunk.Length, out error);
		}

		internal override int Send (ByteBufferS [] buffers, int length, out int error)
		{
			return manos_socket_send (fd, buffers, length, out error);
		}

		public event Action<SocketStream> Connected;

		[DllImport ("libmanos", CallingConvention = CallingConvention.Cdecl)]
		private static extern int manos_socket_connect (string host, int port, out int err);

		[DllImport ("libmanos", CallingConvention = CallingConvention.Cdecl)]
		private static extern int manos_socket_close (int fd, out int err);

		[DllImport ("libmanos", CallingConvention = CallingConvention.Cdecl)]
		private static extern int manos_socket_listen (string host, int port, int backlog, out int err);

		[DllImport ("libmanos", CallingConvention = CallingConvention.Cdecl)]
		private static extern int manos_socket_accept_many (int fd, SocketInfo [] infos, int max, out int err);

		[DllImport ("libmanos", CallingConvention = CallingConvention.Cdecl)]
		private static extern int manos_socket_receive (int fd, byte [] buffer, int max, out int err);

		[DllImport ("libmanos", CallingConvention = CallingConvention.Cdecl)]
		internal static extern int manos_socket_send (int fd, ByteBufferS [] buffers, int len, out int err);
	}
}

