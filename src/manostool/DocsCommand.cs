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
using System.Linq;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;

using Manos;
using Manos.Http;


namespace Manos.Tool
{
	public class DocsCommand
	{				
		int? port;
		
		public DocsCommand (Environment env)
		{
			Environment = env;
		}
		
		public Environment Environment {
			get;
			private set;
		}
		
		public int Port {
			get { 
				if (port == null)
					return 8181;
				return (int) port;
			}
			set {
				if (port <= 0)
					throw new ArgumentException ("port", "port must be greater than zero.");
				port = value;	
			}
		}
		
		public void Run ()
		{
			DocsModule docs = new DocsModule (Environment.DocsDirectory);
			Console.WriteLine ("Go to http://localhost:{0}/ to see your docs.", Port);
			
			AppHost.Port = Port;
			AppHost.Start (docs);
		}
	}
}
