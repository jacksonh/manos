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



using Manos;
using Manos.Server;

using System;
using System.Text;


namespace TestApp {

	public class StreamTestsModule : ManosModule {

		public void EchoLocalPath (IManosContext ctx)
		{
			ctx.Response.Write (ctx.Request.LocalPath);
		}

		public void EchoInt (IManosContext ctx, int the_int)
		{
			ctx.Response.Write (the_int.ToString ());
		}

		public void EchoString (IManosContext ctx, string the_string)
		{
			ctx.Response.Write (the_string);
		}

		public void ReverseString (IManosContext ctx, string the_string)
		{
			// This is intentionally awful, as its trying to test the stream 
			// Write it normally, then try to overwrite it with the new value

			ctx.Response.Write (the_string);
			ctx.Response.Stream.Position = 0;

			byte [] data = Encoding.Default.GetBytes (the_string);

			for (int i = data.Length; i >= 0; --i) {
				ctx.Response.Stream.Write (data, i, 1);
			}
		}

		public void MultiplyString (IManosContext ctx, string the_string, int amount)
		{
			// Create all the data for the string, then do all the writing
			
			long start = ctx.Response.Stream.Position;
			ctx.Response.Write (the_string);

			long len = ctx.Response.Stream.Position - start;
			ctx.Response.Stream.SetLength (len * amount);
			
			ctx.Response.Stream.Position = start;
			for (int i = 0; i < amount; i++) {
				ctx.Response.Write (the_string);
			}
		}

		public void SendFile (IManosContext ctx, string name)
		{
			ctx.Response.SendFile (name);
		}

		public void UploadFile (IManosContext ctx, string name)
		{
			UploadedFile file = ctx.Request.Files [name];
			byte [] data = new byte [file.Contents.Length];

			file.Contents.Read (data, 0, data.Length);

			ctx.Response.Write (data);
		}
	}
}

