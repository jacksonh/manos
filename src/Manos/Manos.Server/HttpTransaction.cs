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


using Manos.Http;
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

		private HttpParser parser;
		private ParserSettings parser_settings;

		private StringBuilder query_data = new StringBuilder ();
		private StringBuilder current_header_field = new StringBuilder ();
		private StringBuilder current_header_value = new StringBuilder ();

		private IHttpBodyHandler body_handler;

		private Queue<IWriteOperation> write_ops;
		
		public HttpTransaction (HttpServer server, IOStream stream, Socket socket, HttpConnectionCallback callback)
		{
			Server = server;
			IOStream = stream;
			Socket = socket;
			ConnectionCallback = callback;

			write_ops = new Queue<IWriteOperation> ();

			stream.OnClose (OnClose);

			parser_settings = CreateParserSettings ();
			parser = new HttpParser ();

			stream.ReadBytes (OnBytesRead);
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
				if (Request.MinorVersion > 0 && Request.Headers.TryGetValue ("Connection", out dis))
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

			// IOStream.ReadUntil ("\r\n\r\n", OnHeaders);
		}

		private void OnClose (IOStream stream)
		{
		}

		private void OnBytesRead (IOStream stream, byte [] data, int offset, int count)
		{
			ByteBuffer bytes = new ByteBuffer (data, offset, count);

			parser.Execute (parser_settings, bytes);
		}

		private int OnPath (HttpParser parser, ByteBuffer data, int pos, int len)
		{
			string str = Encoding.ASCII.GetString (data.Bytes, pos, len);

			Request.LocalPath = Request.LocalPath == null ? str : String.Concat (Request.LocalPath, str);
			return 0;
		}

		private int OnQueryString (HttpParser parser, ByteBuffer data, int pos, int len)
		{
			string str = Encoding.ASCII.GetString (data.Bytes, pos, len);

			query_data.Append (str);
			return 0;
		}

		private int OnMessageBegin (HttpParser parser)
		{
			Request = new HttpRequest (this);

			return 0;
		}

		private int OnMessageComplete (HttpParser parser)
		{
			OnFinishedReading ();
			return 0;
		}

		public int OnHeaderField (HttpParser parser, ByteBuffer data, int pos, int len)
		{
			string str = Encoding.ASCII.GetString (data.Bytes, pos, len);

			if (current_header_value.Length != 0)
				FinishCurrentHeader ();

			current_header_field.Append (str);
			return 0;
		}

		public int OnHeaderValue (HttpParser parser, ByteBuffer data, int pos, int len)
		{
			string str = Encoding.ASCII.GetString (data.Bytes, pos, len);

			if (current_header_field.Length == 0)
				throw new HttpException ("Header Value raised with no header field set.");

			current_header_value.Append (str);
			return 0;
		}

		private void FinishCurrentHeader ()
		{
			try {
				Request.Headers.SetHeader (current_header_field.ToString (), current_header_value.ToString ());
				current_header_field.Length = 0;
				current_header_value.Length = 0;
			} catch (Exception e) {
				Console.WriteLine (e);
			}
		}

		private int OnHeadersComplete (HttpParser parser)
		{
			if (current_header_field.Length != 0)
				FinishCurrentHeader ();

			if (query_data.Length != 0) {
				Request.QueryData = HttpUtility.ParseUrlEncodedData (query_data.ToString ());
				query_data.Length = 0;
			}

			Request.MajorVersion = parser.Major;
			Request.MinorVersion = parser.Minor;
			Request.Method = parser.HttpMethod;

			return 0;
		}

		public int OnBody (HttpParser parser, ByteBuffer data, int pos, int len)
		{
			if (body_handler == null)
				CreateBodyHandler ();

			if (body_handler != null)
				body_handler.HandleData (this, data, pos, len);

			return 0;
		}

		private void CreateBodyHandler ()
		{
			string ct = Request.Headers ["Content-Type"];

			if (ct != null && ct.StartsWith ("application/x-www-form-urlencoded", StringComparison.InvariantCultureIgnoreCase)) {
				body_handler = new HttpFormDataHandler ();
				return;
			}

			if (ct != null && ct.StartsWith ("multipart/form-data", StringComparison.InvariantCultureIgnoreCase)) {
				string boundary = ParseBoundary (ct);
				IUploadedFileCreator file_creator = GetFileCreator ();

				body_handler = new HttpMultiPartFormDataHandler (boundary, Request.ContentEncoding, file_creator);
				return;
			}
		}

		private IUploadedFileCreator GetFileCreator ()
		{
			if (Request.Headers.ContentLength == null || Request.Headers.ContentLength >= MAX_BUFFERED_CONTENT_LENGTH)
				return new TempFileUploadedFileCreator ();
			return new InMemoryUploadedFileCreator ();
		}

		private void OnFinishedReading ()
		{
			if (body_handler != null)
				body_handler.Finish (this);

			try {
				IOStream.DisableReading ();
				Response = new HttpResponse (this, Encoding.Default);

				Server.RunTransaction (this);
			} catch (Exception e) {
				Console.WriteLine ("Exception while running transaction");
				Console.WriteLine (e);
			}
		}

		private ParserSettings CreateParserSettings ()
		{
			ParserSettings settings = new ParserSettings ();

			settings.OnError = OnParserError;

			settings.OnPath = OnPath;
			settings.OnQueryString = OnQueryString;

			settings.OnMessageBegin = OnMessageBegin;
			settings.OnMessageComplete = OnMessageComplete;

			settings.OnHeaderField = OnHeaderField;
			settings.OnHeaderValue = OnHeaderValue;
			settings.OnHeadersComplete = OnHeadersComplete;
			
			settings.OnBody = OnBody;

			return settings;
		}

		private void OnParserError (HttpParser parser, string message, ByteBuffer buffer, int initial_position)
		{
			Console.WriteLine ("parser error: '{0}'", message);
			Server.RemoveTransaction (this);
			IOStream.Close ();
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

