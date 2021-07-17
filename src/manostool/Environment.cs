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
			IsWindows = false;
			
			string exe_path = new Uri (typeof (Driver).Assembly.GetName ().CodeBase).LocalPath;

			if (System.Environment.OSVersion.Platform == PlatformID.Win32NT
				|| System.Environment.OSVersion.Platform == PlatformID.Win32S
				|| System.Environment.OSVersion.Platform == PlatformID.Win32Windows
				|| System.Environment.OSVersion.Platform == PlatformID.WinCE) {
				IsWindows = true;
				ManosDirectory = Path.GetDirectoryName(exe_path);
				DataDirectory = ManosDirectory;
				DocsDirectory = Path.Combine(ManosDirectory, "docs");
			} else {
				ManosDirectory = Path.GetDirectoryName (exe_path);
				string lib_dir = Path.GetDirectoryName (ManosDirectory);
				string prefix = Path.GetDirectoryName (lib_dir);
			
				DataDirectory = Path.Combine (prefix, "share/manos/");
				DocsDirectory = Path.Combine (prefix, "share/doc/manos");
			}
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

		public string DocsDirectory {
			get;
			set;
		}

		public bool IsWindows {
			get;
			private set;
		}
	}
}
