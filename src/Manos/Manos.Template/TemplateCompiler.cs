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

