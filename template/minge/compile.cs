

using System;
using System.IO;

using Mono.Cecil;

namespace Mango.Templates.Minge {

	public class Tester {

		public static int Main (string [] args)
		{
			if (args.Length < 2)
				return Usage ();

//			Environment environment = new Environment ();
//			Application app = new Application (environment, "minge.test", args [0]);
//			MingeParser p = new MingeParser (environment, app);

			Templates.Initialize ("test/");

			Templates.RenderToStream ("about.html", Console.Out, new Tester ());

			return 0;
		}

		public string Message {
			get { return "THIS IS THE MESSSAGE MON"; }
		}

		public static int Usage ()
		{
			Console.WriteLine ("compile template-assembly template[,...]");
			return 1;
		}
	}
}

