using System;
using System.IO;
using System.Text;
using System.Collections.Generic;

using Manos.Http;
using Manos.IO;

namespace Manos.Spdy
{
	public class SpdyResponse : IHttpResponse
	{
		private int statuscode;
		private SpdyRequest request;
		private Socket socket;
		private DeflatingZlibContext deflate;
		public SpdyResponse (SpdyRequest req, Socket sock, DeflatingZlibContext deflate)
		{
			this.request = req;
			this.socket = sock;
			this.Headers = new HttpHeaders();
			this.deflate = deflate;
		}

		#region IHttpResponse implementation
		public event Action OnEnd;

		public event Action OnCompleted;

		public void SetProperty (string name, object o)
		{
			throw new NotImplementedException ("SetProperty");
		}

		public object GetProperty (string name)
		{
			throw new NotImplementedException ("GetProperty");
		}

		public T GetProperty<T> (string name)
		{
			throw new NotImplementedException ("GetProperty");
		}
		public void Write (string str)
		{
			byte [] data = ContentEncoding.GetBytes (str);

			WriteToBody (data, 0, data.Length);
		}

		public void Write (byte [] data)
		{
			WriteToBody (data, 0, data.Length);
		}

		public void Write (byte [] data, int offset, int length)
		{
			WriteToBody (data, offset, length);
		}

		public void Write (string str, params object [] prms)
		{
			Write (String.Format (str, prms));	
		}

		public void WriteLine (string str)
		{
			Write (str + Environment.NewLine);	
		}

		public void WriteLine (string str, params object [] prms)
		{
			WriteLine (String.Format (str, prms));	
		}

		public void End ()
		{
			Console.WriteLine("End");
			DataFrame d = new DataFrame();
			d.Flags = 0x01;
			d.StreamID = this.request.StreamID;
			d.Length = 0;
			d.Data = new byte[0];
			byte[] ret = d.Serialize();
			this.socket.GetSocketStream().Write (new ByteBuffer(ret, 0, ret.Length));
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

		public void End (byte [] data, int offset, int length)
		{
			Write (data, offset, length);
			End ();
		}

		public void End (string str, params object [] prms)
		{
			Write (str, prms);
			End ();
		}
		
		private void WriteToBody (byte [] data, int offset, int length)
		{
			DataFrame d = new DataFrame();
			d.Flags = 0x00;
			d.StreamID = this.request.StreamID;
			d.Length = length - offset;
			d.Data = new byte[d.Length];
			Array.Copy(data, offset, d.Data, 0, length);
			var ret = d.Serialize();
			this.socket.GetSocketStream().Write (new ByteBuffer(ret, 0, ret.Length));
		}

		public void Complete (Action callback)
		{
			throw new NotImplementedException ("Complete");
		}

		public void SendFile (string file)
		{
			throw new NotImplementedException ("SendFile");
		}

		public void Redirect (string url)
		{
			StatusCode = 302;
			Headers.SetNormalizedHeader("Location", url);
			End();
		}

		public void SetHeader (string name, string value)
		{
			throw new NotImplementedException ("SetHeader");
		}

		public void SetCookie (string name, HttpCookie cookie)
		{
			throw new NotImplementedException ("SetCookie");
		}

		public HttpCookie SetCookie (string name, string value)
		{
			throw new NotImplementedException ("SetCookie");
		}

		public HttpCookie SetCookie (string name, string value, string domain)
		{
			throw new NotImplementedException ("SetCookie");
		}

		public HttpCookie SetCookie (string name, string value, DateTime expires)
		{
			throw new NotImplementedException ("SetCookie");
		}

		public HttpCookie SetCookie (string name, string value, string domain, DateTime expires)
		{
			throw new NotImplementedException ("SetCookie");
		}

		public HttpCookie SetCookie (string name, string value, TimeSpan max_age)
		{
			throw new NotImplementedException ("SetCookie");
		}

		public HttpCookie SetCookie (string name, string value, string domain, TimeSpan max_age)
		{
			throw new NotImplementedException ("SetCookie");
		}

		public void RemoveCookie (string name)
		{
			throw new NotImplementedException ("RemoveCookie");
		}

		public void Read ()
		{
			throw new NotImplementedException ("Read");
		}

		public void WriteMetadata (StringBuilder builder)
		{
			throw new NotImplementedException ("WriteMetadata");
		}

		public HttpHeaders Headers { get; set; }

		public HttpStream Stream {
			get {
				throw new NotImplementedException ("Stream");
			}
		}

		public StreamWriter Writer {
			get {
				throw new NotImplementedException ("Writer");
			}
		}

		public Encoding ContentEncoding {
			get {
				return this.Headers.ContentEncoding;
			}
			set {
				this.Headers.ContentEncoding = value;
			}
		}

		public int StatusCode {
			get {
				return statuscode;
			}
			set {
				if (statuscode != 0) {
					throw new Exception("Status Code already Set");
				}
				else {
					this.statuscode = value;
					SynReplyFrame rep = new SynReplyFrame();
					rep.StreamID = this.request.StreamID;
					rep.Version = 2;
					rep.Flags = 0x00;
					rep.Headers = new NameValueHeaderBlock();
					rep.Headers["version"] = "HTTP/" + this.request.MajorVersion + "." + this.request.MinorVersion;
					rep.Headers["status"] = value.ToString();
					foreach (var header in this.Headers.Keys)
					{
						rep.Headers[header] = this.Headers[header];
					}
					this.socket.GetSocketStream().Write(rep.Serialize(this.deflate));
				}
			}
		}

		public bool WriteHeaders {
			get {
				throw new NotImplementedException ("WriteHeaders");
			}
			set {
				throw new NotImplementedException ("WriteHeaders");
			}
		}

		public Dictionary<string, object> Properties {
			get {
				throw new NotImplementedException ("Properties");
			}
		}

		public string PostBody {
			get {
				throw new NotImplementedException ("PostBody");
			}
			set {
				throw new NotImplementedException ("PostBody");
			}
		}
		#endregion

		#region IDisposable implementation
		public void Dispose ()
		{
			throw new NotImplementedException ("Dispose");
		}
		#endregion
	}
}

