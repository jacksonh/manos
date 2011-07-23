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
	public interface IManosPipe
	{
		/// <summary>
		///  Called as a request is coming in, before any actions at all have been taken on the
		///  request.  There is not a valid Response object on the transaction yet and the
		///  routing information has not been looked up.
		/// </summary>
		void OnPreProcessRequest (ManosApp app, IHttpTransaction transaction, Action complete);

		/// <summary>
		///  Called after the response object has been created but before any routing has been done
		///  if you would like to override the default routing you can do it here, by changing the
		///  IManosTarget returned in the complete Action.
		/// </summary>
		void OnPreProcessTarget (IManosContext ctx, Action<IManosTarget> changeHandler);

		/// <summary>
		///  The default routing information has been looked up, but the actual action execution
		///  has not occurred yet.
		/// </summary>
		void OnPostProcessTarget (IManosContext ctx, IManosTarget target, Action complete);

		/// <summary>
		///  The action has been invoked and has had its End method called.  This is the final
		///  step in the pipeline.
		/// </summary>
		void OnPostProcessRequest (ManosApp app, IHttpTransaction transaction, Action complete);

		/// <summary>
		///  An error has been raised by Manos. (Currently unimplemented).
		/// </summary>
		void OnError (IManosContext ctx, Action complete);
		
	}
}

