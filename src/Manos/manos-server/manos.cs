
using System;
using System.IO;
using System.Text;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;

using Manos;
using Manos.Server;


public class T {

	public static bool show_headers;

	public static ManosApp app;

	public static void Main (string [] args)
	{
		show_headers = false;

		if (args.Length > 0)
			app = LoadLibrary (args [0]);

		AppHost.Start (app);
	}

	public static ManosApp LoadLibrary (string library)
	{
		Assembly a = Assembly.LoadFrom (library);

		foreach (Type t in a.GetTypes ()) {
			if (t.BaseType == typeof (ManosApp)) {
				if (app != null)
					throw new Exception ("Library contains multiple apps.");
				app = (ManosApp) Activator.CreateInstance (t);
			}
		}

		Console.WriteLine ("running app:  {0}", app);
		return app;
	}
}

