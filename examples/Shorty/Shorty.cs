
using Manos;
using Manos.Collections;

using System;
using System.Text;
using System.Threading;
using System.Security.Cryptography;



namespace Shorty {

	public class Shorty : ManosApp {

		public Shorty ()
		{
		}

		[Route ("/", "/Home", "/Index")]
		public void Index (IManosContext ctx)
		{
			ctx.Response.WriteLine (@"<html>
                                   <head><title>Welcome to Shorty</title></head>
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

			Cache [id] = new LinkData (link);
			ctx.Response.Redirect ("/r/" + id + "~");
		}

		[Route ("/r/{id}~")]
		public void LinkInfo (IManosContext ctx, Shorty app, string id)
		{
			LinkData info = Cache [id] as LinkData;

			if (info == null) {
				ctx.Response.StatusCode = 404;
				return;
			}

			ctx.Response.WriteLine (@"<html>
                                     <head><title>Welcome to Shorty</title></head>
                                     <body>
                                      <a href='{0}'>{0}</a> was clicked {1} times.
                                     </body>
				    </html>", info.Link, info.Clicks);
		}

		[Route ("/r/{id}")]
		public void Redirector (IManosContext ctx, Shorty app, string id)
		{
			if (ctx.Request.Cookies.Get ("show_info") != null) {
				LinkInfo (ctx, app, id);
				return;
			}

			LinkData info = Cache [id] as LinkData;
			if (info == null) {
				ctx.Response.StatusCode = 404;
				return;
			}

			//
			// Because multiple http transactions could be occuring at the
			// same time, we need to make sure this shared data is incremented
			// properly
			//
			Interlocked.Increment (ref info.Clicks);

			ctx.Response.Redirect (info.Link);
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
