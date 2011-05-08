using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using Manos.IO;
using Manos.Collections;

namespace Manos.Managed
{
	public class Socket : Manos.IO.Socket
	{
		System.Net.Sockets.Socket socket;
		Action connectedCallback;
		Action<Socket> acceptedCallback;
		Stream stream;
		IOLoop loop;

		public Socket (IOLoop loop)
		{
			if (loop == null)
				throw new ArgumentNullException ("loop");
			this.loop = loop;
		}

		Socket (IOLoop loop, System.Net.Sockets.Socket socket) : this (loop)
		{
			this.socket = socket;
			this.address = ((IPEndPoint) socket.RemoteEndPoint).Address.ToString ();
			this.port = ((IPEndPoint) socket.RemoteEndPoint).Port;
            this.state = SocketState.Open;
		}
		
		class SocketStream : Manos.IO.Stream
		{
			Socket parent;
			bool readAllowed, writeAllowed;
			long readLimit, position;
			byte [] receiveBuffer = new byte [4096];
			System.Timers.Timer readTimer, writeTimer;

			public SocketStream (Socket parent)
			{
				this.parent = parent;
			}

            public override bool Managed
            {
                get { return true; }
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

			public override bool CanTimeout {
				get { return true; }
			}

			public override TimeSpan ReadTimeout {
				get { return readTimer == null ? TimeSpan.Zero : TimeSpan.FromMilliseconds (readTimer.Interval); }
				set {
					if (value < TimeSpan.Zero)
						throw new ArgumentException ("value");
					if (readTimer == null) {
						readTimer = new System.Timers.Timer (value.TotalMilliseconds);
						readTimer.Elapsed += HandleReadTimerElapsed;
					}
					if (value == TimeSpan.Zero) {
						readTimer.Stop ();
					} else {
						readTimer.Interval = value.TotalMilliseconds;
						readTimer.Start ();
					}
				}
			}

			public override TimeSpan WriteTimeout {
				get { return writeTimer == null ? TimeSpan.Zero : TimeSpan.FromMilliseconds (writeTimer.Interval); }
				set {
					if (value < TimeSpan.Zero)
						throw new ArgumentException ("value");
					if (writeTimer == null) {
						writeTimer = new System.Timers.Timer (value.TotalMilliseconds);
						writeTimer.Elapsed += HandleWriteTimerElapsed;
					}
					if (value == TimeSpan.Zero) {
						writeTimer.Stop ();
					} else {
						writeTimer.Interval = value.TotalMilliseconds;
						writeTimer.Start ();
					}
				}
			}

			void HandleReadTimerElapsed (object sender, System.Timers.ElapsedEventArgs e)
			{
				if (readAllowed) {
					RaiseError (new TimeoutException ());
					PauseReading ();
				}
			}

			void HandleWriteTimerElapsed (object sender, System.Timers.ElapsedEventArgs e)
			{
				if (writeAllowed) {
					RaiseError (new TimeoutException ());
					PauseWriting ();
				}
			}

			public override void Write (IEnumerable<ByteBuffer> data)
			{
				base.Write (data);
				ResumeWriting ();
			}

			public override void ResumeReading ()
			{
				ResumeReading (long.MaxValue);
			}

			public override void ResumeReading (long forBytes)
			{
				if (forBytes < 0)
					throw new ArgumentException ("forBytes");
				readLimit = forBytes;
				throw new NotImplementedException ();
			}

			public override void ResumeWriting ()
			{
				writeAllowed = true;
				HandleWrite ();
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

			public override IDisposable Read (Action<ByteBuffer> onData, Action<Exception> onError, Action onClose)
			{
				ResumeReading ();
				return base.Read (onData, onError, onClose);
			}

			protected override void HandleWrite ()
			{
				if (writeAllowed) {
					base.HandleWrite ();
				}
			}

			protected override int WriteSingleBuffer (ByteBuffer buffer)
			{
				SocketError err = SocketError.Success;
				parent.socket.BeginSend (buffer.Bytes, buffer.Position, buffer.Length, SocketFlags.None, out err, ar => {
					if (writeTimer != null) {
						writeTimer.Stop ();
						writeTimer.Start ();
					}
					parent.socket.EndSend (ar);
					if (err != SocketError.Success) {
						parent.loop.NonBlockInvoke (delegate {
							RaiseError (new SocketException ());
						});
					} else {
						parent.loop.NonBlockInvoke (ResumeWriting);
					}
				}, null);
				return buffer.Length;
			}

			void HandleRead ()
			{
				
				SocketError se;
				int length = (int) Math.Min (readLimit, receiveBuffer.Length);
				parent.socket.BeginReceive (receiveBuffer, 0, length, SocketFlags.None, out se, ReadCallback, null);
			}

			void ReadCallback (IAsyncResult ar)
			{
				if (readTimer != null) {
					readTimer.Stop ();
					readTimer.Start ();
				}
				
				SocketError error;
				int len = parent.socket.EndReceive (ar, out error);
				
				if (error != SocketError.Success) {
					parent.loop.NonBlockInvoke (delegate {
						RaiseError (new SocketException ());
					});
				} else if (len == 0) {
					parent.loop.NonBlockInvoke (RaiseEndOfStream);
				} else {
					parent.loop.NonBlockInvoke (delegate {
						RaiseData (new ByteBuffer (receiveBuffer, 0, len));
						HandleRead ();
					});
				}
			}

			protected override void RaiseData (ByteBuffer data)
			{
				position += data.Length;
				base.RaiseData (data);
			}

			public override void Close ()
			{
				if (parent == null) {
					return;
				}
				
				parent.socket.BeginDisconnect (false, ar => {
					try {
						parent.socket.EndDisconnect (ar);
						RaiseEndOfStream ();
						parent = null;
						
						if (readTimer != null) {
							readTimer.Dispose ();
						}
						if (writeTimer != null) {
							writeTimer.Dispose ();
						}
						readTimer = null;
						writeTimer = null;
						
						base.Close ();
					} catch {
					}
				}, null);
				
				base.Close ();
			}
		}
		
		public override Manos.IO.Stream GetSocketStream ()
		{
			if (state != Socket.SocketState.Open)
				throw new InvalidOperationException ();
			
			if (stream == null)
				stream = new SocketStream (this);
			
			return stream;
		}

		public override void Connect (string host, int port, Action callback)
		{
			if (state != Socket.SocketState.Invalid)
				throw new InvalidOperationException ();
			
			address = host;
			this.port = port;
			connectedCallback = callback;
			
			IPAddress addr;
			if (!IPAddress.TryParse (host, out addr)) {
				Dns.BeginGetHostEntry (host, (a) => {
					try {
						IPHostEntry ep = Dns.EndGetHostEntry (a);
						StartConnectingSocket (ep.AddressList [0], port);
					} catch {
					}
				}, null);
			} else {
				StartConnectingSocket (addr, port);
			}
		}

		void StartConnectingSocket (IPAddress addr, int port)
		{
			socket = new System.Net.Sockets.Socket (addr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
			try {
				socket.BeginConnect (addr, port, (ar) => {
					try {
						socket.EndConnect (ar);
						loop.NonBlockInvoke (connectedCallback);
					} catch {
					}
				}, null);
			} catch {
			}
		}

		public override void Listen (string host, int port, Action<Manos.IO.Socket> callback)
		{
			if (state != Socket.SocketState.Invalid)
				throw new InvalidOperationException ();
			
			address = host;
			this.port = port;
			acceptedCallback = callback;
			
			IPAddress addr;
			if (!IPAddress.TryParse (host, out addr)) {
				Dns.BeginGetHostEntry (host, (a) => {
					try {
						IPHostEntry ep = Dns.EndGetHostEntry (a);
						StartListeningSocket (ep.AddressList [0], port);
					} catch {
					}
				}, null);
			} else {
				StartListeningSocket (addr, port);
			}
		}

		void StartListeningSocket (IPAddress addr, int port)
		{
			socket = new System.Net.Sockets.Socket (addr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
			try {
				socket.Bind (new IPEndPoint (addr, port));
				socket.Listen (5);
				socket.BeginAccept (AcceptCallback, null);
			} catch {
			}
		}

		void AcceptCallback (IAsyncResult ar)
		{
			try {
				var sock = socket.EndAccept (ar);

				loop.NonBlockInvoke (delegate {
					acceptedCallback (new Socket (loop, sock));
				});
				socket.BeginAccept (AcceptCallback, null);
			} catch {
			}
		}
	}
}
