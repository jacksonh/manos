
using System;
using System.IO;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Collections;
using System.Collections.Generic;

using Mono.Unix.Native;


namespace Mango.Server {

	public delegate void HttpConnectionCallback (IHttpTransaction transaction);

	public class HttpServer {

		// This gets called on every request so lets just use a hard coded string instead of reflection
		public static readonly string ServerVersion = "0.0.0.1";

		private HttpConnectionCallback callback;

		public HttpServer (HttpConnectionCallback callback)
		{
			this.callback = callback;
		}

		public IOLoop IOLoop {
			get;
			private set;
		}

		public Socket Socket {
			get;
			private set;
		}

		public void Bind (int port)
		{
			Socket = new Socket (AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			Socket.SetSocketOption (SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
			Socket.Blocking = false;
			Socket.Bind (new IPEndPoint (IPAddress.Parse ("0.0.0.0"), port));
			Socket.Listen (128);
		}

		public void Start ()
		{
			Start (1);
		}

		public void Start (int num_process)
		{
			// For now only start a single process
			IOLoop = new IOLoop ();
			IOLoop.AddHandler (Socket.Handle, HandleEvents, IOLoop.EPOLL_READ_EVENTS);
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


