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
	
	/// <summary>
	/// Relevent information related to the request initiated by an HTTP client.
	/// </summary>
	/// <remarks>
	/// Similar in concept to HttpContext under the ASP.Net stack.
	/// </remarks>
	public class ManosContext : IManosContext {

		public ManosContext (IHttpTransaction transaction)
		{
			Transaction = transaction;
		}
		
		/// <summary>
		/// Information related to the Environment under which this app is running.
		/// </summary>
		/// <remarks>
		/// This is a cenvenience method that calls Transaction.Server.
		/// </remarks>
		public HttpServer Server {
			get { return Transaction.Server; }
		}
		
		/// <summary>
		/// A single request/response interation between the client and this server.
		/// </summary>
		public IHttpTransaction Transaction {
			get;
			private set;
		}
		
		/// <summary>
		/// Information related to the request initiated by the client.
		/// </summary>
		/// <remarks>
		/// This is a convenience method that calls Transaction.Request.
		/// </remarks>
		public IHttpRequest Request {
			get { return Transaction.Request; }
		}
		
		/// <summary>
		/// Information related to how this server will respond to the client's request.
		/// </summary>
		/// <remarks>
		/// This is a convenience method that calls Transaction.Response.
		/// </remarks>
		public IHttpResponse Response {
			get { return Transaction.Response; }
		}
	}
}
