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

using Manos;
using Manos.Server;
using Manos.Collections;


namespace Manos.Server.Testing
{
	public class MockHttpRequest : IHttpRequest
	{
		private DataDictionary data;
		private DataDictionary uri_data;
		private DataDictionary post_data;
		private DataDictionary query_data;
		private DataDictionary cookies;
		private HttpHeaders headers;
		private Dictionary<string,UploadedFile> uploaded_files;
		private Encoding encoding;

		private bool http_1_1_supported;
		
		public MockHttpRequest (string method, string local_path)
		{
			Method = method;
			LocalPath = local_path;

			data = new DataDictionary ();
			uri_data = new DataDictionary ();
			query_data = new DataDictionary ();
			post_data = new DataDictionary ();
			
			data.Children.Add (UriData);
			data.Children.Add (QueryData);
			data.Children.Add (PostData);
		}
		
		public string Method {
			get;
			private set;
		}
		
		
		public string LocalPath {
			get;
			private set;
		}
		
		public bool Aborted {
			get;
			private set;
		}
		
		public DataDictionary Data {
			get {
				return data;
			}
		}
		
		public DataDictionary PostData {
			get {
			    return post_data;
			}
		}

		public DataDictionary UriData {
			get {
				return uri_data;
			}
		}
		
		public DataDictionary QueryData {
			get {
			    return query_data;
			}
		}

		public DataDictionary Cookies {
			get {
				if (cookies == null)
					cookies = new DataDictionary ();
				return cookies;
			}
		}
		
		public HttpHeaders Headers {
			get {
			    if (headers == null)
			       headers = new HttpHeaders ();
			    return headers;
			}
		}
		
		public Encoding ContentEncoding {
		       get {
		       	   if (encoding == null)
			      encoding = Encoding.Default;
			   return encoding;
		       }
		}

		public Dictionary<string,UploadedFile> Files {
			get {
			    if (uploaded_files == null)
			       uploaded_files = new Dictionary<string,UploadedFile> ();
			    return uploaded_files;
			}
		}

		public bool Http_1_1_Supported {
			get {
			    return http_1_1_supported;
			}
			set {
			    http_1_1_supported = value;
			}
		}

		public void SetWwwFormData (DataDictionary data)
		{
			PostData.Children.Add (data);
		}
	}
}
