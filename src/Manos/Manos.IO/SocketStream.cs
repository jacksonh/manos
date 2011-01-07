//
// Copyright (C) 2010 Jackson Harper (jackson@manosdemono.com)
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//
//


using System;
using System.Net;
using System.Net.Sockets;
using System.Collections;
using System.Collections.Generic;

using Libev;


namespace Manos.IO {

	public class SocketStream : IOStream, IDisposable {

		public enum SocketState {
			None,

			AcceptingConnections,
			Open,
		}

		private SocketState state;
		internal Socket socket;

		public SocketStream (IOLoop ioloop) : this (null, ioloop)
		{
		}

		public SocketStream (Socket socket, IOLoop ioloop) : base (ioloop)
		{
			this.socket = socket;

			if (socket != null) {
				socket.Blocking = false;
				SetHandle (IOWatcher.GetHandle (socket));
				state = SocketState.Open;
			}
		}

		public string Address {
			get {
				if (state == SocketState.None)
					return null;
				int port;
				return GetAddress (out port);
			}
		}

		public int Port {
			get {
				if (state == SocketState.None)
					return -1;
				int port;
				GetAddress (out port);
				return port;
			}
		}

		//
		// Eventually everything will be stored as IPAddress only, since
		// DNS will be handled by our special DNS code
		//
		private string GetAddress (out int port)
		{
			EndPoint end = null;
			switch (state) {
			case SocketState.Open:
				end = socket.RemoteEndPoint;
				break;
			case SocketState.AcceptingConnections:
				end = socket.LocalEndPoint;
				break;
			}

			IPEndPoint ip = end as IPEndPoint;
			if (ip != null) {
				port = ip.Port;
				return ip.Address.ToString ();
			}

			DnsEndPoint dns  = end as DnsEndPoint;
			if (dns != null) {
				port = dns.Port;
				return dns.Host;
			}

			port = -1;
			return null;

		}

		public void Dispose ()
		{
			Close ();
		}

		public override void Close ()
		{
			IOWatcher.ReleaseHandle (socket, Handle);

			base.Close ();
		}

		public void Connect (string host, int port)
		{
			socket = CreateSocket ();
			socket.Connect (host, port);

			IntPtr handle = IOWatcher.GetHandle (socket);
			IOWatcher iowatcher = new IOWatcher (handle, EventTypes.Write, IOLoop.EventLoop, (l, w, r) => {
				w.Stop ();
				OnConnected ();
			});
			iowatcher.Start ();
		}

		public void Connect (int port)
		{
			Connect ("127.0.0.1", port);
		}

		public void Listen (IPEndPoint endpoint)
		{
			socket = CreateSocket ();

			socket.Bind (endpoint);
			socket.Listen (128);

			SetHandle (IOWatcher.GetHandle (socket));

			DisableTimeout ();
			EnableReading ();
			state = SocketState.AcceptingConnections;
		}

		public void Listen (string host, int port)
		{
			// TODO: Proper DNS lookups
			IPEndPoint endpoint = new IPEndPoint (IPAddress.Parse (host), port);

			Listen (endpoint);
		}	

		public void Write (byte [] data, WriteCallback callback)
		{
			Write (data, 0, data.Length, callback);
		}

		public void Write (byte [] data, int offset, int count, WriteCallback callback)
		{
			var bytes = new List<ArraySegment<byte>> ();
			bytes.Add (new ArraySegment<byte> (data, offset, count));

			var write_bytes = new SendBytesOperation (bytes, callback);
			QueueWriteOperation (write_bytes);
		}
		
		private void OnConnected ()
		{
			SetHandle (IOWatcher.GetHandle (socket));
			state = SocketState.Open;

			if (Connected != null)
				Connected (this);
		}

		private Socket CreateSocket ()
		{
			Socket socket = new Socket (AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			socket.SetSocketOption (SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
			socket.Blocking = false;

			return socket;
		}

		protected override void HandleRead ()
		{
			if (state == SocketState.AcceptingConnections) {
				AcceptConnections ();
				return;
			}

			if (state == SocketState.Open) {
				Read ();
				return;
			}
		}

		private void AcceptConnections ()
		{
			while (true) {
			      	Socket s = null;
				try {
					s = socket.Accept ();
				} catch (SocketException se) {
					if (se.SocketErrorCode == SocketError.WouldBlock || se.SocketErrorCode == SocketError.TryAgain)
						return;
					Console.WriteLine ("Socket exception in Accept handler");
					Console.WriteLine (se);
					return;
				} catch (Exception e) {
					Console.WriteLine ("Exception in Accept handler");
					Console.WriteLine (e);
					return;
				}

				SocketStream iostream = new SocketStream (s, IOLoop);
				OnConnectionAccepted (iostream);
			}
		}

		private void Read ()
		{
			int size;

			try {
				size = socket.Receive (ReadChunk);
			} catch (SocketException se) {
				if (se.SocketErrorCode == SocketError.WouldBlock || se.SocketErrorCode == SocketError.TryAgain)
					return;
				Close ();
				return;
			} catch (Exception e) {
			  	Console.WriteLine (e);
				Close ();
				return;
			}

			if (size == 0) {
				read_callback (this, ReadChunk, 0, 0);
				Close ();
				return;
			}

			read_callback (this, ReadChunk, 0, size);
		}

		private void OnConnectionAccepted (SocketStream stream)
		{
			if (ConnectionAccepted != null)
				ConnectionAccepted (this, new ConnectionAcceptedEventArgs (stream));
		}

		public event Action<SocketStream> Connected;
		public event EventHandler<ConnectionAcceptedEventArgs> ConnectionAccepted;
	}
}

