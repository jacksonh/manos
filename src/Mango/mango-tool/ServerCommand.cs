
using System;
using System.IO;
using System.Reflection;

using Mango;
using Mango.Server;


namespace Mango.Tool
{
	public class ServerCommand
	{
		private MangoApp app;
		
		private int? port;
		private string application_assembly;
		
		public ServerCommand (Environment env)
		{
			Environment = env;
		}
		
		public Environment Environment {
			get;
			private set;
		}
		
		public string ApplicationAssembly {
			get {
				if (application_assembly == null)
					return Path.GetFileName (Directory.GetCurrentDirectory ()) + ".dll";
				return application_assembly;
			}
			set {
				if (value == null)
					throw new ArgumentNullException ("value");
				application_assembly = value;
			}
		}
		
		public int Port {
			get { 
				if (port == null)
					return 8080;
				return (int) port;
			}
			set {
				if (port <= 0)
					throw new ArgumentException ("port", "port must be greater than zero.");
				port = value;	
			}
		}
		
		public void Run ()
		{
			app = LoadLibrary (ApplicationAssembly);

			HttpServer server = new HttpServer (HandleRequest);
			server.Bind (Port);
			server.Start ();
			server.IOLoop.Start ();	
			Console.WriteLine ("loop started.");
		}
		
		public void HandleRequest (IHttpTransaction con)
		{
			app.HandleTransaction (con);
		}
			
		public MangoApp LoadLibrary (string library)
		{
			Assembly a = Assembly.LoadFrom (library);

			foreach (Type t in a.GetTypes ()) {
				if (t.BaseType == typeof (MangoApp)) {
					if (app != null)
						throw new Exception ("Library contains multiple apps.");
					app = (MangoApp) Activator.CreateInstance (t);
				}
			}

			Console.WriteLine ("running app:  {0}", app);
			return app;
		}
	}
}
