
using System;
using System.IO;
using System.Text;

using Mango.Server;


public class T {

	public static void Main ()
	{
	

		HttpServer server = new HttpServer (HandleRequest);
		server.Bind (8080);
		server.Start ();
		server.IOLoop.Start ();
	}

	public static void HandleRequest (HttpConnection con)
	{
		string message = String.Format ("You requested {0}\n", con.Request.Path);

		string fullpath = con.Request.Path;

		if (fullpath.Length > 0) {
			string path = fullpath.Substring (1);

			int query = path.IndexOf ('?');
			if (query > 0)
				path = path.Substring (0, query);

			Console.WriteLine ("PATH:  {0}", path);
			if (File.Exists (path)) {
				con.Response.StatusCode = 200;
				con.Response.SendFile (path);
			} else
				con.Response.StatusCode = 404;
			
		}

		// con.Response.Write (String.Format ("HTTP/1.1 200 OK\r\nContent-Length: {0}\r\n\r\n{1}", Encoding.ASCII.GetBytes (message).Length, message));
		con.Response.Finish ();
	}
}

