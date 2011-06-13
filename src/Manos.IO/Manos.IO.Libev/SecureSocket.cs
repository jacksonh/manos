using System;
using System.Runtime.InteropServices;
using Libev;

namespace Manos.IO.Libev
{
	class SecureSocket : EventedSocket
	{
		Action<Socket> acceptCallback;
		IntPtr tlsContext;
		SecureSocketStream stream;
		
		class SecureSocketStream : EventedStream
		{
			SecureSocket parent;
			IntPtr tlsContext;
			byte [] receiveBuffer = new byte[4096];
			long position;

			public SecureSocketStream (SecureSocket parent, IntPtr handle, IntPtr tlsContext)
				: base (parent.Context, handle)
			{
				this.parent = parent;
				this.tlsContext = tlsContext;
            }


			public override long Position {
				get { return position; }
				set { SeekTo (value); }
			}

			public override bool CanRead {
				get { return true; }
			}

			public override bool CanWrite {
				get { return true; }
			}

			public override void Close ()
			{
				if (parent == null) {
					return;
				}
				
				RaiseEndOfStream ();
				
				int res = manos_tls_close (tlsContext);

				if (res < 0) {
					Console.Error.WriteLine ("Error '{0}' closing socket: {1}", res, Handle.ToInt32 ());
					Console.Error.WriteLine (Environment.StackTrace);
				}
				
				receiveBuffer = null;
				parent = null;
				
				base.Close ();
			}

			public override void Flush ()
			{
			}

			protected override void HandleRead ()
			{
				switch (parent.state) {
					case SocketState.Invalid:
						throw new InvalidOperationException ();
						
					case SocketState.Listening:
						HandleAccept ();
						break;
						
					case SocketState.Open:
						HandleData ();
						break;
				}
			}

			void HandleAccept ()
			{
				int error = 0;
			
				while (error == 0) {
					IntPtr clientTlsContext;
					SocketInfo socketInfo;
				
					error = manos_tls_accept (tlsContext, out clientTlsContext, out socketInfo);
					if (error == 0) {
						var socket = new SecureSocket (parent.Context, socketInfo, clientTlsContext);
						parent.acceptCallback (socket);
					}
				}
			}

			void HandleData ()
			{
				int err;
				int limit = (int) Math.Min (receiveBuffer.Length, readLimit ?? long.MaxValue);
				var received = manos_tls_receive (tlsContext, receiveBuffer, limit, out err);
				if (received < 0 && err != 0 || received == 0) {
					if (received < 0) {
						RaiseError (new Exception ());
					}
					Close ();
				} else if (received > 0) {
					RaiseData (new ByteBuffer (receiveBuffer, 0, received));
				}
			}

			protected override void RaiseData (ByteBuffer data)
			{
				position += data.Length;
				base.RaiseData (data);
			}

			protected override int WriteSingleBuffer (ByteBuffer buffer)
			{
				int err;
				return manos_tls_send (tlsContext, buffer.Bytes, buffer.Position, buffer.Length, out err);
			}

			[DllImport ("libmanos", CallingConvention = CallingConvention.Cdecl)]
			private static extern int manos_tls_accept (IntPtr tls, out IntPtr client, out SocketInfo info);

			[DllImport ("libmanos", CallingConvention = CallingConvention.Cdecl)]
			private static extern int manos_tls_receive (IntPtr tls, byte [] data, int len, out int error);

			[DllImport ("libmanos", CallingConvention = CallingConvention.Cdecl)]
			private static extern int manos_tls_send (IntPtr tls, byte [] buffer, int offset, int len, out int error);

			[DllImport ("libmanos", CallingConvention = CallingConvention.Cdecl)]
			private static extern int manos_tls_close (IntPtr tls);
		}
		
		public SecureSocket (Context context, string certFile, string keyFile)
			: base (context)
		{
			int err = manos_tls_init (out tlsContext, certFile, keyFile);
			if (err != 0) {
				throw new InvalidOperationException (
					string.Format ("Error {0}: failed to initialize TLS socket with keypair ({1}, {2})", 
						err, certFile, keyFile));
			}
		}

		SecureSocket (Context context, SocketInfo info, IntPtr tlsContext)
			: base (context, info)
		{
			stream = new SecureSocketStream (this, new IntPtr (info.fd), tlsContext);
			this.tlsContext = tlsContext;
			this.state = Socket.SocketState.Open;
		}

		public override Stream GetSocketStream ()
		{
			if (state != Socket.SocketState.Open)
				throw new InvalidOperationException ();
			return stream;
		}

		public override void Connect (string host, int port, Action callback)
		{
			throw new NotSupportedException ();
		}

		public override void Listen (string host, int port, Action<Socket> callback)
		{
			if (callback == null)
				throw new ArgumentNullException ("callback");
			
			if (state != Socket.SocketState.Invalid)
				throw new InvalidOperationException ("Socket already in use");
			
			this.acceptCallback = callback;
			
			int error;
			int fd = manos_tls_listen (tlsContext, host, port, 128, out error);

			if (fd < 0) {
				if (error == 98)
					throw new Exception (String.Format ("Address {0}::{1} is already in use.", host, port));
				throw new Exception (String.Format ("An error occurred while trying to liste to {0}:{1} errno: {2}", host, port, error));
			}
			
			state = Socket.SocketState.Listening;
			
			stream = new SecureSocketStream (this, new IntPtr (fd), tlsContext);
			stream.ResumeReading ();
		}

		public override void Close ()
		{
			if (stream != null) {
				stream.Close ();
				stream = null;
			}
			base.Close ();
		}

		public void RedoHandshake ()
		{
			manos_tls_redo_handshake (tlsContext);
		}

		[DllImport ("libmanos", CallingConvention = CallingConvention.Cdecl)]
		private static extern int manos_tls_init (out IntPtr tls, string certFile, string keyFile);

		[DllImport ("libmanos", CallingConvention = CallingConvention.Cdecl)]
		private static extern int manos_tls_listen (IntPtr tls, string host, int port, int backlog, out int error);

		[DllImport ("libmanos", CallingConvention = CallingConvention.Cdecl)]
		private static extern int manos_tls_redo_handshake (IntPtr tls);
	}
}

