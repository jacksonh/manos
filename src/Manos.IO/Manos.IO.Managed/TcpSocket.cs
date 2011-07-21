using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using Manos.IO;
using System.Threading;

namespace Manos.IO.Managed
{
	class TcpSocket : IPSocket<ByteBuffer, IByteStream>, ITcpSocket, ITcpServerSocket
	{
		TcpStream stream;
		bool wasDisposing;
		
		class TcpStream : ManagedByteStream, ISendfileCapable
		{
			TcpSocket parent;
			
			internal TcpStream (TcpSocket parent)
				: base (parent.Context, 4 * 1024)
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
			
			protected override void Dispose (bool disposing)
			{
				if (!parent.disposed) {
					parent.socket.BeginDisconnect (false, ar => {
						Context.Enqueue (delegate {
							try {
								((System.Net.Sockets.Socket) ar.AsyncState).EndDisconnect (ar);
								((System.Net.Sockets.Socket) ar.AsyncState).Dispose ();
							} catch {
							}
				
							RaiseEndOfStream ();
							
							parent.EndDispose ();
				
							base.Dispose (disposing);
						});
					}, parent.socket);
				}
			}
			
			protected override void DoRead ()
			{
				SocketError se;
				int length = (int) Math.Min (readLimit ?? long.MaxValue, buffer.Length);
				parent.socket.BeginReceive (buffer, 0, length, SocketFlags.None, out se, ReadCallback, null);
			}

			void ReadCallback (IAsyncResult ar)
			{
				Context.Enqueue (delegate {
					if (!parent.disposed) {
						ResetReadTimeout ();
				
						SocketError error;
						int len = parent.socket.EndReceive (ar, out error);
				
						if (error != SocketError.Success) {
							RaiseError (new Manos.IO.SocketException ("Read failure", error));
						} else if (len == 0) {
							RaiseEndOfStream ();
							Close ();
						} else {
							byte [] newBuffer = new byte [len];
							Buffer.BlockCopy (buffer, 0, newBuffer, 0, len);
						
							RaiseData (new ByteBuffer (newBuffer));
							DispatchRead ();
						}
					}
				});
			}
			
			protected override WriteResult WriteSingleFragment (ByteBuffer fragment)
			{
				parent.socket.BeginSend (fragment.Bytes, fragment.Position, fragment.Length, SocketFlags.None, WriteCallback, null);
				return WriteResult.Consume;
			}
			
			void WriteCallback (IAsyncResult ar)
			{
				Context.Enqueue (delegate {
					if (!parent.disposed) {
						ResetWriteTimeout ();
					
						SocketError err;
						parent.socket.EndSend (ar, out err);
						if (err != SocketError.Success) {
							RaiseError (new Manos.IO.SocketException ("Write failure", err));
						} else {
							HandleWrite ();
						}
					}
				});
			}
			
			public void SendFile (string file)
			{
				parent.socket.BeginSendFile (file, ar => {
					parent.socket.EndSendFile (ar);
				}, null);
			}
		}
		
		public TcpSocket (Context context, AddressFamily addressFamily)
			: base (context, addressFamily, ProtocolFamily.Tcp)
		{
		}
		
		TcpSocket (Context context, AddressFamily addressFamily, System.Net.Sockets.Socket socket)
			: base (context, addressFamily, socket)
		{
		}
		
		public override void Connect (IPEndPoint endpoint, Action callback, Action<Exception> error)
		{
			if (callback == null)
				throw new ArgumentNullException ("callback");
			if (error == null)
				throw new ArgumentNullException ("error");
			
			socket.BeginConnect (endpoint, (ar) => {
				Context.Enqueue (delegate {
					if (!disposed) {
						try {
							socket.EndConnect (ar);
							callback ();
						} catch (System.Net.Sockets.SocketException e) {
							error (new Manos.IO.SocketException ("Connect failure", e.SocketErrorCode));
						}
					}
				});
			}, null);
		}
		
		protected override void Dispose (bool disposing)
		{
			GetSocketStream ().Dispose ();
			wasDisposing = disposing;
		}
		
		void EndDispose ()
		{
			disposed = true;
			base.Dispose (wasDisposing);
		}
		
		public override IByteStream GetSocketStream ()
		{
			CheckDisposed ();
			
			if (stream == null) {
				stream = new TcpStream (this);
			}
			return stream;
		}
		
		public void Listen (int backlog, Action<ITcpSocket> callback)
		{
			try {
				socket.Listen (backlog);
				AcceptOne (callback);
			} catch (System.Net.Sockets.SocketException e) {
				throw new Manos.IO.SocketException ("Listen failure", e.SocketErrorCode);
			}
		}
		
		void AcceptOne (Action<ITcpSocket> callback)
		{
			socket.BeginAccept (ar => {
				if (!disposed) {
					var sock = socket.EndAccept (ar);
					
					Context.Enqueue (delegate {
						callback (new TcpSocket (Context, AddressFamily, sock));
						AcceptOne (callback);
					});
				}
			}, null);
		}
	}
}
