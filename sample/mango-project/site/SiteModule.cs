
using System;

using Mango;


namespace MangoProject.Site {

	public class SiteModule : MangoModule {

		public override void OnStartup ()
		{
			Get ("$", Home);
			Get ("Home/$", Home);
			Get ("About/$", About);
			Get ("Download/$", Download);
		}

		public static void Home (MangoContext context)
		{

		}

		public static void About (MangoContext context)
		{
		}

		public static void Download (MangoContext context)
		{
		}
	}
}


