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
using System.Text;
using System.Collections.Generic;
using System.Collections.Specialized;

using Manos.IO;
using Manos.Http;
using Manos.Collections;

namespace Manos.Http {

	public interface IHttpRequest : IDisposable {
		
		HttpMethod Method {
			get;
			set;
		}
		
		string Path {
			get;
			set;
		}

		DataDictionary Data {
			get;	
		}
		
		DataDictionary PostData {
			get;
		}

		DataDictionary QueryData {
			get;
			set;
		}

		DataDictionary UriData {
			get;
			set;
		}
		
		DataDictionary Cookies {
			get;	
		}

		HttpHeaders Headers {
			get;
			set;
		}
		
		Dictionary<string,UploadedFile> Files {
			get;
		}

		int MajorVersion {
			get;
			set;
		}

		int MinorVersion {
			get;
			set;
		}

		Encoding ContentEncoding {
			get;
		}

		Socket Socket {
			get;
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
		
		void Read (Action onClose);
		void SetWwwFormData (DataDictionary data);
		void WriteMetadata (StringBuilder builder);

	}
}

