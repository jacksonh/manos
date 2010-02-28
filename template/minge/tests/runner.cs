
using System;
using System.IO;
using System.Text;
using System.Linq;
using System.Collections.Generic;


namespace Mango.Templates.Minge.Test {

	private static string TEST_PREFIX = "test_";

	public static void Main ()
	{
		string [] tests = Directory.GetFiles (String.Concat (TEST_PREFIX, "*"));

		Environment environment = new Environment ();
		Application app = new Application (environment, "minge.test", args [0]);
		MingeParser p = new MingeParser (environment, app);

		foreach (string test in tests) {
			using (TextReader tr = new StreamReader (File.OpenRead (test))) {
				p.ParsePage (test, tr);
			}
		}

		app.Save ();

	}
}


