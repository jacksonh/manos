using System;
using System.IO;
using System.Text;
using System.Collections.Generic;

namespace Manos.Http.Testing
{
	public class MockHttpResponse : Manos.Http.IHttpResponse
	{
		StringBuilder builder = new StringBuilder ();

		public MockHttpResponse (IHttpTransaction txn)
		{
			this.Headers = new HttpHeaders ();
			this.Transaction = txn;
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
			get {
				throw new NotImplementedException ();
			}
			set{
				throw new NotImplementedException();	
			}
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
			throw new NotImplementedException ();
		}

		public void Redirect (string url)
		{
			throw new NotImplementedException ();
		}

		public void Read ()
		{
			throw new NotImplementedException ();
		}

		public void SetHeader (string name, string value)
		{
			this.Headers.SetHeader (name, value);
		}
		public void SetCookie (string name, HttpCookie cookie)
		{
			throw new NotImplementedException ();
		}
		public HttpCookie SetCookie (string name, string value)
		{
			throw new NotImplementedException ();
		}
		public HttpCookie SetCookie (string name, string value, string domain)
		{
			throw new NotImplementedException ();
		}
		public HttpCookie SetCookie (string name, string value, DateTime expires)
		{
			throw new NotImplementedException ();
		}
		public HttpCookie SetCookie (string name, string value, string domain, DateTime expires)
		{
			throw new NotImplementedException ();
		}
		public HttpCookie SetCookie (string name, string value, TimeSpan max_age)
		{
			throw new NotImplementedException ();
		}
		public HttpCookie SetCookie (string name, string value, string domain, TimeSpan max_age)
		{
			throw new NotImplementedException ();
		}
		public void Complete (Manos.IO.WriteCallback callback)
		{
			throw new NotImplementedException ();
		}
		
		public Dictionary<string,object> Properties {
			get;
			set;
		}

		public string PostBody {
			get;
			set;
		}

		public void SetProperty (string name, object o)
		{
			throw new NotImplementedException ();
		}

		public object GetProperty (string name)
		{
			throw new NotImplementedException ();
		}

		public T GetProperty<T> (string name)
		{
			throw new NotImplementedException ();
		}
		
		public event Action OnCompleted;
		public event Action<byte [], int, int> BodyData;

		
		public event Action OnEnd;
	}
}

