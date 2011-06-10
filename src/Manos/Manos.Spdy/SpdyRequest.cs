using System;
using System.Text;

using Manos.Http;

namespace Manos.Spdy
{
	public class SpdyRequest : IHttpRequest
	{
		private Action RequestReady;
		private SpdyTransaction transaction;
		private HttpHeaders headers;
		public int StreamID { get; set; }
		public SpdyRequest (SpdyTransaction trans, Action ready)
		{
			this.transaction = trans;
			this.RequestReady = ready;
			var version = trans.SynStream.Headers["version"];
			var num = version.Split('/')[1];
			var numsplit = num.Split('.');
			this.MajorVersion = int.Parse(numsplit[0]);
			this.MinorVersion = int.Parse(numsplit[1]);
			this.headers = trans.SynStream.Headers.ToHttpHeaders(new string[] { "version", "url" });
			this.Path = trans.SynStream.Headers["url"];
			this.Method = trans.MethodFromString(trans.SynStream.Headers["method"]);
			this.StreamID = trans.SynStream.StreamID;
		}

		#region IHttpRequest implementation
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

		public void Read (Action onClose)
		{
			throw new NotImplementedException ("Read");
		}

		public void SetWwwFormData (Manos.Collections.DataDictionary data)
		{
			throw new NotImplementedException ("SetWwwFormData");
		}

		public void WriteMetadata (System.Text.StringBuilder builder)
		{
			throw new NotImplementedException ("WriteMetadata");
		}

		public HttpMethod Method { get; set; }

		public string Path { get; set; }

		public Manos.Collections.DataDictionary Data {
			get {
				throw new NotImplementedException ("Data");
			}
		}

		public DataDictionary PostData {
			get {
				throw new NotImplementedException ("PostData");
			}
		}

		public DataDictionary QueryData {
			get {
				throw new NotImplementedException ("QueryData");
			}
			set {
				throw new NotImplementedException ("QueryData");
			}
		}

		public DataDictionary UriData {
			get {
				throw new NotImplementedException ("UriData");
			}
			set {
				throw new NotImplementedException ("UriData");
			}
		}

		public DataDictionary Cookies {
			get {
				throw new NotImplementedException ("Cookies");
			}
		}

		public HttpHeaders Headers {
			get {
				return headers;
			}
			set {
				headers = value;
			}
		}

		public Dictionary<string, UploadedFile> Files {
			get {
				throw new NotImplementedException ("Files");
			}
		}

		public int MajorVersion { get; set; }

		public int MinorVersion { get; set; }

		public Encoding ContentEncoding {
			get {
				return Headers.ContentEncoding;
			}
		}

		public Socket Socket {
			get {
				return transaction.Socket;
			}
		}

		public Dictionary<string, object> Properties {
			get {
				throw new NotImplementedException ("Properties");
			}
		}

		public string PostBody {
			get {
				return Encoding.UTF8.GetString(transaction.DataArray);
			}
			set {
				transaction.DataArray = Encoding.UTF8.GetBytes(value);
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

