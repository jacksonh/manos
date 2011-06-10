using System;
using System.IO;
using System.Text;
using System.Linq;
using System.Net;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using Manos.IO;
using Manos.Collections;
using Manos.Http;

namespace Manos.Spdy {

	public class SpdyTransaction : IHttpTransaction {
		
		public SynStreamFrame SynStream { get; set; }
		public SPDYParser Parser { get; set; }
		public SpdyConnectionCallback Callback { get; set; }
		public byte[] DataArray { get; set; }
		public Socket Socket { get; set; }
		private SpdySession session { get; set; }
		public SpdyTransaction (Socket socket, SynStreamFrame synstream, SPDYParser parser, SpdyConnectionCallback callback, SpdySession session, List<byte[]> data = null)
		{
			this.SynStream = synstream;
			this.Parser = parser;
			this.Callback = callback;
			this.Socket = socket;
			this.session = session;
			if (data != null) {
				var length = data.Select(x => x.Length).Sum();
				int index = 0;
				this.DataArray = new byte[length];
				foreach (var arr in data) {
					Array.Copy(arr, 0, this.DataArray, index, arr.Length);
					index += arr.Length;
				}
			}
		}
		public void LoadRequest()
		{
				this.Request = new SpdyRequest(this, OnRequestReady);
			OnRequestReady();
		}

		public HttpServer Server {
			get
			{
				throw new NotImplementedException("Server");
			}
			set
			{
				throw new NotImplementedException("Server");
			}
		}
		public Context Context {
			get
			{
				throw new NotImplementedException("Context");
			}
			set
			{
				throw new NotImplementedException("Context");
			}
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
			get 
			{
				throw new NotImplementedException("Aborted");
			}	
		}

		public bool ResponseReady { get; set; }

		public void Close ()
		{

			if (Request != null)
				Request.Dispose ();

			if (Response != null)
				Response.Dispose ();

			Request = null;
			Response = null;
		}
		public void OnRequestReady ()
		{
			try {
				Response = new SpdyResponse (Request as SpdyRequest, Socket, this.session.Deflate);
				ResponseReady = true;
				//if( closeOnEnd ) Response.OnEnd += () => Response.Complete( OnResponseFinished );
				this.Callback(this);
			} catch (Exception e) {
				Console.WriteLine ("Exception while running transaction");
				Console.WriteLine (e);
			}
		}

		public void OnResponseFinished ()
		{
				Request.Read (Close);
		}
		public void Abort (int status, string message, params object [] p)
		{
			throw new NotImplementedException();
		}
		Dictionary<string, HttpMethod> lookup = new Dictionary<string, HttpMethod>() {
			{ "OPTIONS", HttpMethod.HTTP_OPTIONS },
			{ "GET", HttpMethod.HTTP_GET },
			{ "HEAD", HttpMethod.HTTP_HEAD },
			{ "POST", HttpMethod.HTTP_POST },
			{ "PUT", HttpMethod.HTTP_PUT },
			{ "DELETE", HttpMethod.HTTP_DELETE },
			{ "TRACE", HttpMethod.HTTP_TRACE },
			{ "CONNECT", HttpMethod.HTTP_CONNECT }
		};
		public HttpMethod MethodFromString(string str)
		{
			str = str.ToUpper();
			if (lookup.ContainsKey(str)) {
				return lookup[str];
			} else {
				return HttpMethod.ERROR;
			}
		}

	}
}