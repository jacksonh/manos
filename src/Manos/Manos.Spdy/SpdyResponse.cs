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

		public SpdyRequest Request { get; set; }

		private SpdyStream writestream;
		private Dictionary<string, HttpCookie> cookies;
		private Dictionary<string, object> properties;

		public SpdyResponse (SpdyRequest req,SpdyStream writestream)
		{
			this.Request = req;
			this.Headers = new HttpHeaders ();
			this.cookies = new Dictionary<string, HttpCookie> ();
			this.writestream = writestream;
		}

		#region IHttpResponse implementation
		public event Action OnEnd;
		public event Action OnCompleted;

		private void EnsureReplyWritten (bool done)
		{
			if (writestream.ReplyWritten)
				return;
			writestream.WriteReply (this, done);
		}

		private void EnsureReplyWritten ()
		{
			EnsureReplyWritten (false);
		}

		public Dictionary<string,object> Properties {
			get {
				if (properties == null)
					properties = new Dictionary<string,object> ();
				return properties;
			}
		}

		public void SetProperty (string name, object o)
		{
			if (name == null)
				throw new ArgumentNullException ("name");

			if (o == null && properties == null)
				return;

			if (properties == null)
				properties = new Dictionary<string,object> ();

			if (o == null) {
				properties.Remove (name);
				if (properties.Count == 0)
					properties = null;
				return;
			}

			properties [name] = o;
		}

		public object GetProperty (string name)
		{
			if (name == null)
				throw new ArgumentNullException ("name");

			if (properties == null)
				return null;

			object res = null;
			if (!properties.TryGetValue (name, out res))
				return null;
			return res;
		}

		public T GetProperty<T> (string name)
		{
			object res = GetProperty (name);
			if (res == null)
				return default (T);
			return (T) res;
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
			if (OnEnd != null) {
				OnEnd ();
			}
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
			EnsureReplyWritten ();
			writestream.Write (data, offset, length);
		}

		public void Complete (Action callback)
		{
			EnsureReplyWritten ();
			writestream.End ();
			callback ();
		}

		public void SendFile (string file)
		{
			Headers.SetNormalizedHeader ("Content-Type", ManosMimeTypes.GetMimeType (file));
			EnsureReplyWritten ();
			writestream.SendFile (file);
		}

		public void Redirect (string url)
		{
			Headers.SetNormalizedHeader ("Location", url);
			StatusCode = 302;
			EnsureReplyWritten (true);
			End ();
		}

		public void SetHeader (string name, string value)
		{
			this.Headers.SetHeader (name, value);
		}

		public void SetCookie (string name, HttpCookie cookie)
		{
			cookies [name] = cookie;
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

		public void RemoveCookie (string name)
		{
			var cookie = new HttpCookie (name, "");
			cookie.Expires = DateTime.Now.AddYears (-1);

			SetCookie (name, cookie);
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
				this.statuscode = value;
			}
		}

		public bool WriteHeaders { get; set; }

		public string PostBody { get; set; }

		#endregion

		#region IDisposable implementation
		public void Dispose ()
		{
		}
		#endregion
	}
}

