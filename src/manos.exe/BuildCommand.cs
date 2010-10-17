//
// Copyright (C) 2010 Jackson Harper (jackson@manosdemono.com)
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//
//


using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Diagnostics;
using Microsoft.CSharp;
using System.CodeDom.Compiler;

namespace Manos.Tool
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
			if (RunXBuild ())
				return;
			if (RunMake ())
				return;

			CompileCS ();
		}

		public bool RunXBuild ()
		{
			string [] slns = Directory.GetFiles (Directory.GetCurrentDirectory (), "*.sln");
			Console.WriteLine ("COMPILING SOLUTION:  '{0}'  '{1}'", slns.Length, Directory.GetCurrentDirectory ());
			if (slns.Length < 1)
				return false;

			foreach (string sln in slns) {
				Process p = Process.Start ("xbuild", sln);
				p.WaitForExit ();
			}

			return true;
		}

		public bool RunMake ()
		{
			if (!File.Exists ("Makefile") && !File.Exists ("makefile"))
				return false;

			Process p = Process.Start ("make");
			p.WaitForExit ();
			
			return true;
		}

		public void CompileCS ()
		{
			var provider = new CSharpCodeProvider ();
			var options = new CompilerParameters (ReferencedAssemblies, OutputAssembly, true);

			foreach (string s in ReferencedAssemblies) {
				Console.WriteLine (s);	
			}
			
			var results = provider.CompileAssemblyFromFile (options, Sources);
			
			if (results.Errors.Count > 0) {
				foreach (var e in results.Errors) {
					Console.WriteLine (e);	
				}
			}
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
			var libs = new List<string> ();
			
			AddDefaultReferences (libs);

			foreach (string lib in Directory.GetFiles (Directory.GetCurrentDirectory ())) {
				if (!lib.EndsWith (".dll", StringComparison.InvariantCultureIgnoreCase))
					continue;
				if (Path.GetFileName (lib) == OutputAssembly)
					continue;
				libs.Add (lib);
			}
			
			return libs.ToArray ();
		}
		
		private void AddDefaultReferences (List<string> libs)
		{
			string manosdll = Path.Combine (Environment.ManosDirectory, "Manos.dll");
			libs.Add (manosdll);
		}
	}
}
