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
using System.Text;
using System.Globalization;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;


using Libev;
using Manos.IO;
using Manos.Collections;

namespace Manos.Http {

	public class HttpRequest : HttpEntity, IHttpRequest {
		
		private StringBuilder query_data_builder = new StringBuilder ();

		private DataDictionary uri_data;
		private	DataDictionary query_data;
		private DataDictionary cookies;

		
		public HttpRequest (string address)
		{
			Uri uri = null;

			if (!Uri.TryCreate (address, UriKind.Absolute, out uri))
				throw new Exception ("Invalid URI: '" + address + "'.");

			RemoteAddress = uri.Host;
			RemotePort = uri.Port;
			Path = uri.AbsolutePath;

			Method = HttpMethod.HTTP_GET;
			MajorVersion = 1;
			MinorVersion = 1;
		}

		public HttpRequest (string remote_address, int port) : this (remote_address)
		{
			RemotePort = port;
		}

		public HttpRequest (IHttpTransaction transaction, Socket stream)
		{
			Transaction = transaction;
			Socket = stream;
			RemoteAddress = stream.Address;
			RemotePort = stream.Port;
		}

		public IHttpTransaction Transaction {
			get;
			private set;
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

		public override void Reset ()
		{
			Path = null;

			uri_data = null;
			query_data = null;
			cookies = null;

			base.Reset ();
		}

		public void SetWwwFormData (DataDictionary data)
		{
			PostData = data;
		}

		private DataDictionary ParseCookies ()
		{
			string cookie_header;

			if (!Headers.TryGetValue ("Cookie", out cookie_header))
				return new DataDictionary ();

			return HttpCookie.FromHeader (cookie_header);
		}

		private int OnPath (HttpParser parser, ByteBuffer data, int pos, int len)
		{
			string str = Encoding.ASCII.GetString (data.Bytes, pos, len);

			str = HttpUtility.UrlDecode (str, Encoding.ASCII);
			Path = Path == null ? str : String.Concat (Path, str);
			return 0;
		}

		private int OnQueryString (HttpParser parser, ByteBuffer data, int pos, int len)
		{
			string str = Encoding.ASCII.GetString (data.Bytes, pos, len);

			query_data_builder.Append (str);
			return 0;
		}

		protected override void OnFinishedReading (HttpParser parser)
		{
			base.OnFinishedReading (parser);

			MajorVersion = parser.Major;
			MinorVersion = parser.Minor;
			Method = parser.HttpMethod;

			if (query_data_builder.Length != 0) {
				QueryData = HttpUtility.ParseUrlEncodedData (query_data_builder.ToString ());
				query_data_builder.Length = 0;
			}

			Transaction.OnRequestReady ();
		}

		public override ParserSettings CreateParserSettings ()
		{
			ParserSettings settings = new ParserSettings ();

			settings.OnPath = OnPath;
			settings.OnQueryString = OnQueryString;

			return settings;
		}

		public void Execute ()
		{
            Socket = AppHost.IOLoop.CreateSocket ();
			Socket.Connect (RemoteAddress, RemotePort, delegate {
				Stream = new HttpStream (this, Socket.GetSocketStream ());
				Stream.Chunked = false;
				Stream.AddHeaders = false;

				byte [] body = GetBody ();

				if (body != null) {
					Headers.ContentLength = body.Length;
					Stream.Write (body, 0, body.Length);
				}

				Stream.End (() => {
					HttpResponse response = new HttpResponse (this, Socket);

//					response.OnCompleted += () => {
//						if (OnResponse != null)
//							OnResponse (response);
//					};
					
					response.Read (() => {
						if (OnResponse != null) OnResponse (response);
					});
				});
			});
		}

		public override void WriteMetadata (StringBuilder builder)
		{
			builder.Append (Encoding.ASCII.GetString (HttpMethodBytes.GetBytes (Method)));
			builder.Append (" ");
			builder.Append (Path);
			builder.Append (" HTTP/");
			builder.Append (MajorVersion.ToString (CultureInfo.InvariantCulture));
			builder.Append (".");
			builder.Append (MinorVersion.ToString (CultureInfo.InvariantCulture));
			builder.Append ("\r\n");
			Headers.Write (builder, null, Encoding.ASCII);
		}

		public event Action<IHttpResponse> OnResponse;
	}
}

