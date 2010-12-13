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

		public HttpRequest (IHttpTransaction transaction, SocketStream stream)
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

		public string RemoteAddress {
			get;
			set;
		}

		public int RemotePort {
			get;
			set;
		}

		public string Path {
			get;
			set;
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

		public override void Reset ()
		{
			Path = null;

			uri_data = null;
			query_data = null;

			base.Reset ();
		}

		public void SetWwwFormData (DataDictionary data)
		{
			PostData = data;
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

			Console.WriteLine ("A REQUEST IS READY!");
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
			Socket = new SocketStream (AppHost.IOLoop);
			Socket.Connect (RemoteAddress, RemotePort);

			Socket.Connected += delegate {
				Stream = new HttpStream (this, Socket);

				Stream.WriteMetadata (() => {
					HttpResponse response = new HttpResponse (Socket);

					if (BodyData != null)
						BodyData (this);
					if (Connected != null)
						Connected (response);
				});
			};
		}

		public override void WriteMetadata (StringBuilder builder)
		{
			Headers.SetNormalizedHeader ("Transfer-Encoding", "chunked");

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

		public event Action<IHttpResponse> Connected;
		public event Action<IHttpRequest> BodyData;
	}
}

