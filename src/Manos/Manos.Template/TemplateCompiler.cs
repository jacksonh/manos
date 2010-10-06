

namespace Manos.Templates {

	public class TemplateCompiler {

		private Dictionary<string,Page> pages = new Dictionary<string,Page> ();

		public TemplateCompiler (TemplateEnvironment environment)
		{
			Environment = environment;
			Application = CreateApplication ();
		}

		public TemplateEnvironment Environment {
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

