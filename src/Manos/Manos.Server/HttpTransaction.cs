


using System;
using System.IO;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Collections;
using System.Collections.Generic;

using Mono.Unix.Native;

using Manos.Collections;

namespace Manos.Server {

	public class HttpTransaction : IHttpTransaction {

		public static void BeginTransaction (HttpServer server, IOStream stream, Socket socket, HttpConnectionCallback cb)
		{
			HttpTransaction transaction = new HttpTransaction (server, stream, socket, cb);
		}

		private bool aborted;
		private bool connection_finished;

		private Queue<IWriteOperation> write_ops;
		
		public HttpTransaction (HttpServer server, IOStream stream, Socket socket, HttpConnectionCallback callback)
		{
			Server = server;
			IOStream = stream;
			Socket = socket;
			ConnectionCallback = callback;

			write_ops = new Queue<IWriteOperation> ();
			
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
		
		// Force the server to disconnect
		public bool NoKeepAlive {
			get;
			set;
		}

		public bool ConnectionFinished {
			get {
				return connection_finished;
			}
		}
		
		public void Abort (int status, string message, params object [] p)
		{
			aborted = true;
		}

		public void Write (List<ArraySegment<byte>> data)
		{
			write_ops.Enqueue (new WriteBytesOperation (data, OnWriteFinished));
			PerformNextWrite ();
		}

		public void SendFile (string file)
		{
			write_ops.Enqueue (new WriteFileOperation (file, OnWriteFinished)); 
			PerformNextWrite ();
		}

		public void Finish ()
		{
			//
			// We mark the connection as finished, and FinishResponse
			// will be raised once all the writing is done.
			//
			connection_finished = true;
		}

		public void Run ()
		{
			ConnectionCallback (this);	
		}
		
		private void OnWriteFinished ()
		{
			if (PerformNextWrite ())
				return;
			
			if (ConnectionFinished)
				FinishResponse ();
		}

		private bool NoWritesQueued {
			get { return write_ops.Count < 1; }	
		}
		
		private bool PerformNextWrite ()
		{
			if (NoWritesQueued)
				return false;
			
			if (IOStream.IsWriting)
				return true;
			
			IWriteOperation op = write_ops.Dequeue ();
			op.Write (IOStream);
			
			return true;
		}
		
		private void FinishResponse ()
		{
			bool disconnect = true;

			if (!NoKeepAlive) {
				string dis;
				if (Request.Http_1_1_Supported && Request.Headers.TryGetValue ("Connection", out dis))
					disconnect = (dis == "close");
			}

			Request = null;
			Response = null;

			if (disconnect) {
			      	IOStream.Close ();
				return;
			} else 
				IOStream.DisableWriting ();

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

			stream.DisableReading ();
			Server.IOLoop.QueueTransaction (this);
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
			if (Request.Method == "POST" || Request.Method == "PUT") {
				string ct = Request.Headers ["Content-Type"];
				if (ct != null && ct.StartsWith ("application/x-www-form-urlencoded"))
					OnWwwFormData (data);
				else if (ct != null && ct.StartsWith ("multipart/form-data"))
					OnMultiPartFormData (data);
			}

			IOStream.DisableReading ();
			Server.IOLoop.QueueTransaction (this);
		}

		private void OnWwwFormData (byte [] data)
		{
			//
			// The best I can tell, you can't actually set the content-type of
			// the url-encoded form data.  Looking at the source of apache
			// seems to confirm this.  So for now I wont worry about the
			// encoding type and I'll just use ASCII.
			//

			string post = Encoding.ASCII.GetString (data);

			// TODO: pass this to the encoder to populate
			DataDictionary post_data = HttpUtility.ParseUrlEncodedData (post);
			if (post_data != null)
				Request.SetWwwFormData (post_data);
		}

		private void OnMultiPartFormData (byte [] data)
		{
			throw new NotImplementedException ();
		}
		
		private bool Version_1_1_Supported (string version)
		{
			return version == "HTTP/1.1";
		}
	}
}

