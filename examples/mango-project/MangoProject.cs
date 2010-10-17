//
// Copyright (C) 2010 Jackson Harper (jackson@manosdemono.com)
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//
//



using Mango;


//
//  This is a sample file, demonstrating what its like to develop
//  web applications with Mango.
//

namespace MangoProject {

	//
	// A mango application is made of MangoModules and MangoApps
	// There really isn't any difference between a Module and an App
	// except that the server will load the MangoApp first. A mango
	// application can only have one MangoApp, but as many MangoModules
	// as you want.
	// 
	public class MangoProject : MangoApp {

		public MangoProject ()
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

		/// blah, blah, blah
	}

}


