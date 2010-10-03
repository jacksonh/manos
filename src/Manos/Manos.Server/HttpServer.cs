
using System;
using System.IO;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Collections;
using System.Collections.Generic;

using Mono.Unix.Native;


namespace Manos.Server {

	public delegate void HttpConnectionCallback (IHttpTransaction transaction);

	public class HttpServer {

		// This gets called on every request so lets just use a hard coded string instead of reflection
		public static readonly string ServerVersion = "0.0.3";

		private HttpConnectionCallback callback;
		private IOLoop ioloop;

		public HttpServer (HttpConnectionCallback callback, IOLoop ioloop)
		{
			this.callback = callback;
			this.ioloop = ioloop;
		}

		public IOLoop IOLoop {
			get { return ioloop; }
		}

		public Socket Socket {
			get;
			private set;
		}

		public void Bind (IPEndPoint endpoint)
		{
			Socket = new Socket (AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			Socket.SetSocketOption (SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
			Socket.Blocking = false;
			Socket.Bind (endpoint);
			Socket.Listen (128);
		}

		public void Start ()
		{
			ioloop.AddHandler (Socket.Handle, HandleEvents, IOLoop.EPOLL_READ_EVENTS);
		}

		private void HandleEvents (IntPtr fd, EpollEvents events)
		{
			while (true) {
				Socket s = null;
				try {
					s = Socket.Accept ();
				} catch (SocketException se) {
					if (se.SocketErrorCode == SocketError.WouldBlock || se.SocketErrorCode == SocketError.TryAgain)
						return;
					throw se;
				} catch {
					throw;
				}

				IOStream iostream = new IOStream (s, IOLoop);
				HttpTransaction.BeginTransaction (this, iostream, s, callback);
			}
		}
	}
}


