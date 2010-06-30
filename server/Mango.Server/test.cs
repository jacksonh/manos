
using Mango.Server;


public class T {

	public static void Main ()
	{
	

		HttpServer server = new HttpServer (HandleRequest);
		server.Bind (8081);
		server.Start ();
		server.IOLoop.Start ();
	}

	public static void HandleRequest (HttpRequest request)
	{

	}
}

