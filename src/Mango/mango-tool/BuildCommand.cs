
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Microsoft.CSharp;
using System.CodeDom.Compiler;

using Mango.Templates;

namespace Mango.Tool
{
	public class BuildCommand
	{
		public static readonly string COMPILED_TEMPLATES = "Templates.dll";
		
		private string [] sources;
		private string [] ref_asm;
		private string output_name;
		
		public BuildCommand (Environment env)
		{
			Environment = env;
		}
		
		public Environment Environment {
			get;
			private set;
		}
		
		public string [] Sources {
			get {
				if (sources == null)
					sources = CreateSourcesList ();
				return sources;
			}
			set {
				if (value == null)
					throw new ArgumentNullException ("value");
				sources = value;
			}
		}
		
		public string OutputAssembly {
			get {
				if (output_name == null)
					return Path.GetFileName (Directory.GetCurrentDirectory ()) + ".dll";
				return output_name;
			}
			set {
				if (value == null)
					throw new ArgumentNullException ("value");
				output_name = value;
			}
		}
		
		public string [] ReferencedAssemblies {
			get {
				if (ref_asm == null)
					ref_asm = CreateReferencesList ();
				return ref_asm;
			}
			set {
				ref_asm = value;
			}
		}
		
		public void Run ()
		{
			CompileCS ();
			CompileTemplates ();
			MergeLibraries ();
		}
		
		public void CompileCS ()
		{
			var provider = new CSharpCodeProvider ();
			var options = new CompilerParameters (ReferencedAssemblies, OutputAssembly);

			foreach (string s in ReferencedAssemblies) {
				Console.WriteLine (s);	
			}
			
			Console.WriteLine ("output assembly: {0}", OutputAssembly);
			var results = provider.CompileAssemblyFromFile (options, Sources);
			
			if (results.Errors.Count > 0) {
				Console.WriteLine ("Compiler Errors.");
				foreach (var e in results.Errors) {
					Console.WriteLine (e);	
				}
			}
		}
		
		public void CompileTemplates ()
		{
			MingeEnvironment environment = new MingeEnvironment (new string [] { Environment.TemplatesDirectory }) {
				AssemblyName = Path.GetFileNameWithoutExtension (COMPILED_TEMPLATES),
				AssemblyFile = COMPILED_TEMPLATES,
			};
			MingeCompiler compiler = new MingeCompiler (environment);

			compiler.CompileTemplates ();
		}
		
		public void MergeLibraries ()
		{
		// 	Mono.Merge.Driver.Run (Environment.ApplicationName, new string [] { COMPILED_TEMPLATES, Environment.ApplicationName });
		}
		
		private string [] CreateSourcesList ()
		{
			List<string> sources = new List<string> ();
			
			FindCSFilesRecurse (Environment.WorkingDirectory, sources);
			
			return sources.ToArray ();
		}
		
		private void FindCSFilesRecurse (string dir, List<string> sources)
		{
			sources.AddRange (Directory.GetFiles (dir, "*.cs"));
			
			foreach (string subdir in Directory.GetDirectories (dir)) {
				if (dir == Environment.WorkingDirectory) {
					if (subdir == "Content" || subdir == "Templates")
						continue;
				}
				if (subdir.EndsWith (".exclude"))
					continue;
				FindCSFilesRecurse (subdir, sources);
			}
		}
		
		private string [] CreateReferencesList ()
		{
			Console.WriteLine ("lib directory:  {0}", Environment.LibDirectory);
			if 	(!Directory.Exists (Environment.LibDirectory))
				return new string [0];
			
			List<string> libs = new List<string> ();
			foreach (string lib in Directory.GetFiles (Environment.LibDirectory)) {
				if (!lib.EndsWith (".dll", StringComparison.InvariantCultureIgnoreCase))
					continue;
				libs.Add (lib);
			}
			
			AddDefaultReferences (libs);
			
			return libs.ToArray ();
		}
		
		private void AddDefaultReferences (List<string> libs)
		{
		}
	}
}
