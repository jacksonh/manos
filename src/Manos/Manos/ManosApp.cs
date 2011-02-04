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
using Manos.Routing;

namespace Manos {
	
	/// <summary>
	/// The entry point for your manos app. Derive from this class one time in your manos app and it will get instantiated when the runtime executes.
	/// </summary>
	/// <remarks>
	/// This is similar in concept to the HttpApplication in the ASP.Net stack.
	/// </remarks>
	public class ManosApp : ManosModule {
		
		public ManosApp ()
		{
			ManosConfig.Load (this);
		}

		public void HandleTransaction (ManosApp app, IHttpTransaction con)
		{
			Pipeline pipeline = new Pipeline (app, con);

			pipeline.Begin ();
		}
	}
}

