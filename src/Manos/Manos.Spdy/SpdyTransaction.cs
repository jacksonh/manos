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

namespace Manos.Spdy
{
	public class SpdyTransaction : IHttpTransaction
	{
		private static int bufferlength = 1000;

		public Context Context { get; set; }

		public SynStreamFrame SynStream { get; set; }

		public SPDYParser Parser { get; set; }

		public SpdyStream WriteStream { get; set; }

		public SpdyConnectionCallback Callback { get; set; }

		private byte [] data;

		public byte [] DataArra {
			get {
				return data;
			}
		}

		private int DataIndex { get; set; }

		public SpdyTransaction (Context context,SynStreamFrame synstream,SPDYParser parser,SpdyStream writestream,SpdyConnectionCallback callback)
		{
			this.Context = context;
			this.SynStream = synstream;
			this.Parser = parser;
			this.WriteStream = writestream;
			this.Callback = callback;
			if ((synstream.Flags & 0x01) == 1) {
				this.data = new byte[0];
				this.Request = new SpdyRequest (context, this.SynStream);
				Context.CreateTimerWatcher (new TimeSpan (1), OnRequestReady).Start ();
			} else {
				if (synstream.Headers ["Content-Length"] != null) {
					data = new byte[int.Parse (synstream.Headers ["Content-Length"])];
				} else {
					data = new byte[bufferlength];
				}
				parser.OnData += delegate(DataFrame packet) {
					if (packet.Data.Length > 0) {
						if (packet.Data.Length + DataIndex > data.Length) {
							Array.Resize (ref data, packet.Data.Length + DataIndex);
						}
						Array.Copy (packet.Data, 0, data, this.DataIndex, packet.Data.Length);
						this.DataIndex += packet.Data.Length;
					}
					if ((packet.Flags & 0x01) == 1) {
						this.Request = new SpdyRequest (context, this.SynStream, data);
						OnRequestReady ();
					}
				};
			}
		}

		public HttpServer Server {
			get {
				throw new NotImplementedException ("Server");
			}
			set {
				throw new NotImplementedException ("Server");
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

		public bool Aborted { get; private set; }

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
				Response = new SpdyResponse (Request as SpdyRequest, WriteStream);
				ResponseReady = true;
				//if( closeOnEnd ) Response.OnEnd += () => Response.Complete( OnResponseFinished );
				this.Callback (this);
			} catch (Exception e) {
				Console.WriteLine ("Exception while running transaction");
				Console.WriteLine (e);
			}
		}

		public void OnResponseFinished ()
		{
			//Request.Read (Close);
			//Close();
		}

		public void Abort (int status, string message, params object [] p)
		{
			throw new NotImplementedException ();
		}
	}
}