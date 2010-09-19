
using Manos;


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
                                     <input type='text' name='link' />
                                     <input type='submit' />
                                    </form>
                                   </body>");
		}

		[Post ("/submit-link")]
		public void SubmitLink (IManosContext ctx, Shorty app, string link)
		{
			string id = GenerateHash (link, 5);

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
                                    {0} was clicked {1} times.
                                   </body>", info.Link, info.Clicks);
		}

		[Route ("/r/{id}")]
		public void Redirector (IManosContext ctx, Shorty app, string id)
		{
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
