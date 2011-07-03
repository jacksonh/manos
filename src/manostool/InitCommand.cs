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

namespace Manos.Tool
{
	public class InitCommand
	{
		public static readonly string LEGAL_NAME_CHARS = "_-.";
		
		public static readonly string TEMPLATES_DIRECTORY = "Templates";
		public static readonly string DEPLOYMENT_DIRECTORY = "Deployment";
		
		private string layout;
		private string dest_dir;
		
		public InitCommand (Environment env, string name)
		{
			if (env == null)
				throw new ArgumentNullException ("environment");
			if (name == null)
				throw new ArgumentNullException ("name");
			if (!IsLegalApplicationName (name))
				throw new ArgumentException ("name", "Application name contains non legal chars.");
			
			Environment = env;
			ApplicationName = name;
		}
		
		public Environment Environment {
			get;
			private set;
		}
		
		public string ApplicationName {
			get;
			private set;
		}
		
		public string LayoutsDirectory {
			get {
				return Path.Combine (Environment.DataDirectory, "layouts");
			}
		}
		
		public string LayoutDirectory {
			get {
				return Path.Combine (LayoutsDirectory, Layout);
			}
		}
		
		public string Layout {
			get {
				if (layout == null)
					return "default";
				return layout;
			}

			set {
				if (value == null) {
					layout = null;
					return;
				}
				if (!Directory.Exists (Path.Combine (LayoutsDirectory, value)))
					throw new ArgumentException ("Layout does not exist.");
				
				layout = value;	
			}
		}
		
		public string DestinationDirectory {
			get {
				if (dest_dir == null)
					return Environment.WorkingDirectory;
				return dest_dir;
			}
			set {
				if (value == null)
					throw new ArgumentNullException ("value");
				if (!Directory.Exists (value))
					throw new ArgumentException ("value", "Directory does not exist.");	
				
				dest_dir = value;
			}
		}
		
		public void Run ()
		{
			string appdir = Path.Combine (DestinationDirectory, ApplicationName);

			Directory.CreateDirectory (appdir);
			
			CopyFilesRecurse (new DirectoryInfo (LayoutDirectory), new DirectoryInfo (appdir));
		}
		
		public void CopyFilesRecurse (DirectoryInfo source, DirectoryInfo target) {
    		
			foreach (DirectoryInfo dir in source.GetDirectories ())
        		CopyFilesRecurse (dir, target.CreateSubdirectory (dir.Name));
    		
			foreach (FileInfo file in source.GetFiles()) {
				string dest = Path.Combine (target.FullName, file.Name);
        		file.CopyTo (dest);
				
				if (file.Extension == ".cs" || file.Extension==".cshtml" || file.Extension == ".sln" || file.Extension ==".csproj" || file.Extension == ".user")
					ConvertCSFile (dest);
				
				ConvertFileName (dest);
			}
		}
		
		// HACK: This needs to be a lot more extensive if we want people 
		// to create their own layouts.
		private void ConvertCSFile (string file)
		{
			string cs = File.ReadAllText (file);
			string converted = Convert (cs);
			
			File.WriteAllText (file, converted);
		}
		
		private void ConvertFileName (string name)
		{
			string new_name = Convert (name);
			
			File.Move (name, new_name);
		}
		
		private string Convert (string src)
		{
			src = src.Replace ("$APPNAME", ApplicationName);
			src = src.Replace ("$MANOSDIR", System.IO.Path.GetDirectoryName(GetType().Assembly.Location));
			return src;
		}
		
		public bool IsLegalApplicationName (string name)
		{	
			if (String.IsNullOrEmpty (name))
				return false;
			
			if (!Char.IsLetter (name [0]) || name [0] == '_')
				return false;
			
			for (int i = 1; i < name.Length; i++) {
				if (Char.IsLetterOrDigit (name [i])	|| LEGAL_NAME_CHARS.IndexOf (name [i]) != -1)
					continue;
				return false;
			}
			
			return true;
		}
	}
}
