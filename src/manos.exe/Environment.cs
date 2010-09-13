
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
			
			string exe_path = typeof (Driver).Assembly.GetName ().CodeBase;
			Console.WriteLine ("EXE PATH:  '{0}", exe_path);
			string manos_dir = Path.GetDirectoryName (exe_path);
			string lib_dir = Path.GetDirectoryName (manos_dir);
			string prefix = Path.GetDirectoryName (lib_dir);
			Console.WriteLine ("PREFIX:  '{0}'", prefix);
			
			DataDirectory = Path.Combine (prefix.ToString (), "share/manos/");
			Console.WriteLine ("DATA DIRECTORY:  '{0}'", DataDirectory);
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
