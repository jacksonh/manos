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


namespace Manos.Http {
	
	public enum HttpMethod {
		ERROR = -1,
		
		HTTP_DELETE,
		HTTP_GET,
		HTTP_HEAD,
		HTTP_POST,
		HTTP_PUT,
		HTTP_CONNECT,
		HTTP_OPTIONS,
		HTTP_TRACE,
		HTTP_COPY,
		HTTP_LOCK,
		HTTP_MKCOL,
		HTTP_MOVE,
		HTTP_PROPFIND,
		HTTP_PROPPATCH,
		HTTP_UNLOCK,
		HTTP_REPORT,
		HTTP_MKACTIVITY,
		HTTP_CHECKOUT,
		HTTP_MERGE,
	}

	public static class HttpMethodBytes {

		private static object lock_obj = new object ();
		private static Dictionary<HttpMethod,byte[]> methods = new Dictionary<HttpMethod,byte[]> ();

		static HttpMethodBytes ()
		{
			lock (lock_obj) {
				foreach (HttpMethod m in Enum.GetValues (typeof (HttpMethod))) {
					Init (m);
				}
			}
		}

		public static void Init (HttpMethod method)
		{
			methods [method] = Encoding.ASCII.GetBytes (method.ToString ().Substring (5));
		}

		// TODO: This is good enough for now, but we shouldn't be allocing
		public static byte [] GetBytes (HttpMethod method)
		{
			byte [] bytes;
			if (!methods.TryGetValue (method, out bytes))
				return null;
			return bytes;
		}

	}
}

