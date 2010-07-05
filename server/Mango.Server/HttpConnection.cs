


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

		public static void HandleConnection (HttpServer server, IOStream stream, Socket socket, HttpConnectionCallback cb)
		{
			HttpConnection connection = new HttpConnection (server, stream, socket, cb);
		}

		private bool aborted;
		private bool connection_finished;

		public HttpConnection (HttpServer server, IOStream stream, Socket socket, HttpConnectionCallback callback)
		{
			Server = server;
			IOStream = stream;
			Socket = socket;
			ConnectionCallback = callback;

			stream.ReadUntil ("\r\n\r\n", OnHeaders);
		}

		public HttpServer Server {
			get;
			private set;
		}

		public IOStream IOStream {
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

		public HttpRequest Request {
			get;
			private set;
		}

		public HttpResponse Response {
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

		internal void Write (List<ArraySegment<byte>> data)
		{
			IOStream.Write (data, OnWriteFinished);
		}

		internal void SendFile (string file)
		{
			IOStream.SendFile (file);
		}

		internal void Finish ()
		{
			connection_finished = true;

			if (!IOStream.IsWriting)
				FinishResponse ();
		}

		private void OnWriteFinished ()
		{
			if (connection_finished)
				FinishResponse ();
		}

		private void FinishResponse ()
		{
			bool disconnect = true;

			if (!NoKeepAlive) {
				if (Request.Http_1_1_Supported)
					disconnect = Request.Headers ["Connection"] == "close";
			}

			Request = null;
			Response = null;

			if (disconnect) {
				IOStream.Close ();
				return;
			}

			IOStream.ReadUntil ("\r\n\r\n", OnHeaders);
		}

		private void OnHeaders (IOStream stream, byte [] data)
		{
			string h = Encoding.ASCII.GetString (data);
			StringReader reader = new StringReader (h);

			string verb;
			string path;
			string version;

			string line = reader.ReadLine ();
			ParseStartLine (line, out verb, out path, out version);
			
			HttpHeaders headers = new HttpHeaders ();
			headers.Parse (reader);

			Request = new HttpRequest (this, headers, verb, path, Version_1_1_Supported (version));
			Response = new HttpResponse (this, Encoding.ASCII);
			
			if (headers.ContentLength != null && headers.ContentLength > 0) {
				stream.ReadBytes ((int) headers.ContentLength, OnBody);
				return;
			}

			ConnectionCallback (this);
		}

		private void ParseStartLine (string line, out string verb, out string path, out string version)
		{
			int s = 0;
			int e = line.IndexOf (' ');

			verb = line.Substring (s, e);

			s = e + 1;
			e = line.IndexOf (' ', s);
			path = line.Substring (s, e - s);

			s = e + 1;
			version = line.Substring (s);

			if (!version.StartsWith ("HTTP/"))
				throw new Exception ("Malformed HTTP request, no version specified.");
		}

		private void OnBody (IOStream stream, byte [] data)
		{
			if (Request.Method == "POST") {
				string ct = Request.Headers ["Content-Type"];
				if (ct == "application/x-www-form-urlencoded")
					Request.SetWwwFormData (data);
				else if (ct == "multipart/form-data")
					Request.SetMultiPartFormData (data);
			}

			ConnectionCallback (this);
		}

		private bool Version_1_1_Supported (string version)
		{
			return version == "HTTP/1.1";
		}
	}
}

