
using System;
using System.IO;

namespace Manos.Tool
{


	public class Environment
	{

		public Environment ()
		{
			LibDirectory = "lib";
			TemplatesDirectory = "Templates";
			WorkingDirectory = Directory.GetCurrentDirectory ();
			DataDirectory = Path.Combine (System.Environment.GetFolderPath (System.Environment.SpecialFolder.CommonApplicationData), "manos-tool");
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
