

using System;
using System.IO;

using Manos;


//
//  This the default StaticContentModule that comes with all Manos apps
//  if you do not wish to serve any static content with Manos you can
//  remove its route handler from <YourApp>.cs's constructor and delete
//  this file.
//
//  All Content placed on the Content/ folder should be handled by this
//  module.
//

namespace $APPNAME {

	public class StaticContentModule : ManosModule {

		public StaticContentModule ()
		{
			Get (".*", Content);

		}

		public static void Content (IManosContext ctx)
		{
			string path = ctx.Request.LocalPath;

			if (path.StartsWith ("/"))
				path = path.Substring (1);

			if (File.Exists (path)) {
				ctx.Response.SendFile (path);
			} else
				ctx.Response.StatusCode = 404;

			ctx.Response.End ();
		}
	}
}

