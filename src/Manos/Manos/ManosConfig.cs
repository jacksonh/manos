//
// ManosConfig - configuration management for Manos
// Author: Axel Callabed <axelc@github.com>
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

using Manos.Logging;

#if ENABLE_CONFIG
using Mono.CSharp;
using System.Threading;
using System.Runtime.Remoting.Messaging;
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
		delegate bool AsyncParseWorker(string path);
	
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
		
		// TODO: because execution of config scripts is async, we can run into race conditions if application
		// code requests a config variable before we're done executing the script. We should add a callback
		// parameter, so we can notify the ManosApp that config is ready for business.
		
		public static void Load (ManosApp app)
		{
#if ENABLE_CONFIG		
			
			Evaluator.Init (new string[0]);
			
			string [] sources =
			{
				Path.Combine (Environment.CurrentDirectory, "manos.config"),
			};
			
			foreach (string src in sources)
			{
				try
				{
					if (File.Exists (src)) 
					{
						AsyncParseWorker w = new AsyncParseWorker (ParseFile);
						w.BeginInvoke (src, ir =>
							{
								AsyncResult res = (AsyncResult) ir;
								AsyncParseWorker cw = (AsyncParseWorker) res.AsyncDelegate;
								
								cw.EndInvoke (ir);
							},
						null);
					}
				}
				catch (Exception ex)
				{
					AppHost.Log.Error ("Error processing configuration: {0}", ex.Message);
					AppHost.Log.Error ("Trace: {0}", ex.StackTrace);
					continue; 
				}
			}
#endif				
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
		private static bool ParseFile (string path)
		{
#if ENABLE_CONFIG		
			try
			{	
				using (StreamReader sr = new StreamReader (path))
				{
					string txt = sr.ReadToEnd ();
					
					if (String.IsNullOrEmpty (txt))
						return true;		
											
					Evaluator.ReferenceAssembly (Assembly.GetExecutingAssembly ());
					Evaluator.Run ("using Manos;");
					Evaluator.Run (txt);
								
					AppHost.Log.Info ("Read configuration from '{0}'", path);
					
					return true;
				}
			}
			catch (Exception ex)
			{
				AppHost.Log.Error ("Error processing config file '{0}': {1}", path, ex.Message);
				AppHost.Log.Error ("Trace: {0}", ex.StackTrace);
				
				return false;
			}
#else
		return false;	
#endif		

		}
		
#endregion		
	}	
}

