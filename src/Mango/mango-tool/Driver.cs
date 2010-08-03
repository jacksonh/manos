

using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

using Mango.Templates;

using NDesk.Options;

namespace Mango.Tool
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
				{ "build|b", v => command = Build },
				{ "server|s", v => command = Server },
				{ "compile-templates|ct", v => command = CompileTemplates },
				{ "link-templates|lt", v => command = LinkTemplates }
			};
			
			List<string> extra = null;
			try {
				extra = p.Parse(args);
			} catch (OptionException){
				Console.WriteLine ("Try `mango-tool --help' for more information.");
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
			
			Console.WriteLine ("executing command: {0}", command);
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
				Console.WriteLine ("Try `mango-tool --help' for more information.");
				return null;
			}
			
			if (extra == null)
				return null;
			
			return extra.ToArray ();
		}
		
		private static int Init (IList<string> args)
		{
			if (args.Count < 1) {
				Console.WriteLine ("mango-tool init <AppName>");
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
		
		private static int Build (IList<string> args)
		{
			string app_name = null;
			
			Driver d = new Driver ();
			
			try {
				d.Build ();
			} catch (Exception e) {
				Console.WriteLine ("error while building application:");
				Console.WriteLine (e);
				return 1;
			}
			
			return 0;
		}
		
		public void Build ()
		{
			BuildCommand build = new BuildCommand (Environment);
			
			build.Run ();
		}
		
		private static int Server (IList<string> args)
		{
			Driver d = new Driver ();
			
			try {
				d.Server ();
			} catch (Exception e) {
				Console.WriteLine ("error while serving application:");
				Console.WriteLine (e);
				return 1;
			}
			
			return 0;
		}
		
		public void Server ()
		{
			ServerCommand cmd = new ServerCommand (Environment);
			
			cmd.Run ();
		}
		
		private static int CompileTemplates (IList<string> args)
		{
			string templates;
			
			if (args.Count > 1)
				templates = args [0];
			else {
				templates = TEMPLATES_DIRECTORY;
				if (!Directory.Exists (templates)) {
					Console.WriteLine ("mango-tool -compile-templates [Template Directory]");
					Console.WriteLine ("Compile the supplied template directory.");
					Console.WriteLine ("If the template directory is not found {0} will be used.", TEMPLATES_DIRECTORY);
					return 1;
				}
			}
			
			Driver d = new Driver ();
			d.CompileTemplates (templates, COMPILED_TEMPLATES_ASSEMBLY);
			return 0;
		}
		
		public void CompileTemplates (string templates, string assembly_name)
		{
			MingeEnvironment environment = new MingeEnvironment (new string [] { templates }) {
				AssemblyName = Path.GetFileNameWithoutExtension (assembly_name),
				AssemblyFile = assembly_name,
			};
			MingeCompiler compiler = new MingeCompiler (environment);

			compiler.CompileTemplates ();	
		}
		
		private static int LinkTemplates (IList<string> args)
		{
			string app_name;
			string templates;
			
			if (args.Count > 0)
				app_name = args [0];
			else
				app_name = Directory.GetCurrentDirectory () + ".dll";

			if (args.Count > 1) 
				templates = args [1];
			else 
				templates = COMPILED_TEMPLATES_ASSEMBLY;
			
			if (!File.Exists (app_name) || !File.Exists (templates)) {
				Console.WriteLine ("mango-tool -build [AppName.dll] [TemplatesAssembly.dll]");
				return 1;
			}
			
			Driver d = new Driver ();
			d.LinkTemplates (app_name, templates);
			return 0;
		}
		
		public void LinkTemplates (string app_name, string templates)
		{
			Mono.Merge.Driver.Run (app_name, new string [] { templates, app_name });
		}
		
		private static void ShowHelp (OptionSet os)
		{
			Console.WriteLine ("mango-tool usage is: mango-tool [command] [options]");
			Console.WriteLine ();
			os.WriteOptionDescriptions (Console.Out);
		}
	}
}
