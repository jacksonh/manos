
using System;
using System.IO;
using System.Text;
using System.Linq;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;


namespace Mango.Templates {

	public class FooBarTest {
		
	}
	
	public static class Engine {

		private static Dictionary<string,Type> loaded_templates = new Dictionary<string,Type> ();
		private static Assembly template_assembly = null;

		public static string RenderToSting (string path)
		{
			return RenderToString (path, new object ());
		}

		public static string RenderToString (string path, object the_arg)
		{
			MemoryStream stream = new MemoryStream ();
			StreamWriter writer = new StreamWriter (stream);

			RenderToStream (path, writer, the_arg);
			writer.Flush ();

			stream.Seek (0, SeekOrigin.Begin);
			StreamReader reader = new StreamReader (stream);
			return reader.ReadToEnd ();
		}

		public static void RenderToStream (string path, TextWriter writer, object the_arg)
		{
			/*
			string name = Page.FullTypeNameForPath (ApplicationName, path);
			Type t = GetType (name);

			IMingePage page = (IMingePage) Activator.CreateInstance (t);
			page.RenderToStream (writer, the_arg);
			*/
		}

		private static Type GetType (string name)
		{
			Type res = null;

			if (loaded_templates.TryGetValue (name, out res))
				return res;

			if (template_assembly == null) {
				foreach (Assembly a in AppDomain.CurrentDomain.GetAssemblies ()) {
					res = a.GetType (name);
					if (res != null) {
						template_assembly = a;
						break;
					}
				}
			}

			loaded_templates.Add (name, res);
			return res;
		}
	}

	public class MingeCompiler {

		private Dictionary<string,Page> pages = new Dictionary<string,Page> ();

		public MingeCompiler (MingeEnvironment environment)
		{
			Environment = environment;
			Application = CreateApplication ();
		}

		public MingeEnvironment Environment {
			get;
			private set;
		}

		internal Application Application {
			get;
			private set;
		}

		private Application CreateApplication ()
		{
			Application app = new Application (this, Environment.AssemblyName, Environment.AssemblyPath);
			
			return app;
		}

		// Ensures that the compiled assembly is up to date with all of the template files.
		public bool CheckForUpdates ()
		{
			if (!File.Exists (Environment.AssemblyPath))
				return true;

			DateTime ct = File.GetLastWriteTime (Environment.AssemblyPath);

			return CheckDirectoriesRecursive (ct, Environment.TemplateDirectories);
		}

		// Compiles all the templates that are found in the template directories
		public void CompileTemplates ()
		{
			MingeParser p = new MingeParser (Environment, Application);

			CompileDirectoriesRecursive (p, Environment.TemplateDirectories, String.Empty, true);

			Application.Save ();

		}

		internal Page ParsePage (string path)
		{
			MingeParser p = new MingeParser (Environment, Application);
			string full_path = FindFullPath (path);

			if (full_path == null)
				throw new Exception (String.Format ("Template not found: {0}", path));

			Page page = null;
			if (pages.TryGetValue (full_path, out page)) {
				return page;
			}

			using (TextReader tr = new StreamReader (File.OpenRead (full_path))) {
				page = p.ParsePage (path, tr);
			}

			pages.Add (full_path, page);
			return page;
		}

		private string FindFullPath (string path)
		{
			foreach (string directory in Environment.TemplateDirectories) {
				string full = Path.Combine (directory, path);
				
				if (File.Exists (full))
					return full;
			}

			return null;
		}

		private void CompileDirectoriesRecursive (MingeParser parser, string [] directories, string root_dir, bool top)
		{
			foreach (string directory in directories) {
				CompileFiles (parser, top ? directory : root_dir, Directory.GetFiles (directory));
				CompileDirectoriesRecursive (parser, Directory.GetDirectories (directory), top ? directory : root_dir, false);
			}
		}

		private void CompileFiles (MingeParser parser, string root_dir, string [] files)
		{
			foreach (string file in files) {
				if (!Environment.AllowedExtensions.Contains (Path.GetExtension (file)))
					continue;
				ParsePage (file.Substring (root_dir.Length + 1));
			}
		}

		private bool CheckDirectoriesRecursive (DateTime ct, string [] directories)
		{
			foreach (string directory in directories) {
				if (CheckFiles (ct, Directory.GetFiles (directory)))
					return true;
				if (CheckDirectoriesRecursive (ct, Directory.GetDirectories (directory)))
					return true;
			}

			return false;
		}

		private bool CheckFiles (DateTime ct, string [] files)
		{
			return files.Count (f => File.GetLastWriteTime (f) > ct) > 0;
		}
	}
}

