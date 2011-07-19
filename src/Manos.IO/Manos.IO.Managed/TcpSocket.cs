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
		
		class TcpStream : ManagedByteStream, ISendfileCapable
		{
			TcpSocket parent;
			byte [] buffer = new byte[64 * 1024];
			long? readLimit;
			bool readAllowed, writeAllowed;
			Timer readTimer, writeTimer;
			int readTimeoutInterval = -1;
			int writeTimeoutInterval = -1;
			
			internal TcpStream (TcpSocket parent)
				: base (parent.Context)
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
			
			public override bool CanTimeout {
				get { return true; }
			}
			
			public override TimeSpan ReadTimeout {
				get { return readTimer == null ? TimeSpan.Zero : TimeSpan.FromMilliseconds (readTimeoutInterval); }
				set {
					if (value < TimeSpan.Zero)
						throw new ArgumentException ("value");
					
					readTimeoutInterval = value == TimeSpan.Zero ? -1 : (int) value.TotalMilliseconds;
					
					if (readTimer == null) {
						readTimer = new Timer (HandleReadTimerElapsed);
					}
					readTimer.Change (readTimeoutInterval, readTimeoutInterval);
				}
			}

			public override TimeSpan WriteTimeout {
				get { return writeTimer == null ? TimeSpan.Zero : TimeSpan.FromMilliseconds (writeTimeoutInterval); }
				set {
					if (value < TimeSpan.Zero)
						throw new ArgumentException ("value");
					
					writeTimeoutInterval = value == TimeSpan.Zero ? -1 : (int) value.TotalMilliseconds;
					
					if (writeTimer == null) {
						writeTimer = new Timer (HandleWriteTimerElapsed);
					}
					writeTimer.Change (writeTimeoutInterval, writeTimeoutInterval);
				}
			}

			void HandleReadTimerElapsed (object state)
			{
				if (readAllowed) {
					RaiseError (new TimeoutException ());
					PauseReading ();
				}
			}

			void HandleWriteTimerElapsed (object state)
			{
				if (writeAllowed) {
					RaiseError (new TimeoutException ());
					PauseWriting ();
				}
			}
			
			public override void ResumeReading ()
			{
				readLimit = null;
				if (!readAllowed) {
					readAllowed = true;
					Receive ();
				}
			}
			
			public override void ResumeReading (long forFragments)
			{
				if (forFragments < 0)
					throw new ArgumentException ("forFragments");

				readLimit = forFragments;
				if (!readAllowed) {
					readAllowed = true;
					Receive ();
				}
			}
			
			public override void Close ()
			{
				parent.socket.BeginDisconnect (false, ar => {
					Context.Enqueue (delegate {
						try {
							((System.Net.Sockets.Socket) ar.AsyncState).EndDisconnect (ar);
							((System.Net.Sockets.Socket) ar.AsyncState).Dispose ();
						} catch {
						}
				
						RaiseEndOfStream ();
						if (readTimer != null) {
							readTimer.Dispose ();
						}
						if (writeTimer != null) {
							writeTimer.Dispose ();
						}
						readTimer = null;
						writeTimer = null;
				
						base.Close ();
					});
				}, parent.socket);
			}
			
			public override void ResumeWriting ()
			{
				if (!writeAllowed) {
					writeAllowed = true;
					HandleWrite ();
				}
			}

			public override void PauseReading ()
			{
				readAllowed = false;
			}

			public override void PauseWriting ()
			{
				writeAllowed = false;
			}

			public override void Flush ()
			{
			}
			
			void Receive ()
			{
				SocketError se;
				int length = (int) Math.Min (readLimit ?? long.MaxValue, buffer.Length);
				parent.socket.BeginReceive (buffer, 0, length, SocketFlags.None, out se, ReadCallback, null);
			}

			void ReadCallback (IAsyncResult ar)
			{
				Context.Enqueue (delegate {
					if (readTimer != null) {
						readTimer.Change (readTimeoutInterval, readTimeoutInterval);
					}
				
					SocketError error;
					int len = parent.socket.EndReceive (ar, out error);
				
					if (error != SocketError.Success) {
						RaiseError (new SocketException ());
					} else if (len == 0) {
						RaiseEndOfStream ();
					} else {
						byte [] newBuffer = new byte [len];
						Buffer.BlockCopy (buffer, 0, newBuffer, 0, len);
						
						RaiseData (new ByteBuffer (newBuffer));
						Receive ();
					}
				});
			}
			
			protected override void HandleWrite ()
			{
				if (writeAllowed) {
					base.HandleWrite ();
				}
			}
			
			protected override WriteResult WriteSingleFragment (ByteBuffer fragment)
			{
				parent.socket.BeginSend (fragment.Bytes, fragment.Position, fragment.Length, SocketFlags.None, WriteCallback, null);
				return WriteResult.Consume;
			}
			
			void WriteCallback (IAsyncResult ar)
			{
				Context.Enqueue (delegate {
					if (writeTimer != null) {
						writeTimer.Change (writeTimeoutInterval, writeTimeoutInterval);
					}
					SocketError err;
					parent.socket.EndSend (ar, out err);
					if (err != SocketError.Success) {
						RaiseError (new SocketException ());
					} else {
						HandleWrite ();
					}
				});
			}
			
			public void SendFile(string file)
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
		
		public override void Connect (IPEndPoint endpoint, Action callback)
		{
			try {
				socket.BeginConnect (endpoint, (ar) => {
					Context.Enqueue (delegate {
						try {
							socket.EndConnect (ar);
							callback ();
						} catch {
						}
					});
				}, null);
			} catch {
			}
		}

		public override void Close ()
		{
			GetSocketStream ().Close ();
			base.Close ();
		}
		
		public override IByteStream GetSocketStream ()
		{
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
			} catch {
			}
		}
		
		void AcceptOne (Action<ITcpSocket> callback)
		{
			try {
				socket.BeginAccept (ar => {
					try {
						var sock = socket.EndAccept (ar);
					
						Context.Enqueue (delegate {
							callback (new TcpSocket (Context, AddressFamily, sock));
						});
					} catch {
					}
					AcceptOne (callback);
				}, null);
			} catch {
			}
		}
	}
}
