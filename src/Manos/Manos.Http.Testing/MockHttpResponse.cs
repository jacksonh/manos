using System;
using System.IO;
using System.Text;
using System.Collections.Generic;

namespace Manos.Http.Testing
{
	public class MockHttpResponse : Manos.Http.IHttpResponse
	{
		StringBuilder builder = new StringBuilder ();
		Dictionary<string, HttpCookie> cookies = new Dictionary<string, HttpCookie> ();

		public MockHttpResponse ()
		{
			this.Headers = new HttpHeaders ();
			Properties = new Dictionary<string,object> ();
		}

		public void Dispose ()
		{
		}

		public IHttpTransaction Transaction { get; set; }

		public HttpHeaders Headers { get; set; }

		public HttpStream Stream {
			get {
				throw new NotImplementedException ();
			}
		}

		public String ResponseString()
		{
			return this.builder.ToString();	
		}
		
		public StreamWriter Writer { get{throw new NotImplementedException();} }

		public Encoding ContentEncoding {
			get { return Headers.ContentEncoding; }
			set { Headers.ContentEncoding = value; }
		}

		public int StatusCode { get; set; }

		public bool WriteHeaders { get; set; }

		public void Write (string str)
		{
			this.builder.Append(str);
		}
		public void Write (string str, params object[] prms)
		{
			this.builder.AppendFormat (str, prms);
		}

		public void WriteLine (string str)
		{
			this.builder.AppendLine (str);
		}
		public void WriteLine (string str, params object[] prms)
		{
			this.builder.AppendLine(String.Format(str, prms));
		}

		public void End ()
		{
		}
		public void End (string str)
		{
			this.Write (str);
		}
		public void End (byte[] data)
		{
			this.Write (data);
		}
		public void End (string str, params object[] prms)
		{
			this.Write (str, prms);
		}

		public void Write (byte[] data)
		{
			throw new NotImplementedException();
		}

		public void Write (byte [] data, int offset, int length)
		{
			throw new NotImplementedException ();
		}

		public void End (byte [] data, int offset, int length)
		{
			throw new NotImplementedException ();
		}

		public void SendFile (string file)
		{
			SentFile = file;
		}
		
		public string SentFile
		{
			get;
			private set;
		}

		public string RedirectedUrl
		{
			get;
			private set;
		}

		public void Redirect (string url)
		{
			RedirectedUrl = url;
		}

		public void Read ()
		{
			throw new NotImplementedException ();
		}

		public void SetHeader (string name, string value)
		{
			this.Headers.SetHeader (name, value);
		}
		
		public Dictionary<string,HttpCookie> Cookies {
			get { return cookies; }
		}
		
		public void SetCookie (string name, HttpCookie cookie)
		{
			cookies [name] = cookie;
		}
		public HttpCookie SetCookie (string name, string value)
		{
			var cookie = new HttpCookie (name, value);
			
			SetCookie (name, cookie);
			return cookie;
		}
		public HttpCookie SetCookie (string name, string value, string domain)
		{
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
		
		public void Complete (Manos.IO.WriteCallback callback)
		{
			
		}
		
		public Dictionary<string,object> Properties {
			get;
			set;
		}

		public string PostBody {
			get;
			set;
		}

		public void WriteMetadata (StringBuilder builder)
		{
			throw new NotImplementedException ();
		}
		
		public void SetProperty (string name, object o)
		{
			if (name == null)
				throw new ArgumentNullException ("name");

			if (o == null) {
				Properties.Remove (name);
				return;
			}

			Properties [name] = o;
		}

		public object GetProperty (string name)
		{
			if (name == null)
				throw new ArgumentNullException ("name");

			object res = null;
			if (Properties.TryGetValue (name, out res))
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
		
		public event Action OnCompleted;
		public event Action<byte [], int, int> BodyData;

		
		public event Action OnEnd;
	}
}

