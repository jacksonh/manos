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

using Manos.IO;


namespace Manos.Http {

	public class HttpResponse : IHttpResponse {

	       	internal bool metadata_written;

		private StreamWriter writer;
		private Dictionary<string, HttpCookie> cookies;

		public HttpResponse ()
		{
		}

		public HttpResponse (IHttpTransaction transaction, IOStream stream)
		{
			Transaction = transaction;
			IOStream = stream;

			StatusCode = 200;

			WriteHeaders = true;

			Headers = new HttpHeaders ();
			Stream = new HttpResponseStream (this, IOStream);
			Stream.Chunked = (transaction.Request.MajorVersion > 0 && transaction.Request.MinorVersion > 0);
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
			get;
			private set;
		}

		public byte [] Body {
			get;
			set;
		}

		public HttpResponseStream Stream {
			get;
			private set;
		}

		public StreamWriter Writer {
			get {
				if (writer == null)
					writer = new StreamWriter (Stream);
				return writer;
			}
		}

		public Encoding ContentEncoding {
			get { return Headers.ContentEncoding; }
			set { Headers.ContentEncoding = value; }
		}

		public int StatusCode {
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

		public bool WriteHeaders {
			get;
			set;
		}

		public Dictionary<string,HttpCookie> Cookies {
			get {
				if (cookies == null)
					cookies = new Dictionary<string, HttpCookie> ();
				return cookies;
			}
		}

		public void Write (string str)
		{
			byte [] data = ContentEncoding.GetBytes (str);

			WriteToBody (data);
		}

		public void Write (byte [] data)
		{
			WriteToBody (data);
		}

		public void Write (string str, params object [] prms)
		{
			Write (String.Format (str, prms));	
		}

		public void End (string str)
		{
			Write (str);
			End ();
		}

		public void End (byte [] data)
		{
			Write (data);
			End ();
		}

		public void End (string str, params object [] prms)
		{
			Write (str, prms);
			End ();
		}

		public void End ()
		{
			if (!Stream.Chunked) {
				Headers.ContentLength = Stream.Length;
				WriteMetadata ();
			}

			Stream.End (Transaction.OnResponseFinished);
		}

		public void WriteLine (string str)
		{
			Write (str + Environment.NewLine);	
		}
		
		public void WriteLine (string str, params object [] prms)
		{
			WriteLine (String.Format (str, prms));	
		}
		
		public void SendFile (string file)
		{
			Stream.SendFile (file);
		}

		public void Redirect (string url)
		{
			StatusCode =  302;
			Headers.SetNormalizedHeader ("Location", url);
			
			WriteMetadata ();
			End ();
		}
		
		public void WriteMetadata ()
		{
			if (metadata_written)
				return;

			SetStandardHeaders ();
			
			StringBuilder builder = new StringBuilder ();
			WriteStatusLine (builder);

			if (WriteHeaders)
				Headers.Write (builder, cookies == null ? null : Cookies.Values, Encoding.ASCII);

			byte [] data = Encoding.ASCII.GetBytes (builder.ToString ());

			metadata_written = true;

			var bytes = new List<ArraySegment<byte>> ();
			bytes.Add (new ArraySegment<byte> (data, 0, data.Length));
			var write_bytes = new SendBytesOperation (bytes, null);

			IOStream.QueueWriteOperation (write_bytes);
		}

		public void Finish ()
		{
			
		}

		public void SetHeader (string name, string value)
		{
			Headers.SetHeader (name, value);
		}

		public void SetCookie (string name, HttpCookie cookie)
		{
			Cookies [name] = cookie;
		}
		
		public HttpCookie SetCookie (string name, string value)
		{
			if (name == null)
				throw new ArgumentNullException ("name");
			if (value == null)
				throw new ArgumentNullException ("value");
			
			var cookie = new HttpCookie (name, value);
			
			SetCookie (name, cookie);
			return cookie;
		}
		
		public HttpCookie SetCookie (string name, string value, string domain)
		{
			if (name == null)
				throw new ArgumentNullException ("name");
			if (value == null)
				throw new ArgumentNullException ("value");
			
			var cookie = new HttpCookie (name, value);
			cookie.Domain = domain;
			
			SetCookie (name, cookie);
			return cookie;
		}
		
		public HttpCookie SetCookie (string name, string value, DateTime expires)
		{
			return SetCookie (name, value, null, expires);
		}
		
		public HttpCookie SetCookie (string name, string value, string domain, DateTime expires)
		{
			if (name == null)
				throw new ArgumentNullException ("name");
			if (value == null)
				throw new ArgumentNullException ("value");
			
			var cookie = new HttpCookie (name, value);
			
			cookie.Domain = domain;
			cookie.Expires = expires;
			
			SetCookie (name, cookie);
			return cookie;
		}
		
		public HttpCookie SetCookie (string name, string value, TimeSpan max_age)
		{
			return SetCookie (name, value, DateTime.Now + max_age);
		}
		
		public HttpCookie SetCookie (string name, string value, string domain, TimeSpan max_age)
		{
			return SetCookie (name, value, domain, DateTime.Now + max_age);
		}
		
		private void WriteStatusLine (StringBuilder builder)
		{
			builder.Append ("HTTP/");
			builder.Append (Transaction.Request.MajorVersion);
			builder.Append (".");
			builder.Append (Transaction.Request.MinorVersion);
			builder.Append (" ");
			builder.Append (StatusCode);
			builder.Append (" ");
			builder.Append (GetStatusDescription (StatusCode));
			builder.Append ("\r\n");
		}

		private void WriteToBody (byte [] data)
		{
			Stream.Write (data, 0, data.Length);
		}

		private void SetStandardHeaders ()
		{
//			Headers.ContentLength = 0;

			Headers.SetNormalizedHeader ("Server", HttpServer.ServerVersion);

			if (Stream.Chunked)
				Headers.SetNormalizedHeader ("Transfer-Encoding", "chunked");
		}

		private static string GetStatusDescription (int code)
		{
			switch (code){
			case 100: return "Continue";
			case 101: return "Switching Protocols";
			case 102: return "Processing";
			case 200: return "OK";
			case 201: return "Created";
			case 202: return "Accepted";
			case 203: return "Non-Authoritative Information";
			case 204: return "No Content";
			case 205: return "Reset Content";
			case 206: return "Partial Content";
			case 207: return "Multi-Status";
			case 300: return "Multiple Choices";
			case 301: return "Moved Permanently";
			case 302: return "Found";
			case 303: return "See Other";
			case 304: return "Not Modified";
			case 305: return "Use Proxy";
			case 307: return "Temporary Redirect";
			case 400: return "Bad Request";
			case 401: return "Unauthorized";
			case 402: return "Payment Required";
			case 403: return "Forbidden";
			case 404: return "Not Found";
			case 405: return "Method Not Allowed";
			case 406: return "Not Acceptable";
			case 407: return "Proxy Authentication Required";
			case 408: return "Request Timeout";
			case 409: return "Conflict";
			case 410: return "Gone";
			case 411: return "Length Required";
			case 412: return "Precondition Failed";
			case 413: return "Request Entity Too Large";
			case 414: return "Request-Uri Too Long";
			case 415: return "Unsupported Media Type";
			case 416: return "Requested Range Not Satisfiable";
			case 417: return "Expectation Failed";
			case 422: return "Unprocessable Entity";
			case 423: return "Locked";
			case 424: return "Failed Dependency";
			case 500: return "Internal Server Error";
			case 501: return "Not Implemented";
			case 502: return "Bad Gateway";
			case 503: return "Service Unavailable";
			case 504: return "Gateway Timeout";
			case 505: return "Http Version Not Supported";
			case 507: return "Insufficient Storage";
			}
			return "";
		}
	}

}



