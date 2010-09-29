
using System;
using System.IO;
using System.Reflection;

namespace Manos.Tool
{


	public class Environment
	{

		public Environment ()
		{
			LibDirectory = "lib";
			TemplatesDirectory = "Templates";
			WorkingDirectory = Directory.GetCurrentDirectory ();
			
			string exe_path = new Uri (typeof (Driver).Assembly.GetName ().CodeBase).LocalPath;
			ManosDirectory = Path.GetDirectoryName (exe_path);
			string lib_dir = Path.GetDirectoryName (ManosDirectory);
			string prefix = Path.GetDirectoryName (lib_dir);
			
			DataDirectory = Path.Combine (prefix.ToString (), "share/manos/");
		}
		
		public string LibDirectory {
			get;
			set;
		}
		
		public string ManosDirectory {
			get;
			set;
		}
		
		public string TemplatesDirectory {
			get;
			set;
		}
		
		public string WorkingDirectory {
			get;
			set;
		}
		
		public string DataDirectory {
			get;
			set;
		}
	}
}
