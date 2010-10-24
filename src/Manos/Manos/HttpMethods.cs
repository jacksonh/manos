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

using Manos.Http;

namespace Manos {

	public static class HttpMethods {

		public static readonly HttpMethod [] GetMethods = new HttpMethod [] { HttpMethod.HTTP_GET };
		public static readonly HttpMethod [] HeadMethods = new HttpMethod [] { HttpMethod.HTTP_HEAD };
		public static readonly HttpMethod [] PostMethods = new HttpMethod [] { HttpMethod.HTTP_POST };
		public static readonly HttpMethod [] PutMethods = new HttpMethod [] { HttpMethod.HTTP_PUT };
		public static readonly HttpMethod [] DeleteMethods = new HttpMethod [] { HttpMethod.HTTP_DELETE };
		public static readonly HttpMethod [] TraceMethods = new HttpMethod [] { HttpMethod.HTTP_TRACE };
		public static readonly HttpMethod [] OptionsMethods = new HttpMethod [] { HttpMethod.HTTP_OPTIONS };
		
		public static readonly HttpMethod [] RouteMethods = new HttpMethod [] { HttpMethod.HTTP_GET,
											HttpMethod.HTTP_PUT,
											HttpMethod.HTTP_POST,
											HttpMethod.HTTP_HEAD,
											HttpMethod.HTTP_DELETE,
											HttpMethod.HTTP_TRACE,
											HttpMethod.HTTP_OPTIONS };

	}
}

