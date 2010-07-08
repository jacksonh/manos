

using System;
using System.IO;

namespace Mango.Templates.Minge {

	public class Tester {

		public static int Main (string [] args)
		{
			if (args.Length < 1)
				return Usage ();

			MingeEnvironment environment = new MingeEnvironment (args);
			MingeCompiler compiler = new MingeCompiler (environment);

			compiler.CompileTemplates ();

			return 0;
		}

		public static int Usage ()
		{
			Console.WriteLine ("compile template_dir[,...]");
			return 1;
		}
	}
}

