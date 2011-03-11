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

namespace Manos
{
	/// <summary>
	/// ManosPipe provides a mechanism to intercept calls before or after the standard Manos Routing has taking place.
	/// (For example, Gzip compression module could compress content post process)
	/// </summary>
	/// <remarks>
	/// This is similar in concept to the HttpModule in the ASP.Net stack.</remarks>
	public class ManosPipe : IManosPipe
	{
		public ManosPipe ()
		{
		}
			
		public virtual void OnPreProcessRequest (ManosApp app, IHttpTransaction transaction, Action complete)
		{
			complete ();
		}

		public virtual void OnPreProcessTarget (IManosContext ctx, Action<IManosTarget> changeHandler)
		{
			// default: don't change the handler
		}

		public virtual void OnPostProcessTarget (IManosContext ctx, IManosTarget target, Action complete)
		{
			complete ();
		}

		public virtual void OnPostProcessRequest (ManosApp app, IHttpTransaction transaction, Action complete)
		{
			complete ();
		}
		
		public virtual void OnError (IManosContext ctx, Action complete)
		{
			complete ();
		}
	}
}

