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


namespace Manos {

	public static class HttpMethods {

		public static readonly string [] GetMethods = new string [] { "GET" };
		public static readonly string [] HeadMethods = new string [] { "HEAD" };
		public static readonly string [] PostMethods = new string [] { "POST" };
		public static readonly string [] PutMethods = new string [] { "PUT" };
		public static readonly string [] DeleteMethods = new string [] { "DELETE" };
		public static readonly string [] TraceMethods = new string [] { "TRACE" };
		public static readonly string [] OptionsMethods = new string [] { "OPTIONS" };
		
		public static readonly string [] RouteMethods = new string [] { "GET", "PUT", "POST", "HEAD", "DELETE", "TRACE", "OPTIONS" };

	}
}

