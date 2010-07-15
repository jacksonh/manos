


using System;
using System.Collections.Generic;
using Mango.Templates.Minge;



public class MingeDemo {

	public static void Main ()
	{
		MingeEnvironment env = new MingeEnvironment ("", new string [] { "demos/kayak/templates/" });
		MingeContext manager = new MingeContext (env);

		Console.WriteLine ("template directory needs update:  {0}", manager.CheckForUpdates ());

		manager.CompileTemplates ();
		manager.LoadTemplates ();

		manager.RenderToStream ("numbers.html", Console.Out, new Dictionary<string,object> ());
	}

	public int Foobar {
		private get;
		set;
	}
}

