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
using Manos.Http.Testing;
using Manos.Collections;
using System.Collections.Generic;

namespace Manos.Testing
{
	public static class DataDictionaryExtension
	{
		public static void CopyCookies (this DataDictionary reqCookies, Dictionary<string, HttpCookie> resCookie)
		{
			foreach (string key in resCookie.Keys) {
				// get the value - not sure how this will cope with multi value cookie
				reqCookies[key] = resCookie[key].Values[key];
			}
		}
	}
	
	public class With
	{
		DataDictionary _dict;
		MockHttpRequest _request;
		
		public With (MockHttpRequest request, DataDictionary dict)
		{
			_request = request;
			_dict = dict;
		}
		
		public void Parameter (string name, string value)
		{
			_dict[name] = value;
		}
		
		public void Header (string name, string value)
		{
			_request.Headers.SetHeader (name, value);
		}
		
		public void Cookie (string name, string value)
		{
			_request.Cookies[name] = value;
		}

		public void PostBody (string value)
		{
			_request.PostBody = value;
		}
	}
	
	public class ManosBrowser
	{
		public MockHttpRequest Request { get; private set; }
		public MockHttpResponse Response { get; private set; }
		public MockHttpTransaction Transaction { get; private set; }
		
		private ManosApp _app;

		public ManosBrowser (ManosApp app)
		{
			_app = app;
			_app.StartInternal ();
			
			Request = new MockHttpRequest ();
			Response = new MockHttpResponse ();
			Transaction = new MockHttpTransaction (Request, Response);
		}
		
		public void MakeRequest ()
		{
			// copy cookies
			Request.Cookies.CopyCookies (Response.Cookies);
			
			_app.HandleTransaction (_app, Transaction);
			
			Request.Reset ();
		}
		
		public void Get (string url)
		{
			Get (url, null);
		}
			
		public void Get (string url, Action<With> fn)
		{
			Request.Method = HttpMethod.HTTP_GET;
			Request.Path = url;
			
			if (null != fn) {
				With w = new With (Request, Request.QueryData);
				fn (w);
			}
			
			MakeRequest ();
		}
				
		public void Post (string url)
		{
			Post (url, null);
		}
		
		public void Post (string url, Action<With> fn)
		{
			Request.Method = HttpMethod.HTTP_POST;
			Request.Path = url;
			
			if (null != fn) {
				With w = new With (Request, Request.PostData);
				fn (w);
			}
			
			MakeRequest ();
		}
		
		public int StatusCode
		{
			get { return Response.StatusCode; }
		}
		
		public string ResponseString {
			get { return Transaction.ResponseString; }
		}
		
		public string RedirectedUrl {
			get { return Response.RedirectedUrl; }
		}
		
		public string ContentType {
			get { 
				string value;
				
				if(Response.Headers.TryGetValue("Content-Type", out value)) {
					return value;
				}
				
				return string.Empty;
			}
		}
	}
}
