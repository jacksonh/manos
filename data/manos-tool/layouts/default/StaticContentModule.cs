

using System;
using System.IO;

using Mango;


//
//  This the default StaticContentModule that comes with all Mango apps
//  if you do not wish to serve any static content with Mango you can
//  remove its route handler from <YourApp>.cs's constructor and delete
//  this file.
//
//  All Content placed on the Content/ folder should be handled by this
//  module.
//

namespace $APPNAME {

	public class StaticContentModule : MangoModule {

		public StaticContentModule ()
		{
			Get ("*", Content);

		}

		public static void Content (IMangoContext ctx)
		{
			string path = ctx.Request.LocalPath;

			if (path.StartsWith ("/"))
				path = path.Substring (1);

			if (File.Exists (path)) {
				ctx.Response.SendFile (path);
			} else
				ctx.Response.StatusCode = 404;
		}
	}
}

