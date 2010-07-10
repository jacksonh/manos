

using Mango;


namespace MangoProject {

	//
	// A mango application is made of MangoModules and MangoApps
	// There really isn't any difference between a Module and an App
	// except that the server will load the MangoApp first. A mango
	// application can only have one MangoApp, but as many MangoModules
	// as you want.
	// 
	public class MangoProject : MangoApp {

		//
		// This method is called when the server first starts,
		//
		public override void OnStartup ()
		{
			//
			// Routing can be done by using the Get/Head/Post/Put/Delete/Trace
			// methods. They take a regular expression and either a method
			// or another module to hand off processing to.
			//
			// The regular expressions come after the method/module, because
			// Mango uses params to allow you to easily register multiple
			// regexs to the same handler
			//
			Get (Home, "Home");

			//
			// The Route method will hand off all HTTP methods to the supplied
			// method or module
			//
			Route (new DocsModule (), "Docs/");
		}

		//
		// Handlers can be registered with attributes too
		//

		[Get ("About")]
		public static void About (MangoContext ctx)
		{
			//
			// Templates use property lookup, so you can pass in
			// a strongly typed object, or you can use an anonymous
			// type.
			//
			RenderTemplate (ctx, "about.html", new {
				Title = "About",
				Message = "Hey, I am the about page.",
			});
		}

		public static void Home (MangoContext ctx)
		{
			RenderTemplate (ctx, "home.html", new {
				Title = "Home",
				Message = "Welcome to the Mango-Project."
			});
		}
	}

	//
	// Normally a different module would be in a different file, but for
	// demo porpoises we'll do it here.
	//

	public class AdminModule : MangoModule {

		public override void OnStartup ()
		{
		}

		/// blah, blah, blah
	}

}


