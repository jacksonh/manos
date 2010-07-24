
using System;
using System.IO;

namespace Mango.Tool
{


	public class Environment
	{

		public Environment ()
		{
			DataDirectory = Path.Combine (System.Environment.GetFolderPath (System.Environment.SpecialFolder.CommonApplicationData), "mango-tool");
		}
		
		public string DataDirectory {
			get;
			set;
		}
	}
}
