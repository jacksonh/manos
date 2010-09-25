

using System;
using System.Text;

namespace Manos.Server {

	public interface IHttpResponse {

		IHttpTransaction Transaction {
			get;
		}

		HttpHeaders Headers {
			get;
		}

		Encoding Encoding {
			get;
		}

		int StatusCode {
			get;
			set;
		}

		bool WriteStatusLine {
			get;
			set;
		}

		bool WriteHeaders {
			get;
			set;
		}

		void Write (string str);
		void Write (string str, params object [] prms);
		
		void WriteLine (string str);
		void WriteLine (string str, params object [] prms);

		void Write (byte [] data);

		void SendFile (string file);

		void Finish ();

		void SetHeader (string name, string value);
				
		void SetCookie (string name, HttpCookie cookie);
		HttpCookie SetCookie (string name, string value);
		HttpCookie SetCookie (string name, string value, string domain);
		HttpCookie SetCookie (string name, string value, DateTime expires);
		HttpCookie SetCookie (string name, string value, string domain, DateTime expires);
		HttpCookie SetCookie (string name, string value, TimeSpan max_age);
		HttpCookie SetCookie (string name, string value, string domain, TimeSpan max_age);
		
		void Redirect (string url);
	}
}

