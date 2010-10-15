

using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

using NDesk.Options;

namespace Manos.Tool
{
	class Driver
	{
		
		public static readonly string COMPILED_TEMPLATES_ASSEMBLY = "CompiledTemplates.dll";
		public static readonly string TEMPLATES_DIRECTORY = "Templates";
		public static readonly string DEPLOYMENT_DIRECTORY = "Deployment";
		
		private static Environment Environment = new Environment ();
		
		public static int Main (string[] args)
		{
			args = ParseGlobalOptions (args);
			
			bool help = false;
			Func<IList<string>, int> command = null;
			
			var p = new OptionSet () {
				{ "h|?|help", v => help = v != null },
				{ "init|i", v => command = Init },
				{ "server|s", v => command = Server },
				{ "docs|d", v => command = Docs },
				{ "build|b", v => command = Build },
			};
			
			List<string> extra = null;
			try {
				extra = p.Parse(args);
			} catch (OptionException){
				Console.WriteLine ("Try `manos --help' for more information.");
				return 1;
			}
			
			if (help) {
				ShowHelp (p);
				return 0;
			}
			
			if (command == null) {
				ShowHelp (p);
				return 1;
			}
			
			command (extra);
			
			return 0;
		}

		private static string [] ParseGlobalOptions (string [] args)
		{
			var p = new OptionSet () {
				{ "-data-dir=", v => Environment.DataDirectory = v },
			};
			
			List<string> extra = null;
			try {
				extra = p.Parse(args);
			} catch (OptionException){
				Console.WriteLine ("Try `manos --help' for more information.");
				return null;
			}
			
			if (extra == null)
				return null;
			
			return extra.ToArray ();
		}
		
		private static int Init (IList<string> args)
		{
			if (args.Count < 1) {
				Console.WriteLine ("manos -init <AppName>");
				Console.WriteLine ("This will initialize a new application with the supplied name.");
			}
				
			Driver d = new Driver ();
			
			try {
				Console.WriteLine ("initing: {0}", args [0]);
				d.Init (args [0]);
			} catch (Exception e) {
				Console.WriteLine ("error while initializing application:");
				Console.WriteLine (e);
				return 1;
			}
			
			return 0;
		}
		
		public void Init (string name)
		{
			InitCommand initer = new InitCommand (Environment, name);
			
			initer.Run ();
		}

		private static int Server (IList<string> args)
		{
			Driver d = new Driver ();

			
			try {
				d.RunServer (args);
			} catch (Exception e) {
				Console.WriteLine ("error while serving application:");
				Console.WriteLine (e);
				return 1;
			}
			
			return 0;
		}
		
		public void RunServer (IList<string> args)
		{
			string port = null;
			string user = null;

			var p = new OptionSet () {
				{ "p|port=", v => port = v },
				{ "u|user=", v => user = v },
			};
			args = p.Parse(args);

			ServerCommand cmd = new ServerCommand (Environment, args);

			if (port != null) {
				int pt;
				if (!Int32.TryParse (port, out pt))
					throw new ArgumentException ("Port value is not an integer.");
				if (pt <= 0)
					throw new ArgumentOutOfRangeException ("port", "Port must be a positive integer.");
				cmd.Port = pt;
			}

			if (user != null)
				cmd.User = user;

			cmd.Run ();
		}
		
		private static int Docs (IList<string> args)
		{
			Driver d = new Driver ();
			
			try {
				d.RunDocs ();
			} catch (Exception e) {
				Console.WriteLine ("error while serving application:");
				Console.WriteLine (e);
				return 1;
			}
			
			return 0;
		}
		
		public void RunDocs ()
		{
			DocsCommand cmd = new DocsCommand (Environment);
			
			cmd.Run ();
		}

		private static int Build (IList<string> args)
		{
			Driver d = new Driver ();
			
			try {
				d.RunBuild ();
			} catch (Exception e) {
				Console.WriteLine ("error while building application:");
				Console.WriteLine (e);
				return 1;
			}
			
			return 0;
		}
		
		public void RunBuild ()
		{
			BuildCommand cmd = new BuildCommand (Environment);
			
			cmd.Run ();
		}
		
		private static void ShowHelp (OptionSet os)
		{
			Console.WriteLine ("manos usage is: manos [command] [options]");
			Console.WriteLine ();
			os.WriteOptionDescriptions (Console.Out);
		}
	}
}
