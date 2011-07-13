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
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using Manos.IO;
using Manos.Collections;

namespace Manos.Http
{
	public class HttpTransaction : IHttpTransaction, IDisposable
	{

		public static HttpTransaction BeginTransaction (HttpServer server, Socket socket, HttpConnectionCallback cb, bool closeOnEnd = false)
		{
			HttpTransaction transaction = new HttpTransaction (server, socket, cb, closeOnEnd);

			return transaction;
		}

		private bool aborted;
		private bool closeOnEnd;
		private bool wantClose, responseFinished;
		private GCHandle gc_handle;
		
		public HttpTransaction (HttpServer server, Socket socket, HttpConnectionCallback callback, bool closeOnEnd = false)
		{
			Server = server;
			Socket = socket;
			this.closeOnEnd = closeOnEnd;
			
			Context = server.Context;
			
			ConnectionCallback = callback;

			gc_handle = GCHandle.Alloc (this);

			Request = new HttpRequest (this, socket);
			Request.Read (Close);
		}

		public void Dispose ()
		{
			if (Socket != null) 
				Socket.Close ();
			
			// Technically the IOStream should call our Close method, but lets be sure
			if (gc_handle.IsAllocated)
				gc_handle.Free ();
		}
		
		public Context Context {
			get;
			private set;
		}

		public HttpServer Server {
			get;
			private set;
		}

		public Socket Socket {
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
			if (!responseFinished) {
				wantClose = true;
			} else {
				if (gc_handle.IsAllocated)
					gc_handle.Free ();

				if (Request != null)
					Request.Dispose ();

				if (Response != null)
					Response.Dispose ();

				Socket = null;
				Request = null;
				Response = null;
			}
		}

		public void Run ()
		{
			ConnectionCallback (this);
		}

		public void OnRequestReady ()
		{
			try {
				Response = new HttpResponse (Context, Request, Socket);
				ResponseReady = true;
				if (closeOnEnd)
					Response.OnEnd += () => Response.Complete (OnResponseFinished);
				Server.RunTransaction (this);
			} catch (Exception e) {
				Console.WriteLine ("Exception while running transaction");
				Console.WriteLine (e);
			}
		}

		public void OnResponseFinished ()
		{
			Socket.GetSocketStream ().Write (ResponseFinishedCallback ());
		}
		
		IEnumerable<ByteBuffer> ResponseFinishedCallback ()
		{
			IBaseWatcher handler = null;
			handler = Server.Context.CreateIdleWatcher (delegate {
				handler.Dispose ();
				responseFinished = true;
				bool disconnect = true;

				if (!NoKeepAlive) {
					string dis;
					if (Request.MinorVersion > 0 && Request.Headers.TryGetValue ("Connection", out dis))
						disconnect = (dis == "close");
				}

				if (disconnect) {
					Socket.Close ();
					if (wantClose) {
						Close ();
					}
				} else {
					responseFinished = false;
					wantClose = false;
					Request.Read (Close);
				}
			});
			handler.Start ();
			yield break;
		}

	}
}

