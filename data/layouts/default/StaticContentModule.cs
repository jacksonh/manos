

using System;
using System.IO;

using Manos;
using Manos.Routing;

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

		private string basedir;

		public StaticContentModule ()
		{
			Get (".*", MatchType.Regex, Content);

			basedir = Path.GetFullPath ("Content");
		}

		public void Content (IManosContext ctx)
		{
			string path = ctx.Request.Path;

			if (path.StartsWith ("/"))
				path = path.Substring (1);

			if (ValidFile (path)) {
				ctx.Response.Headers.SetNormalizedHeader ("Content-Type", ManosMimeTypes.GetMimeType (path));
				ctx.Response.SendFile (path);
			} else
				ctx.Response.StatusCode = 404;

			ctx.Response.End ();
		}

		
		private bool ValidFile (string path)
		{
			try {
				string full = Path.GetFullPath (path);
				if (full.StartsWith (basedir))
					return File.Exists (full);
			} catch {
				return false;
			}

			return false;
		}
	}
}

