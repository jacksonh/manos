
using Manos;


namespace Logger {

	public class Logger : ManosApp {

		public Logger ()
		{
			Route ("/", ctx => ctx.Response.Write ("Hello, World."));

			AddPipe (new Manos.Util.AccessLogger ("access.log"));
		}
	}
}
