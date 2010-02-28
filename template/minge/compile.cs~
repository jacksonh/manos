

using System;
using System.IO;

using Mono.Cecil;

namespace Mango.Templates.Minge {

	public class Tester {

		public static int Main (string [] args)
		{
			if (args.Length < 2)
				return Usage ();

			Environment environment = new Environment ();
			Application app = new Application (environment, "minge.test", args [0]);
			MingeParser p = new MingeParser (environment, app);

			for (int i = 1; i < args.Length; i++) {
				using (TextReader tr = new StreamReader (File.OpenRead ("template2.html"))) {
					p.ParsePage (args [i], tr);
				}
			}

			app.Save ();

			return 0;
		}

		public static int Usage ()
		{
			Console.WriteLine ("compile template-assembly template[,...]");
			return 1;
		}
	}
}

