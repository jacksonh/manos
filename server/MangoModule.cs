

using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

using Mango.Server;
using Mango.Templates.Minge;

namespace Mango {

	// GET, HEAD, POST, PUT, DELETE, TRACE
	public class MangoModule {

		private Dictionary<string,MangoAction> paths = new Dictionary<string,MangoAction> ();

		public virtual void OnStartup ()
		{
		}

		public virtual void OnShutdown ()
		{
		}

		public void Route (string route, MangoModule module)
		{
		}

		public void Route (string route, MangoAction action)
		{
		}

		public void Get (string route, MangoAction action)
		{
			paths.Add (route, action);
		}

		public void Get (string route, MangoModule module)
		{
		}

		public void Head (string route, MangoAction action)
		{
		}

		public void Head (string route, MangoModule module)
		{
		}

		public void Post (string route, MangoAction action)
		{
		}

		public void Post (string route, MangoModule module)
		{
		}

		public void Put (string route, MangoAction action)
		{
		}

		public void Put (string route, MangoModule module)
		{
		}

		public void Delete (string route, MangoAction action)
		{
		}

		public void Delete (string route, MangoModule module)
		{
		}

		public void Trace (string route, MangoAction action)
		{
		}

		public void Trace (string route, MangoModule module)
		{
		}

		public void HandleConnection (HttpConnection con)
		{
			string path = con.Request.LocalPath.Substring (1);

			Console.WriteLine ("got a connection:  {0}", path);
			foreach (string p in paths.Keys) {
				Console.WriteLine ("checking:  {0}", p);
				if (p == path) {
					MangoAction a = paths [p];

					Console.WriteLine ("executing action:  " + a);
					a (new MangoContext (con));
				}
			}

			con.Response.Finish ();
		}

		public static void RenderTemplate (MangoContext context, string template, object data)
		{
			MemoryStream stream = new MemoryStream ();

			using (StreamWriter writer = new StreamWriter (stream)) {
				Mango.Templates.Minge.Templates.RenderToStream (template, writer, data);
				writer.Flush ();
			}

			byte [] d = stream.GetBuffer ();
			Console.WriteLine ("data: ({0})", d.Length);
			foreach (byte b in d) {
				Console.Write ((char) b);
			}
			Console.WriteLine ();
			context.Response.Write (stream.GetBuffer ());
		}
	}
}

