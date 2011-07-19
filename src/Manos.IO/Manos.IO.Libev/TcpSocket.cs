using System;
using Libev;
using System.Net;

namespace Manos.IO.Libev
{
	class TcpSocket : IPSocket<ByteBuffer, IByteStream>, ITcpSocket, ITcpServerSocket
	{
		IOWatcher listener;
		TcpSocketStream stream;
		
		class TcpSocketStream : EventedByteStream, ISendfileCapable
		{
			TcpSocket parent;
			byte [] receiveBuffer = new byte[4096];
			
			public TcpSocketStream (TcpSocket parent, IntPtr handle)
				: base (parent.Context, handle)
			{
				this.parent = parent;
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

			public void SendFile (string file)
			{
				Write (new SendFileOperation (Context, this, file));
			}
			
			public override void Close ()
			{
				if (parent == null) {
					return;
				}
				
				RaiseEndOfStream ();
				
				receiveBuffer = null;
				parent = null;
				
				base.Close ();
			}
			
			public override void Flush ()
			{
			}
			
			protected override void HandleRead ()
			{
				int err;
				int limit = (int) Math.Min (receiveBuffer.Length, readLimit ?? long.MaxValue);
				var received = SocketFunctions.manos_socket_receive (Handle.ToInt32 (), receiveBuffer, limit, out err);
				if (received < 0 && err != 0 || received == 0) {
					if (received < 0) {
						RaiseError (new Exception ());
					}
					Close ();
				} else if (received > 0) {
					byte [] newBuffer = new byte [received];
					Buffer.BlockCopy (receiveBuffer, 0, newBuffer, 0, received);
					
					RaiseData (new ByteBuffer (newBuffer));
				}
			}

			protected override WriteResult WriteSingleFragment (ByteBuffer buffer)
			{
				int err;
				int sent = SocketFunctions.manos_socket_send (Handle.ToInt32 (), buffer.Bytes, buffer.Position,
					buffer.Length, out err);
				if (sent < 0 && err != 0) {
					return WriteResult.Error;
				} else {
					buffer.Skip (sent);
					return buffer.Length == 0 ? WriteResult.Consume : WriteResult.Continue;
				}
			}
		}
		
		public TcpSocket (Context context, AddressFamily addressFamily)
			: base (context, addressFamily, ProtocolFamily.Tcp)
		{
		}

		TcpSocket (Context context, AddressFamily addressFamily, int fd, IPEndPoint local, IPEndPoint remote)
			: base (context, addressFamily, fd)
		{
			this.stream = new TcpSocketStream (this, new IntPtr (fd));
			this.localname = local;
			this.peername = remote;
		}
		
		public override IByteStream GetSocketStream ()
		{
			if (stream == null) {
				stream = new TcpSocketStream (this, new IntPtr (fd));
			}
			return stream;
		}
		
		public void Listen (int backlog, Action<ITcpSocket> callback)
		{
			int error;
			var result = SocketFunctions.manos_socket_listen (fd, backlog, out error);
			
			if (result < 0) {
				if (error == 98)
					throw new Exception (String.Format ("Address {0} is already in use.", LocalEndpoint));
				throw new Exception (String.Format ("An error occurred while trying to liste to {} errno: {1}", LocalEndpoint, error));
			}
			
			listener = new IOWatcher (new IntPtr (fd), EventTypes.Read, Context.Loop, delegate {
				ManosIPEndpoint ep;
				var client = SocketFunctions.manos_socket_accept (fd, out ep, out error);
				if (client < 0 && error != 0) {
					throw new Exception (string.Format ("Error while accepting: {0}", Errors.ErrorToString (error)));
				} else if (client > 0) {
					var socket = new TcpSocket (Context, AddressFamily, client, LocalEndpoint, ep);
					callback (socket);
				}
			});
			listener.Start ();
		}
		
		public override void Close ()
		{
			if (listener != null) {
				listener.Stop ();
				listener.Dispose ();
				listener = null;
			} else if (stream != null) {
				stream.Close ();
				stream = null;
			}
			base.Close ();
		}
		
		public override void Connect (System.Net.IPEndPoint endpoint, Action callback)
		{
			int err;
			ManosIPEndpoint ep = endpoint;
			err = SocketFunctions.manos_socket_connect_ip (fd, ref ep, out err);
			if (err != 0) {
				throw new Exception ();
			} else {
				var connectWatcher = new IOWatcher (new IntPtr (fd), EventTypes.Write, Context.Loop, (watcher, revents) => {
					watcher.Stop ();
					watcher.Dispose ();
					localname = endpoint;
				
					IsConnected = true;
					
					callback ();
				});
				connectWatcher.Start ();
			}
		}
	}
}

