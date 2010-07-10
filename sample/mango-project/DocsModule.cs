
using System;


using Mango;


namespace MangoProject {

	public class DocsModule : MangoModule {

		public override void OnStartup ()
		{
			Get (GettingStarted, "GettingStarted");
			Get (Api, "Api");
		}

		public static void GettingStarted (MangoContext context)
		{
		}

		public static void Api (MangoContext context)
		{
		}
	}
}

