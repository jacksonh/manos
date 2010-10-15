
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;

using Manos;
using Manos.Server;


namespace Manos.Tool
{
	public class ServerCommand
	{
		private ManosApp app;
		
		private int? port;
		private string application_assembly;
		
		public ServerCommand (Environment env) : this (env, new List<string> ())
		{
		}
		
		public ServerCommand (Environment env, IList<string> args)
		{
			Environment = env;
			Arguments = args;
		}
		
		public Environment Environment {
			get;
			private set;
		}
		
		public IList<string> Arguments {
			get;
			set;
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

			Console.WriteLine ("Running {0} on port {1}.", app, Port);
			AppHost.Port = Port;
			AppHost.Start (app);
		}
		
		public ManosApp LoadLibrary (string library)
		{
			Assembly a = Assembly.LoadFrom (library);

			foreach (Type t in a.GetTypes ()) {
				if (t.BaseType == typeof (ManosApp)) {
					if (app != null)
						throw new Exception ("Library contains multiple apps.");
					app = CreateAppInstance (t);
				}
			}

			return app;
		}
		
		public ManosApp CreateAppInstance (Type t)
		{
			int arg_count = Arguments.Count;
			ConstructorInfo [] constructors = t.GetConstructors ();
			
			foreach (ConstructorInfo ci in constructors.Where (c => c.GetParameters ().Count () == arg_count)) {
				object [] args = ArgsForParams (ci.GetParameters ());
				if (args == null)
					continue;
				try {
					return (ManosApp) Activator.CreateInstance (t, args);
				} catch (Exception e) {
					Console.Error.WriteLine ("Exception creating App Type: '{0}'.", t);
					Console.Error.WriteLine (e);
				}
			}
			
			return null;
		}
		
		public object [] ArgsForParams (ParameterInfo [] prms)
		{
			object [] res = new object [prms.Length];
			
			for (int i = 0; i < prms.Count (); i++) {
				try {
					res [i] = Convert.ChangeType (Arguments [i], prms [i].ParameterType);
				} catch (Exception e) {
					Console.Error.WriteLine ("Exception converting type: '{0}'.", prms [i].ParameterType);
					Console.Error.WriteLine (e);
					
					return null;
				}
			}
			
			return res;
		}
	}
}
