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
		}
		
		class SocketStream : Manos.IO.Stream
		{
			Socket parent;
			bool readAllowed, writeAllowed;
			long readLimit;
			byte [] receiveBuffer = new byte [4096];
			// write queue
			IEnumerator<ByteBuffer> currentWrite;
			Queue<IEnumerable<ByteBuffer>> writeQueue;

			public SocketStream (Socket parent)
			{
				this.parent = parent;
				writeQueue = new Queue<IEnumerable<ByteBuffer>> ();
			}

			public override void Write (IEnumerable<ByteBuffer> data)
			{
				if (data == null)
					throw new ArgumentNullException ("data");
				writeQueue.Enqueue (data);
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

			void WriteSingleBuffer (ByteBuffer buffer)
			{
				SocketError err;
				parent.socket.BeginSend (buffer.Bytes, buffer.Position, buffer.Length, SocketFlags.None, out err, ar => {
					parent.socket.EndSend (ar);
					if (err != SocketError.Success) {
						parent.loop.NonBlockInvoke (delegate {
							RaiseError (new SocketException ());
						});
					} else if (ar.CompletedSynchronously) {
						parent.loop.NonBlockInvoke (ResumeWriting);
					} else {
						ResumeWriting ();
					}
				}, null);
			}

			void HandleWrite ()
			{
				if (!writeAllowed) {
					return;
				}
				
				if (currentWrite == null) {
					if (writeQueue.Count > 0) {
						currentWrite = writeQueue.Dequeue ().GetEnumerator ();
						HandleWrite ();
					} else {
						PauseWriting ();
					}
				} else {
					if (currentWrite.MoveNext ()) {
						WriteSingleBuffer (currentWrite.Current);
						PauseWriting ();
					} else {
						currentWrite.Dispose ();
						currentWrite = null;
						HandleWrite ();
					}
				}
			}

			void HandleRead ()
			{
				if (!readAllowed) {
					return;
				}
				
				SocketError se;
				int length = (int) Math.Min (readLimit, receiveBuffer.Length);
				parent.socket.BeginReceive (receiveBuffer, 0, length, SocketFlags.None, out se, ReadCallback, null);
			}

			void ReadCallback (IAsyncResult ar)
			{
				SocketError error;
				int len = parent.socket.EndReceive (ar, out error);
				
				if (error != SocketError.Success) {
					parent.loop.NonBlockInvoke (delegate {
						RaiseError (new SocketException ());
					});
				} else if (len == 0) {
					parent.loop.NonBlockInvoke (RaiseClose);
				} else {
					parent.loop.NonBlockInvoke (delegate {
						RaiseData (new ByteBuffer (receiveBuffer, 0, len));
						HandleRead ();
					});
				}
			}

			public void Close ()
			{
				if (parent == null) {
					return;
				}
				
				parent.socket.BeginDisconnect (false, ar => {
					try {
						parent.socket.EndDisconnect (ar);
						RaiseClose ();
						parent = null;
						base.Close ();
					} catch {
					}
				}, null);
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
