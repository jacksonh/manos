
using Manos;


namespace Logger {

	public class Logger : ManosApp {

		public Logger ()
		{
			Route ("/", ctx => ctx.Response.Write ("Hello, World."));

			AppHost.Pipes.Add (new Manos.Util.AccessLogger ("access.log"));
		}
	}
}
