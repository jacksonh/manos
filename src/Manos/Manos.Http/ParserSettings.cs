//
// Based on http_parser.java: http://github.com/a2800276/http-parser.java
// which is based on http_parser: http://github.com/ry/http-parser
//
//
// Copyright 2009,2010 Ryan Dahl <ry@tinyclouds.org>
// Copyright (C) 2010 Tim Becker 
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
using System.IO;

using Manos.Collections;

namespace Manos.Http {


	public class ParserSettings {
	
		public HttpCallback       OnMessageBegin;
		public HttpDataCallback   OnPath;
		public HttpDataCallback   OnQueryString;
		public HttpDataCallback   OnUrl;
		public HttpDataCallback   OnFragment;
		public HttpDataCallback   OnHeaderField;
		public HttpDataCallback   OnHeaderValue;
		public HttpCallback       OnHeadersComplete;
		public HttpDataCallback   OnBody;
		public HttpCallback       OnMessageComplete;
		public HttpErrorCallback  OnError;


		public void RaiseOnMessageBegin (HttpParser p)
		{
			Raise (OnMessageBegin, p);
		}

		public void RaiseOnMessageComplete (HttpParser p)
		{
			Raise (OnMessageComplete, p);
		}
  
		// this one is a little bit different:
		// the current `position` of the buffer is the location of the
		// error, `ini_pos` indicates where the position of
		// the buffer when it was passed to the `execute` method of the parser, i.e.
		// using this information and `limit` we'll know all the valid data
		// in the buffer around the error we can use to print pretty error
		// messages.

		public void RaiseOnError (HttpParser p, string message, ByteBuffer buf, int ini_pos)
		{
			if (null != OnError)
				OnError (p, message, buf, ini_pos);

			
			// if on_error gets called it MUST throw an exception, else the parser 
			// will attempt to continue parsing, which it can't because it's
			// in an invalid state.
			Console.WriteLine ("ERROR: '{0}'", message);
			throw new HttpException (message);
		}

		public void RaiseOnHeaderField (HttpParser p, ByteBuffer buf, int pos, int len)
		{
			Raise (OnHeaderField, p, buf, pos, len);
		}

		public void RaiseOnQueryString (HttpParser p, ByteBuffer buf, int pos, int len)
		{
			Raise (OnQueryString, p, buf, pos, len);
		}

		public void RaiseOnFragment (HttpParser p, ByteBuffer buf, int pos, int len)
		{
			Raise (OnFragment, p, buf, pos, len);
		}

		public void RaiseOnPath (HttpParser p, ByteBuffer buf, int pos, int len)
		{
			Raise (OnPath, p, buf, pos, len);
		}

		public void RaiseOnHeaderValue (HttpParser p, ByteBuffer buf, int pos, int len)
		{
			Raise (OnHeaderValue, p, buf, pos, len);
		}

		public void RaiseOnUrl (HttpParser p, ByteBuffer buf, int pos, int len)
		{
			Raise (OnUrl, p, buf, pos, len);
		}

		public void RaiseOnBody(HttpParser p, ByteBuffer buf, int pos, int len)
		{
			Raise (OnBody, p, buf, pos, len);
		}

		public int RaiseOnHeadersComplete (HttpParser p)
		{
			return Raise (OnHeadersComplete, p);
		}

		private int Raise (HttpCallback cb, HttpParser p)
		{
			if (cb == null)
				return 0;

			try {
				return cb (p);
			} catch (Exception e) {
				Console.WriteLine (e);

				RaiseOnError (p, e.Message, null, -1);
				return -1;
			}
		}

		private int Raise (HttpDataCallback cb, HttpParser p, ByteBuffer buf, int pos, int len)
		{
			if (cb == null || pos == -1)
				return 0;

			try {
				return cb (p,buf,pos,len);
			} catch (Exception e) {
				Console.WriteLine (e);

				RaiseOnError (p, e.Message, buf, pos);
				return -1;
			}
				
		}
	}
}
