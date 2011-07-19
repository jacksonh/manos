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
using System.Globalization;
using System.Collections.Generic;
using System.Collections.Specialized;

using Manos;
using Manos.IO;
using Manos.Http;
using Manos.Collections;



namespace Manos.Http.Testing
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
		
		public MockHttpRequest()
		{
			Reset();		
		}
		
		public MockHttpRequest (HttpMethod method, string path)
		{
			Method = method;
			Path = path;

			Reset();
		}

		public void Reset()
		{
			data = new DataDictionary ();
			uri_data = new DataDictionary ();
			query_data = new DataDictionary ();
			post_data = new DataDictionary ();
		
			Properties = new Dictionary<string,object> ();
		
			data.Children.Add (UriData);
			data.Children.Add (QueryData);
			data.Children.Add (PostData);
		}
		
		public void Dispose ()
		{
		}

		public HttpMethod Method {
			get;
			set;
		}
		
		
		public string Path {
			get;
			set;
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
			set {
				uri_data = value;
			}
		}
		
		public DataDictionary QueryData {
			get {
			    return query_data;
			}
			set {
				query_data = value;
			}
		}

		public DataDictionary Cookies {
			get {
				if (cookies == null)
					cookies = new DataDictionary ();
				return cookies;
			}
			
			set { cookies = value; }
		}
		
		public HttpHeaders Headers {
			get {
			    if (headers == null)
			       headers = new HttpHeaders ();
			    return headers;
			}
			set {
				headers = value;
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

		public int MajorVersion {
			get;
			set;
		}

		public int MinorVersion {
			get;
			set;
		}

		public ITcpSocket Socket {
			get;
			set;
		}

		public string PostBody {
			get;
			set;
		}

		public Dictionary<string,object> Properties {
			get;
			set;
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
			if (!Properties.TryGetValue (name, out res))
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
		
		public void Read (Action onClose)
		{
		}

		public void SetWwwFormData (DataDictionary data)
		{
			PostData.Children.Add (data);
		}

		public void WriteMetadata (StringBuilder builder)
		{
			builder.Append (Encoding.ASCII.GetString (HttpMethodBytes.GetBytes (Method)));
			builder.Append (" ");
			builder.Append (Path);
			builder.Append (" HTTP/");
			builder.Append (MajorVersion.ToString (CultureInfo.InvariantCulture));
			builder.Append (".");
			builder.Append (MinorVersion.ToString (CultureInfo.InvariantCulture));
			builder.Append ("\r\n");
			Headers.Write (builder, null, Encoding.ASCII);		
		}
	}
}
