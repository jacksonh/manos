
using System;
using System.IO;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Collections;
using System.Collections.Generic;

using Libev;
using Mono.Unix.Native;


namespace Manos.Server {

	public delegate void HttpConnectionCallback (IHttpTransaction transaction);

	public class HttpServer {

		// This gets called on every request so lets just use a hard coded string instead of reflection
		public static readonly string ServerVersion = "Manos/0.0.4";

		private HttpConnectionCallback callback;
		private IOLoop ioloop;
		private IOWatcher iowatcher;

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
			iowatcher = new IOWatcher (Socket.Handle, EventTypes.Read, ioloop.EventLoop, HandleIOEvents);
			iowatcher.Start ();
		}

		private void HandleIOEvents (Loop loop, IOWatcher watcher, int revents)
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


