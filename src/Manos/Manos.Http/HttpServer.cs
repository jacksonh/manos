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

		public static readonly string ServerVersion;

		private HttpConnectionCallback callback;
		SocketStream socket;
		private bool closeOnEnd;
		
		static HttpServer ()
		{
			Version v = Assembly.GetExecutingAssembly ().GetName ().Version;
			ServerVersion = "Manos/" + v.ToString ();
		}

		public HttpServer (HttpConnectionCallback callback, SocketStream socket, bool closeOnEnd = false)
		{
			this.callback = callback;
			this.socket = socket;
			this.closeOnEnd = closeOnEnd;
		}

		public IOLoop IOLoop {
			get { return socket.IOLoop; }
		}

		public void Listen (string host, int port)
		{
			socket.Listen (host, port);
			socket.ConnectionAccepted += ConnectionAccepted;
		}

		public void Dispose () 
		{
			if (socket != null) {
				socket.Dispose ();
				socket = null;
			}
		}

		public void RunTransaction (HttpTransaction trans)
		{
			trans.Run ();
		}

		private void ConnectionAccepted (object sender, ConnectionAcceptedEventArgs args)
		{
			var t = HttpTransaction.BeginTransaction (this, args.Stream, callback, closeOnEnd);
		}
	}
}


