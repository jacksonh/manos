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
using System.Collections.Generic;
using System.Reflection;

#if !DISABLE_POSIX
using Mono.CSharp;
#endif

namespace Manos
{
	public class ManosConfigException : Exception
	{
		public ManosConfigException (string msg) 
		: base (msg)
		{
		}
		
		public ManosConfigException (string fmt, params object[] args) 
		: base (String.Format (fmt, args))
		{
		}
	}
	
	public static class ManosConfig
	{
		// Property storage
		private static Dictionary<string, object> cfg = new Dictionary<string, object> ();
		
		static ManosConfig ()
		{
			cfg.Add ("Manos.Version", Assembly.GetExecutingAssembly ().ToString());	
		}
				
#region Public methods
		
		// Load configuration from file
		// Search path:
		//	1.<app_folder>/manos.config
		public static void Load (IManosPipe pipe)
		{
			string [] sources =
			{
				Path.Combine (Environment.CurrentDirectory, "manos.config"),
			};
			
			foreach (string src in sources)
			{
				try
				{
					if (File.Exists (src))
						CompileFile (src);
				}
				catch (Exception ex)
				{
					Console.Error.WriteLine ("** Error processing config file '{0}': {1}", src, ex.Message);
					Console.Error.WriteLine ("** Trace: {0}", ex.StackTrace);
					continue; 
				}
			}	
		}
		
		public static void Add<T> (string key, T val)
		{		
			lock (cfg)
			{
				cfg.Remove (key);
					
				cfg.Add (key, val);	
			}
		}
		
		public static void Add (string key, object val)
		{
			Add<object> (key, val);
		}
		
		
		public static T Get<T> (string key)
		{
			object tmp;
			
			if (cfg.TryGetValue (key, out tmp))
				return (T)tmp;
			else
				return default(T);
		}
		
		public static object Get (string key)
		{
			return Get<object> (key);
		}
		
#endregion
		
#region Private methods		
		private static bool CompileFile (string path)
		{
#if !DISABLE_POSIX
			using (StreamReader sr = new StreamReader (path))
			{
				string txt = sr.ReadToEnd ();
				
				Evaluator.Init (new string[0]);
				Evaluator.ReferenceAssembly (Assembly.GetExecutingAssembly ());
				Evaluator.Run ("using Manos;");
				Evaluator.Run (txt);
				
				return true;
			}
#else
            return false;
#endif
		}
		
#endregion		
	}	
}

