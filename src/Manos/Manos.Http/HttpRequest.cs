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
using System.Collections.Specialized;


using Libev;
using Manos.IO;
using Manos.Collections;

namespace Manos.Http {

	public class HttpRequest : IHttpRequest {
		
	        private static readonly long MAX_BUFFERED_CONTENT_LENGTH = 2621440; // 2.5MB (Eventually this will be an environment var)

		private HttpParser parser;
		private ParserSettings parser_settings;

		private StringBuilder query_data_builder = new StringBuilder ();
		private StringBuilder current_header_field = new StringBuilder ();
		private StringBuilder current_header_value = new StringBuilder ();

		private IHttpBodyHandler body_handler;
		
		private HttpHeaders headers;

		private DataDictionary data;
		private DataDictionary uri_data;
		private	DataDictionary query_data;
		private DataDictionary post_data;

		private DataDictionary cookies;
		private Dictionary<string,UploadedFile> uploaded_files;

		public HttpRequest ()
		{
		}

		public HttpRequest (IHttpTransaction transaction, IOStream stream)
		{
			Transaction = transaction;
			IOStream = stream;

			stream.OnClose (OnClose);

			parser_settings = CreateParserSettings ();
		}

		public IHttpTransaction Transaction {
			get;
			private set;
		}

		public IOStream IOStream {
			get;
			private set;
		}

		public HttpHeaders Headers {
			get {
				if (headers == null)
					headers = new HttpHeaders ();
				return headers;
			}
			set {
				headers = value;
			}
		}

		public HttpMethod Method {
			get;
			set;
		}

		public int MajorVersion {
			get;
			set;
		}

		public int MinorVersion {
			get;
			set;
		}

		public string LocalPath {
			get;
			set;
		}

		public Encoding ContentEncoding {
			get { return Headers.ContentEncoding; }
			set { Headers.ContentEncoding = value; }
		}

		public DataDictionary Data {
			get {
				if (data == null)
					data = new DataDictionary ();
				return data;
			}
		}
		
		public DataDictionary PostData {
			get {
				if (post_data == null) {
					post_data = new DataDictionary ();
					Data.Children.Add (post_data);
				}
				return post_data;
			}
			set {
				SetDataDictionary (post_data, value);
				post_data = value;
			}
		}

		public DataDictionary QueryData {
			get {
				if (query_data == null) {
					query_data = new DataDictionary ();
					Data.Children.Add (query_data);
				}
				return query_data;
			}
			set {
				SetDataDictionary (query_data, value);
				query_data = value;
			}
		}

		public DataDictionary UriData {
			get {
				if (uri_data == null) {
					uri_data = new DataDictionary ();
					Data.Children.Add (uri_data);
				}
				return uri_data;
			}
			set {
				SetDataDictionary (uri_data, value);
				uri_data = value;
			}
		}
		
		public DataDictionary Cookies {
			get {
				if (cookies == null)
					cookies = ParseCookies ();
				return cookies;
			}
		}
		
		public Dictionary<string,UploadedFile> Files {
			get {
			    if (uploaded_files == null)
			       uploaded_files = new Dictionary<string,UploadedFile> ();
			    return uploaded_files;
			}
		}

		private DataDictionary ParseCookies ()
		{
			string cookie_header;

			if (!Headers.TryGetValue ("Cookie", out cookie_header))
				return new DataDictionary ();
			
			return HttpCookie.FromHeader (cookie_header);
		}

		public void Reset ()
		{
			LocalPath = null;
			ContentEncoding = null;

			headers = null;
			data = null;
			uri_data = null;
			query_data = null;
			post_data = null;

			cookies = null;
			uploaded_files = null;

			parser = new HttpParser ();
		}

		public void Read ()
		{
			Reset ();
			IOStream.ReadBytes (OnBytesRead);
		}

		public void SetWwwFormData (DataDictionary data)
		{
			PostData = data;
		}

		private void SetDataDictionary (DataDictionary old, DataDictionary newd)
		{
			if (data != null && old != null)
				data.Children.Remove (old);
			if (newd != null)
				Data.Children.Add (newd);
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

			str = HttpUtility.UrlDecode (str, Encoding.ASCII);
			LocalPath = LocalPath == null ? str : String.Concat (LocalPath, str);
			return 0;
		}

		private int OnQueryString (HttpParser parser, ByteBuffer data, int pos, int len)
		{
			string str = Encoding.ASCII.GetString (data.Bytes, pos, len);

			query_data_builder.Append (str);
			return 0;
		}

		private int OnMessageBegin (HttpParser parser)
		{

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
				Headers.SetHeader (current_header_field.ToString (), current_header_value.ToString ());
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

			if (query_data_builder.Length != 0) {
				QueryData = HttpUtility.ParseUrlEncodedData (query_data_builder.ToString ());
				query_data_builder.Length = 0;
			}

			MajorVersion = parser.Major;
			MinorVersion = parser.Minor;
			Method = parser.HttpMethod;

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
			string ct = Headers ["Content-Type"];

			if (ct != null && ct.StartsWith ("application/x-www-form-urlencoded", StringComparison.InvariantCultureIgnoreCase)) {
				body_handler = new HttpFormDataHandler ();
				return;
			}

			if (ct != null && ct.StartsWith ("multipart/form-data", StringComparison.InvariantCultureIgnoreCase)) {
				string boundary = ParseBoundary (ct);
				IUploadedFileCreator file_creator = GetFileCreator ();

				body_handler = new HttpMultiPartFormDataHandler (boundary, ContentEncoding, file_creator);
				return;
			}
		}

		private IUploadedFileCreator GetFileCreator ()
		{
			if (Headers.ContentLength == null || Headers.ContentLength >= MAX_BUFFERED_CONTENT_LENGTH)
				return new TempFileUploadedFileCreator ();
			return new InMemoryUploadedFileCreator ();
		}

		private void OnFinishedReading ()
		{
			if (body_handler != null) {
				body_handler.Finish (this);
				body_handler = null;
			}

			Transaction.OnRequestReady ();
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
			Transaction.Abort (-1, "HttpParser error: {0}", message);
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

		/*
		
		private HttpResponseCallback callback;
		
		public void Get (string url, HttpResponseCallback callback)
		{
			this.callback = callback;
			
			Socket socket = new Socket (AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			socket.SetSocketOption (SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
			socket.Blocking = false;
			socket.Connect (url, 80);

			IntPtr handle = IOWatcher.GetHandle (socket);
			IOWatcher iowatcher = new IOWatcher (handle, EventTypes.Write, AppHost.IOLoop.EventLoop, (l, w, r) => {
				DoGet (socket, url);
				w.Stop ();
			});
			iowatcher.Start ();
		}

		public void DoGet (Socket socket, string url)
		{
			Console.WriteLine ("doing the get");
			IOStream iostream = new IOStream (socket, AppHost.IOLoop);

			byte [] bytes = Encoding.ASCII.GetBytes ("GET " + url + "\r\n\r\n");
			var data = new List<ArraySegment<byte>> ();
			data.Add (new ArraySegment<byte> (bytes));

			WriteBytesOperation write_bytes = new WriteBytesOperation (data, () => {
				Console.WriteLine ("the bytes have been writen");
				iostream.ReadBytes (OnBytesRead);
			});
			iostream.QueueWriteOperation (write_bytes);
		}

		private void OnBytesRead (IOStream stream, byte [] data, int offset, int count)
		{
			HttpResponse response = new HttpResponse ();
			response.Body = data;

			callback (response);
			stream.DisableReading ();
		}

		*/
	}
}

