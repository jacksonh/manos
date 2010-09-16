

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
		
		void Redirect (string url);
	}
}

