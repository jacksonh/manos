
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
			string manos_dir = Path.GetDirectoryName (exe_path);
			string lib_dir = Path.GetDirectoryName (manos_dir);
			string prefix = Path.GetDirectoryName (lib_dir);
			
			DataDirectory = Path.Combine (prefix.ToString (), "share/manos/");
		}
		
		public string LibDirectory {
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
