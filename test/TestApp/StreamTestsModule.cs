

using Manos;

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
	}
}

