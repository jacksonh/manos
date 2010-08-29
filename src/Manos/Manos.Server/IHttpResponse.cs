

using System;
using System.Text;

namespace Mango.Server {

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

		void Write (byte [] data);

		void SendFile (string file);

		void Finish ();

		void SetHeader (string name, string value);
	}
}

