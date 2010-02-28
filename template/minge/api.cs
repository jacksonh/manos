
using System;
using System.IO;
using System.Text;
using System.Linq;
using System.Collections.Generic;


namespace Mango.Templates.Minge {

	public static class Minge {

		private static object lock_object = new object ();

		private static MingeContext context;
		private static MingeEnvironment environment;

		public static void Initialize (string templatedir)
		{
			Initialize (new string [] { templatedir });
		}

		public static void Initialize (string [] templatedirs)
		{
			lock (lock_object) {
				if (context == null) {
					environment = new MingeEnvironment (templatedirs);
					context = new MingeContext (environment);
				}
			}
		}

		public static void RenderToStream (string path, TextWriter writer)
		{
			context.RenderToStream (path, writer);
		}

		public static void RenderToStream (string path, TextWriter writer, Dictionary<string,object> the_args)
		{
			context.RenderToStream (path, writer, the_args);
		}
	}

	public class MingeContext {

		private AppDomain run_domain;
		private MingeAssemblyWrapper assembly_wrapper;

		public MingeContext (MingeEnvironment environment)
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

		public void LoadTemplates ()
		{
			AppDomainSetup domain_setup = new AppDomainSetup ();
			domain_setup.ApplicationBase = AppDomain.CurrentDomain.BaseDirectory;
			// domain_setup.ShadowCopyFiles = "true";

			run_domain = AppDomain.CreateDomain (Environment.AppDomainName, null, domain_setup);
			assembly_wrapper = (MingeAssemblyWrapper) run_domain.CreateInstance (GetType ().Assembly.FullName,
					typeof (MingeAssemblyWrapper).FullName).Unwrap ();
		}

		public void UnloadTemplates ()
		{
			AppDomain.Unload (run_domain);
			assembly_wrapper = null;
		}

		public void RenderToStream (string path, TextWriter writer)
		{
			RenderToStream (path, writer, new Dictionary<string,object> ());
		}

		public void RenderToStream (string path, TextWriter writer, Dictionary<string,object> the_args)
		{
			if (assembly_wrapper == null) {
				CompileTemplates ();
				LoadTemplates ();
			}
			assembly_wrapper.RenderToStream (Page.FullNameForPath (path), Environment.AssemblyPath, writer, the_args);
		}

		internal Page ParsePage (string path)
		{
			Console.WriteLine ("parsing path:  {0}", path);
			MingeParser p = new MingeParser (Environment, Application);
			string full_path = FindFullPath (path);

			if (full_path == null)
				throw new Exception (String.Format ("Template not found: {0}", path));

			using (TextReader tr = new StreamReader (File.OpenRead (full_path))) {
				return p.ParsePage (path, tr);
			}
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

		private Application CreateApplication ()
		{
			Application app = new Application (this, Environment.AssemblyName, Environment.AssemblyPath);

			return app;
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
				using (TextReader tr = new StreamReader (File.OpenRead (file))) {
					Console.WriteLine ("root: {0}   file:  {1}   name:  {2}", root_dir, file, file.Substring (root_dir.Length + 1));
					parser.ParsePage (file.Substring (root_dir.Length + 1), tr);
				}
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
			foreach (string file in files) {
				Console.WriteLine ("CT: {0} -- {1}", ct, File.GetLastWriteTime (file));
			}
			return files.Count (f => File.GetLastWriteTime (f) > ct) > 0;
		}

		private void CompileDirectories ()
		{

		}
	}

	internal class MingeAssemblyWrapper : MarshalByRefObject {

		private static readonly System.Reflection.BindingFlags BINDING_FLAGS = System.Reflection.BindingFlags.Instance |
			System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.CreateInstance;

		public void RenderToStream (string type_name, string assembly_path, TextWriter writer, Dictionary<string,object> the_args)
		{
			Console.WriteLine ("USING ASSEMBLY PATH:  {0}", assembly_path);
			

			try {
				Console.WriteLine ("CREATING THE PAGE");
				IMingePage page = (IMingePage) Activator.CreateInstanceFrom (assembly_path, type_name, false, BINDING_FLAGS, null, null, null, null, null).Unwrap ();
				Console.WriteLine ("RENDERING THE PAGE");
				page.RenderToStream (writer, the_args);
				Console.WriteLine ("DONE");
			} catch (Exception e) {
				Console.WriteLine ("EXCEPTION WHILE RENDERING:  {0}", e);
			}
		}
	}
}


