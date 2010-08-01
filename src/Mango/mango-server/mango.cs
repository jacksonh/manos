
using System;
using System.IO;
using System.Text;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;

using Mango;
using Mango.Server;


public class T {

	public static bool show_headers;

	public static MangoApp app;

	public static void Main (string [] args)
	{
		show_headers = false;

		if (args.Length > 0)
			app = LoadLibrary (args [0]);

		HttpServer server = new HttpServer (HandleRequest);
		server.Bind (8080);
		server.Start ();
		server.IOLoop.Start ();
	}

	public static MangoApp LoadLibrary (string library)
	{
		Assembly a = Assembly.LoadFrom (library);

		foreach (Type t in a.GetTypes ()) {
			if (t.BaseType == typeof (MangoApp)) {
				if (app != null)
					throw new Exception ("Library contains multiple apps.");
				app = (MangoApp) Activator.CreateInstance (t);
			}
		}

		Console.WriteLine ("running app:  {0}", app);
		return app;
	}

	public static void HandleRequest (IHttpTransaction con)
	{
		string message = String.Format ("You requested {0}\n", con.Request.ResourceUri);

		string fullpath = con.Request.LocalPath;

		if (show_headers) {
			Console.WriteLine ("HEADERS:");
			Console.WriteLine ("===========");
			foreach (string h in con.Request.Headers.Keys) {
				Console.WriteLine ("{0} = {1}", h, con.Request.Headers [h]);
			}
			Console.WriteLine ("===========");
		}

		if (app != null) {
			app.HandleTransaction (con);
			return;
		}

		if (fullpath.Length > 0) {
			string path = fullpath.Substring (1);

			int query = path.IndexOf ('?');
			if (query > 0)
				path = path.Substring (0, query);

			if (File.Exists (path)) {
				con.Response.StatusCode = 200;

				Console.WriteLine ("sending file:  {0}", path);
				con.Response.SendFile (path);
			} else
				con.Response.StatusCode = 404;
			
		}

	}
}

