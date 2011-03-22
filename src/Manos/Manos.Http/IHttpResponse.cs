//
// Copyright (C) 2010 Jackson Harper (jackson@manosdemono.com)
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//
//



using System;
using System.IO;
using System.Text;
using System.Collections.Generic;

namespace Manos.Http {

	public interface IHttpResponse : IDisposable {

		HttpHeaders Headers {
			get;
		}

		HttpStream Stream {
			get;
		}

		
		StreamWriter Writer {
			get;
		}

		Encoding ContentEncoding {
			get;
			set;
		}

		int StatusCode {
			get;
			set;
		}

		bool WriteHeaders {
			get;
			set;
		}

		
		Dictionary<string,object> Properties {
			get;
		}

		
		string PostBody {
			get;
			set;
		}

		void SetProperty (string name, object o);

		object GetProperty (string name);

		T GetProperty<T> (string name);
		
		void Write (string str);
		void Write (string str, params object [] prms);
		
		void WriteLine (string str);
		void WriteLine (string str, params object [] prms);

		void Write (byte [] data);
		void Write (byte [] data, int offset, int length);

		void End ();
		void End (string str);
		void End (byte [] data);
		void End (byte [] data, int offset, int length);
		void End (string str, params object [] prms);

		void Complete (Manos.IO.WriteCallback callback);

		void SendFile (string file);
		
		void Redirect (string url);

		void SetHeader (string name, string value);
				
		void SetCookie (string name, HttpCookie cookie);
		HttpCookie SetCookie (string name, string value);
		HttpCookie SetCookie (string name, string value, string domain);
		HttpCookie SetCookie (string name, string value, DateTime expires);
		HttpCookie SetCookie (string name, string value, string domain, DateTime expires);
		HttpCookie SetCookie (string name, string value, TimeSpan max_age);
		HttpCookie SetCookie (string name, string value, string domain, TimeSpan max_age);

		void Read ();
		void WriteMetadata (StringBuilder builder);

		event Action OnEnd;
		event Action OnCompleted;
	}
}

