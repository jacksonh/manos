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
using System.Reflection;

using Manos.Http;

namespace Manos {
	
	/// <summary>
	/// Indicates that the decorated method should respond to any specified routes when the GET verb is used for the request.
	/// </summary>
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
	public class GetAttribute : HttpMethodAttribute {
		
				
		/// <summary>
		/// Specifies that the decoraetd method should only be invoked when the http GET verb is used.
		/// </summary>
		public GetAttribute ()
		{
			Methods = new HttpMethod[] { HttpMethod.HTTP_GET };
		}
		
		/// <summary>
		/// Specifies that the decorated method should be invoked whenever a GET request matches any of the patterns declared)
		/// </summary>
		/// <param name="patterns">
		/// A <see cref="T:System.String[]"/> of patterns to match
		/// </param>
		public GetAttribute (params string [] patterns) : base (patterns)
		{
			Methods = new HttpMethod [] { HttpMethod.HTTP_GET };
		}
	}
}


