

using System;
using System.IO;

using Mono.Cecil;

namespace Mango.Templates.Minge {

	public class Tester {

		public static void Main ()
		{
			Application app = new Application ("minge.test", "mingetest.dll");
			MingeParser p = new MingeParser (new Environment (), app);

			using (TextReader tr = new StreamReader (File.OpenRead ("template.html"))) {
				p.ParsePage ("template.html", tr);
			}

			app.Save ();
		}
	}
}

