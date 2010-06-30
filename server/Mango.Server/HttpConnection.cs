


using System;
using System.IO;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Collections;
using System.Collections.Generic;

using Mono.Unix.Native;


namespace Mango.Server {

	public class HttpConnection {

		public static void HandleConnection (IOStream stream, Socket socket, HttpRequestCallback callback)
		{
			HttpConnection connection = new HttpConnection (stream, socket, callback);
		}

		public HttpConnection (IOStream stream, Socket socket, HttpRequestCallback callback)
		{
			IOStream = stream;
			Socket = socket;
			HttpRequestCallback = callback;

			stream.ReadUntil ("\r\n\r\n", OnHeaders);
		}

		public IOStream IOStream {
			get;
			private set;
		}

		public Socket Socket {
			get;
			private set;
		}

		public  HttpRequestCallback HttpRequestCallback {
			get;
			private set;
		}

		private void OnHeaders (IOStream stream, byte [] headers)
		{
			Console.WriteLine ("HEADERS:");
			Console.WriteLine ("=========");
			Console.WriteLine (Encoding.ASCII.GetString (headers));
			Console.WriteLine ("=========");
			
		}
	}

}

