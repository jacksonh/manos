

using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

using Mango.Templates.Minge;

using NDesk.Options;

namespace Mango.Tool
{
	class Driver
	{
		
		public static readonly string COMPILED_TEMPLATES_ASSEMBLY = "CompiledTemplates.dll";
		public static readonly string TEMPLATES_DIRECTORY = "Templates";
		public static readonly string DEPLOYMENT_DIRECTORY = "Deployment";
		
		public static int Main (string[] args)
		{
			bool help = false;
			Func<IList<string>, int> command;
			
			var p = new OptionSet () {
				{ "h|?|help", v => help = v != null },
				{ "init|i", v => command = Init },
				{ "build|b", v => command = Build },
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
			
			return 0;
		}

		private static int Init (IList<string> args)
		{
			if (args.Count < 1) {
				Console.WriteLine ("mango-tool init <AppName>");
				Console.WriteLine ("This will initialize a new application with the supplied name.");
			}
				
			Driver d = new Driver ();
			
			try {
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
			Directory.CreateDirectory (name);
			Directory.CreateDirectory (Path.Combine (name, TEMPLATES_DIRECTORY));
			Directory.CreateDirectory (Path.Combine (name, DEPLOYMENT_DIRECTORY));
		}
		
		private static int Build (IList<string> args)
		{
			string app_name = null;
			
			if (args.Count > 0)
				app_name = args [0];
			else {
				app_name = Directory.GetCurrentDirectory () + ".dll";
				if (!File.Exists (app_name)) {
					Console.WriteLine ("mango-tool -build [AppName.dll]");
					Console.WriteLine ("This will compile the templates and link them to the supplied assembly.");
					Console.WriteLine ("If you do not supply the AppName.dll the current directory name will be used.");
				}
			}
				
			Driver d = new Driver ();
			
			try {
				d.Build (app_name, TEMPLATES_DIRECTORY);
			} catch (Exception e) {
				Console.WriteLine ("error while building application:");
				Console.WriteLine (e);
				return 1;
			}
			
			return 0;
		}
		
		public void Build (string app, string templates)
		{
			CompileTemplates (templates, COMPILED_TEMPLATES_ASSEMBLY);
			LinkTemplates (app, COMPILED_TEMPLATES_ASSEMBLY);
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
			Mono.Merge.Driver.Run (new string [] { app_name, templates });
		}
		
		private static void ShowHelp (OptionSet os)
		{
			Console.WriteLine ("mango-tool usage is: mango-tool [command] [options]");
			Console.WriteLine ();
			os.WriteOptionDescriptions (Console.Out);
		}
	}
}
