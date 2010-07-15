
using System;


using Mango;


namespace MangoProject {

	public class DocsModule : MangoModule {

		public DocsModule ()
		{
			Get (GettingStarted, "GettingStartedFool");
			Get (Api, "Api");
		}

		public static void GettingStarted (MangoContext context)
		{
			Console.WriteLine ("running geting started template.");
		}

		public static void Api (MangoContext context)
		{
			Console.WriteLine ("running API template");
		}
	}
}

