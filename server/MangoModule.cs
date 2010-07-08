

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

		public void Route (MangoModule module, params string [] route)
		{
		}

		public void Route (MangoAction action, params string [] route)
		{
		}

		public void Get (MangoAction action, params string [] route)
		{
			paths.Add (route [0], action);
		}

		public void Get (MangoModule module, params string [] route)
		{
		}

		public void Head (MangoAction action, params string [] route)
		{
		}

		public void Head (MangoModule module, params string [] route)
		{
		}

		public void Post (MangoAction action, params string [] route)
		{
		}

		public void Post (MangoModule module, params string [] route)
		{
		}

		public void Put (MangoAction action, params string [] route)
		{
		}

		public void Put (MangoModule module, params string [] route)
		{
		}

		public void Delete (MangoAction action, params string [] route)
		{
		}

		public void Delete (MangoModule module, params string [] route)
		{
		}

		public void Trace (MangoAction action, params string [] route)
		{
		}

		public void Trace (MangoModule module, params string [] route)
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

