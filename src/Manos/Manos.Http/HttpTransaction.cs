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
using System.Net.Sockets;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using Manos.IO;
using Manos.Collections;

namespace Manos.Http {

	public class HttpTransaction : IHttpTransaction, IDisposable {

		public static HttpTransaction BeginTransaction (HttpServer server, SocketStream stream, HttpConnectionCallback cb, bool closeOnEnd = false)
		{
			HttpTransaction transaction = new HttpTransaction (server, stream, cb, closeOnEnd);

			return transaction;
		}

		private bool aborted;
        private bool closeOnEnd;
		
		private GCHandle gc_handle;
		
		public HttpTransaction (HttpServer server, SocketStream stream, HttpConnectionCallback callback, bool closeOnEnd = false)
		{
			Server = server;
			Stream = stream;
			this.closeOnEnd = closeOnEnd;
			
			ConnectionCallback = callback;

			gc_handle = GCHandle.Alloc (this);

			Stream.Closed += delegate (object sender, EventArgs args) {
				Close ();
			};

			Request = new HttpRequest (this, stream);
			Request.Read ();
		}

		public void Dispose ()
		{
			if (Stream != null) 
				Stream.Close ();
			
			// Technically the IOStream should call our Close method, but lets be sure
			if (gc_handle.IsAllocated)
				gc_handle.Free ();
		}

		public HttpServer Server {
			get;
			private set;
		}

		public SocketStream Stream {
			get;
			private set;
		}

		public  HttpConnectionCallback ConnectionCallback {
			get;
			private set;
		}

		public IHttpRequest Request {
			get;
			private set;
		}

		public IHttpResponse Response {
			get;
			private set;
		}

		public bool Aborted {
			get { return aborted; }	
		}

		public bool ResponseReady {
			get;
			private set;
		}

		// Force the server to disconnect
		public bool NoKeepAlive {
			get;
			set;
		}
		
		public void Abort (int status, string message, params object [] p)
		{
			aborted = true;
		}

		public void Close ()
		{
			if (gc_handle.IsAllocated)
				gc_handle.Free ();

			if (Request != null)
				Request.Dispose ();

			if (Response != null)
				Response.Dispose ();

			Stream = null;
			Request = null;
			Response = null;
		}

		public void Run ()
		{
			ConnectionCallback (this);
		}

		public void OnRequestReady ()
		{
			try {
				Response = new HttpResponse (Request, Stream);
				ResponseReady = true;
				if( closeOnEnd ) Response.OnEnd += () => Response.Complete( OnResponseFinished );
				Server.RunTransaction (this);
			} catch (Exception e) {
				Console.WriteLine ("Exception while running transaction");
				Console.WriteLine (e);
			}
		}

		public void OnResponseFinished ()
		{
			bool disconnect = true;

			if (!NoKeepAlive) {
				string dis;
				if (Request.MinorVersion > 0 && Request.Headers.TryGetValue ("Connection", out dis))
					disconnect = (dis == "close");
			}

			if (disconnect) {
				if (Request != null) {
					Request.Dispose ();
					Request = null;
				}
				if (Response != null) {
					Response.Dispose ();
					Response = null;
				}
			      	Stream.Close ();
				return;
			} else
				Request.Read ();
		}

	}
}

