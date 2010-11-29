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
using System.IO;
using System.Text;
using System.Net;
using System.Linq;
using System.Reflection;
using System.Net.Sockets;
using System.Collections;
using System.Collections.Generic;

using Libev;

using Manos.IO;

namespace Manos.Http {

	public delegate void HttpConnectionCallback (IHttpTransaction transaction);

	public class HttpServer: IDisposable {

		// This gets called on every request so lets just use a hard coded string instead of reflection
		public static readonly string ServerVersion;

		private HttpConnectionCallback callback;
		private IOLoop ioloop;
		private IOWatcher iowatcher;
		private IntPtr handle;

		private List<HttpTransaction> transactions = new List<HttpTransaction> ();

		static HttpServer ()
		{
			Version v = Assembly.GetExecutingAssembly ().GetName ().Version;
			ServerVersion = "Manos/" + v.ToString ();
		}

		public HttpServer (HttpConnectionCallback callback, IOLoop ioloop)
		{
			this.callback = callback;
			this.ioloop = ioloop;


			AppHost.AddTimeout (TimeSpan.FromMinutes (2), RepeatBehavior.Forever, null, ExpireTransactions);
		}

		public IOLoop IOLoop {
			get { return ioloop; }
		}

		public Socket Socket {
			get;
			private set;
		}

		public List<HttpTransaction> Transactions {
			get { return transactions; }
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
			handle = IOWatcher.GetHandle (Socket);
			iowatcher = new IOWatcher (handle, EventTypes.Read, ioloop.EventLoop, HandleIOEvents);
			iowatcher.Start ();
		}

		public void Dispose () 
		{
			IOWatcher.ReleaseHandle(Socket, handle);
		}

		public void RunTransaction (HttpTransaction trans)
		{
			trans.Run ();
		}

		public void RemoveTransaction (HttpTransaction trans)
		{
			transactions.Remove (trans);
		}

		private void HandleIOEvents (Loop loop, IOWatcher watcher, EventTypes revents)
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
				transactions.Add (HttpTransaction.BeginTransaction (this, iostream, s, callback));
			}
		}

		private void ExpireTransactions (ManosApp app, object data)
		{
			DateTime now = DateTime.UtcNow;
			int count = transactions.Count ();
			transactions.RemoveAll (t => t.IOStream.Expires <= now);
		}
	}
}


