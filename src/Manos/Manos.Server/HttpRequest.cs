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
using System.Net;
using System.Net.Sockets;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;


using Mono.Unix.Native;

using Manos.Collections;

namespace Manos.Server {

	public class HttpRequest : IHttpRequest {

		private DataDictionary data;
		private DataDictionary uri_data;
		private	DataDictionary query_data;
		private DataDictionary post_data;

		private DataDictionary cookies;
		private Dictionary<string,UploadedFile> uploaded_files;
		
		public HttpRequest (IHttpTransaction transaction, HttpHeaders headers, string method, string resource, bool support_1_1)
		{
			Transaction = transaction;
			Headers = headers;
			Method = method;
			ResourceUri = resource;
			Http_1_1_Supported = support_1_1;

			SetEncoding ();
			SetPathAndQuery ();
		}

		public IHttpTransaction Transaction {
			get;
			private set;
		}

		public HttpHeaders Headers {
			get;
			private set;
		}

		public string Method {
			get;
			private set;
		}

		public string ResourceUri {
			get;
			private set;
		}

		public bool Http_1_1_Supported {
			get;
			private set;
		}

		public string LocalPath {
			get;
			private set;
		}
		
		public DataDictionary Data {
			get {
				if (data == null)
					data = new DataDictionary ();
				return data;
			}
		}
		
		public DataDictionary PostData {
			get {
				if (post_data == null) {
					post_data = new DataDictionary ();
					Data.Children.Add (post_data);
				}
				return post_data;
			}
			set {
				SetDataDictionary (post_data, value);
				post_data = value;
			}
		}

		public DataDictionary QueryData {
			get {
				if (query_data == null) {
					query_data = new DataDictionary ();
					Data.Children.Add (query_data);
				}
				return query_data;
			}
			set {
				SetDataDictionary (query_data, value);
				query_data = value;
			}
		}

		public DataDictionary UriData {
			get {
				if (uri_data == null) {
					uri_data = new DataDictionary ();
					Data.Children.Add (uri_data);
				}
				return uri_data;
			}
			set {
				SetDataDictionary (uri_data, value);
				uri_data = value;
			}
		}
		
		public DataDictionary Cookies {
			get {
				if (cookies == null)
					cookies = ParseCookies ();
				return cookies;
			}
		}
		
		public Dictionary<string,UploadedFile> Files {
			get {
			    if (uploaded_files == null)
			       uploaded_files = new Dictionary<string,UploadedFile> ();
			    return uploaded_files;
			}
		}

		public Encoding ContentEncoding {
			get;
			private set;
		}

		private void SetEncoding ()
		{
			string content;

			if (!Headers.TryGetValue ("Content-Type", out content)) {
				ContentEncoding = Encoding.ASCII;
				return;
			}

			// TODO: parse charsets
			ContentEncoding = Encoding.Default;
		}

		private void SetPathAndQuery ()
		{
			// This is used with the OPTIONS verb
			if (ResourceUri == "*")
				return;

			string uri = ResourceUri;
			string scheme;
			string host;
			string path;
			string query;

			if (!UriParser.TryParse (uri, out scheme, out host, out path, out query)) {
				Transaction.Abort (400, "Invalid resource path. '{0}'", uri);
				return;
			}

			LocalPath = path;

			// TODO: Pass this to the encoder to populate
			QueryData = HttpUtility.ParseUrlEncodedData (query);
		}

		private DataDictionary ParseCookies ()
		{
			string cookie_header;

			if (!Headers.TryGetValue ("Cookie", out cookie_header))
				return new DataDictionary ();
			
			return HttpCookie.FromHeader (cookie_header);
		}
		
		public void SetWwwFormData (DataDictionary data)
		{
			PostData = data;
		}

		private void SetDataDictionary (DataDictionary old, DataDictionary newd)
		{
			if (data != null && old != null)
				data.Children.Remove (old);
			if (newd != null)
				Data.Children.Add (newd);
		}

	}
}

