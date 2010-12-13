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
using Manos.Collections;

using System;
using System.Text;
using System.Security.Cryptography;



namespace Shorty {

	public class Shorty : ManosApp {

		public Shorty ()
		{
		}

		[Route ("/", "/Home", "/Index")]
		public void Index (IManosContext ctx)
		{
			ctx.Response.End (@"<html>
                                   <head><title>" + ManosConfig.Get ("shorty.title") + @"</title></head>
                                   <body>
                                    <form method='POST' action='submit-link'>
                                     <input type='text' name='link' /><br />
                                     <input type='checkbox' name='show_info' /> Show me the info page, instead of redirecting.<br />
                                     <input type='submit' />
                                    </form>
                                   </body>
				  </html>");
		}

		[Post ("/submit-link")]
		public void SubmitLink (IManosContext ctx, Shorty app, string link, bool show_info)
		{
			string id = GenerateHash (link, 5);

			if (show_info)
				ctx.Response.SetCookie ("show_info", "true");

			Cache.Set (id, new LinkData (link), (name, item) => {
				ctx.Response.Redirect ("/r/" + id + "~");
			});
		}

		[Route ("/r/{id}~")]
		public void LinkInfo (IManosContext ctx, Shorty app, string id)
		{
			Cache.Get (id, (name, item) => {
				LinkData info = item as LinkData;

				if (info == null) {
					ctx.Response.StatusCode = 404;
					ctx.Response.End ();
					return;
				}

				ctx.Response.End (@"<html>
                                     	<head><title>Welcome to Shorty</title></head>
                                     	<body>
                                     	 <a href='{0}'>{0}</a> was clicked {1} times.
                                    	 </body>
				   	 </html>", info.Link, info.Clicks);
			});
		}

		[Route ("/r/{id}")]
		public void Redirector (IManosContext ctx, Shorty app, string id)
		{
			if (ctx.Request.Cookies.Get ("show_info") != null) {
				LinkInfo (ctx, app, id);
				return;
			}

			Cache.Get (id, (name, item) => {
				LinkData info = item as LinkData;
				if (info == null) {
					ctx.Response.StatusCode = 404;
					return;
				}

				++info.Clicks;
				ctx.Response.Redirect (info.Link);
			});
		}

		private static string GenerateHash (string str, int length)
		{
			byte [] data = Encoding.Default.GetBytes (str);

			SHA1 sha = new SHA1CryptoServiceProvider (); 
			data = sha.ComputeHash (data);

			string base64 = Convert.ToBase64String (data);

			int i = 0;
			StringBuilder result = new StringBuilder ();
			while (result.Length < length) {
				if (Char.IsLetterOrDigit (base64 [i]))
					result.Append (base64 [i]);
				++i;
				if (i >= base64.Length)
					return null;
			}

			return result.ToString ();
		}

	}
}
