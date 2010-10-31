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


using Manos.Collections;

namespace Manos.Server {

	public class HttpTransaction : IHttpTransaction {

	        private static readonly long MAX_BUFFERED_CONTENT_LENGTH = 2621440; // 2.5MB (Eventually this will be an environment var)

		public static HttpTransaction BeginTransaction (HttpServer server, IOStream stream, Socket socket, HttpConnectionCallback cb)
		{
			HttpTransaction transaction = new HttpTransaction (server, stream, socket, cb);

			return transaction;
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

			stream.OnClose (OnClose);
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

			if (disconnect) {
				Request = null;
				Response = null;
			      	IOStream.Close ();
				Server.RemoveTransaction (this);
				return;
			} else 
				IOStream.DisableWriting ();

			IOStream.ReadUntil ("\r\n\r\n", OnHeaders);
		}

		private void OnClose (IOStream stream)
		{
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
			        long cl = (long) headers.ContentLength;
				if (cl < MAX_BUFFERED_CONTENT_LENGTH) {
					HandleBody ();
					return;
				} else {
					HandleLargeBody ();
					return;
				}
			}

			stream.DisableReading ();
			Server.RunTransaction (this);
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

			if (!version.StartsWith ("HTTP/", StringComparison.InvariantCulture))
				throw new Exception ("Malformed HTTP request, no version specified.");
		}

		private void HandleLargeBody ()
		{
			if (Request.Method != "POST" && Request.Method != "PUT")
				throw new InvalidOperationException ("Large Request bodies are only allowed with PUT or POST operations.");

			string ct = Request.Headers ["Content-Type"];
			if (ct == null || !ct.StartsWith ("multipart/form-data", StringComparison.InvariantCultureIgnoreCase)) {
				// TODO: Maybe someone wants to post large www-form-urlencoded data?
				throw new InvalidOperationException ("Large Request bodies are only allowed with multipart form data.");
			}

			string boundary = ParseBoundary (ct);
			IMFDStream stream = new TempFileMFDStream (IOStream);

			MultipartFormDataParser parser = new MultipartFormDataParser (this.Request, boundary, stream, () => {
				IOStream.DisableReading ();
				Server.RunTransaction (this);
			});

			parser.ParseParts ();
		}

		private void HandleBody ()
		{
			if (Request.Method == "POST" || Request.Method == "PUT") {
				string ct = Request.Headers ["Content-Type"];
				if (ct != null && ct.StartsWith ("application/x-www-form-urlencoded", StringComparison.InvariantCultureIgnoreCase))
					IOStream.ReadBytes ((int) Request.Headers.ContentLength, OnWwwFormData);
				else if (ct != null && ct.StartsWith ("multipart/form-data", StringComparison.InvariantCulture)) {
				        string boundary = ParseBoundary (ct);
					if (boundary == null)
			   		        throw new InvalidOperationException ("Invalid content type boundary.");

					IOStream.ReadBytes ((int) Request.Headers.ContentLength, (ios, data) => {
						OnMultiPartFormData (boundary, data);
					});
				}
			}
		}

		private void OnWwwFormData (IOStream stream, byte [] data)
		{
			//
			// The best I can tell, you can't actually set the content-type of
			// the url-encoded form data.  Looking at the source of apache
			// seems to confirm this.  So for now I wont worry about the
			// encoding type and I'll just use ASCII.
			//

			string post = Encoding.ASCII.GetString (data);

			if (!String.IsNullOrEmpty (post)) {
				// TODO: pass this to the encoder to populate
				DataDictionary post_data = HttpUtility.ParseUrlEncodedData (post);
				if (post_data != null)
					Request.SetWwwFormData (post_data);
			}

			IOStream.DisableReading ();
			Server.RunTransaction (this);
		}

		private void OnMultiPartFormData (string boundary, byte [] data)
		{
			IMFDStream stream = new InMemoryMFDStream (data);
			MultipartFormDataParser parser = new MultipartFormDataParser (this.Request, boundary, stream, () => {
				IOStream.DisableReading ();
				Server.RunTransaction (this);
			});

			parser.ParseParts ();
		}
		
		private bool Version_1_1_Supported (string version)
		{
			return version == "HTTP/1.1";
		}

		public static string ParseBoundary (string ct)
		{
			if (ct == null)
				return null;

			int start = ct.IndexOf ("boundary=");
			if (start < 1)
				return null;
			
			return ct.Substring (start + "boundary=".Length);
		}
	}
}

