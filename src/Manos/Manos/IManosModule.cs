//
// Copyright (C) 2011 Antony Pinchbeck (antony@componentx.co.uk)
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
using Manos.Caching;
using Manos.Logging;

namespace Manos
{
	public interface IManosModule 
	{
		RouteHandler Routes { get; }
		IManosCache Cache { get; }
		IManosLogger Log { get; }
		
		RouteHandler Route (string pattern, IManosModule module);
		RouteHandler Route (string pattern, ManosAction action);
		RouteHandler Route (string pattern, Manos.Routing.MatchType matchType, ManosAction action);
		RouteHandler Route (ManosAction action, params string [] patterns);
		RouteHandler Route (IManosModule module, params string [] patterns);
		
		RouteHandler Get (string pattern, IManosModule module);
		RouteHandler Get (string pattern, ManosAction action);
		RouteHandler Get (string pattern, Manos.Routing.MatchType matchType, ManosAction action);
		RouteHandler Get (ManosAction action, params string [] patterns);
		RouteHandler Get (IManosModule module, params string [] patterns);
		
	 	RouteHandler Put (string pattern, IManosModule module);
		RouteHandler Put (string pattern, ManosAction action);
		RouteHandler Put (string pattern, Manos.Routing.MatchType matchType, ManosAction action);
		RouteHandler Put (ManosAction action, params string [] patterns);
		RouteHandler Put (IManosModule module, params string [] patterns);
		
		RouteHandler Post (string pattern, IManosModule module);
		RouteHandler Post (string pattern, ManosAction action);
		RouteHandler Post (string pattern, Manos.Routing.MatchType matchType, ManosAction action);
		RouteHandler Post (ManosAction action, params string [] patterns);
		RouteHandler Post (IManosModule module, params string [] patterns);
		
		RouteHandler Delete (string pattern, IManosModule module);
		RouteHandler Delete (string pattern, ManosAction action);
		RouteHandler Delete (string pattern, Manos.Routing.MatchType matchType, ManosAction action);
		RouteHandler Delete (ManosAction action, params string [] patterns);
		RouteHandler Delete (IManosModule module, params string [] patterns);
		
		RouteHandler Head (string pattern, IManosModule module);
		RouteHandler Head (string pattern, ManosAction action);
		RouteHandler Head (string pattern, Manos.Routing.MatchType matchType, ManosAction action);
		RouteHandler Head (ManosAction action, params string [] patterns);
		RouteHandler Head (IManosModule module, params string [] patterns);
		
		RouteHandler Options (string pattern, IManosModule module);
		RouteHandler Options (string pattern, ManosAction action);
		RouteHandler Options (string pattern, Manos.Routing.MatchType matchType, ManosAction action);
		RouteHandler Options (ManosAction action, params string [] patterns);
		RouteHandler Options (IManosModule module, params string [] patterns);
		
		RouteHandler Trace (string pattern, IManosModule module);
		RouteHandler Trace (string pattern, ManosAction action);
		RouteHandler Trace (string pattern, Manos.Routing.MatchType matchType, ManosAction action);
		RouteHandler Trace (ManosAction action, params string [] patterns);
		RouteHandler Trace (IManosModule module, params string [] patterns);

		void StartInternal ();
	}
}
