
using System;
using System.IO;

namespace Mango.Tool
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
				return Layout;
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
				
				if (file.Extension == ".cs")
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
			return src.Replace ("$APPNAME", ApplicationName);	
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
