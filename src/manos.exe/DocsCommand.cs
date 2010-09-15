
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
	public class DocsCommand
	{				
		int? port;
		
		public DocsCommand (Environment env)
		{
			Environment = env;
		}
		
		public Environment Environment {
			get;
			private set;
		}
		
		public string DocsDirectory {
			get {
				return Path.Combine (Environment.DataDirectory, "docs");	
			}
		}
		
		public int Port {
			get { 
				if (port == null)
					return 8181;
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
			DocsModule docs = new DocsModule (DocsDirectory);
			Console.WriteLine ("Go to http://localhost:{0}/ to see your docs.", Port);
			
			AppHost.Port = Port;
			AppHost.Start (docs);
		}
	}
}
