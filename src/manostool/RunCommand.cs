//
// Copyright (C) 2011 Andrius Bentkus (andrius.bentkus@gmail.com)
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
using System.Collections.Generic;

using Manos.IO;

namespace Manos.Tool
{
	public class RunCommand
	{
		public int Run (string app, IList<string> args)
		{
			IManosRun mr = Loader.LoadLibrary<IManosRun> (app, new List<string> ());
			
			IOLoop loop = IOLoop.Instance;
			
			string [] strargs = new string [args.Count];
			for (int i = 0; i < strargs.Length; i++) {
				strargs [i] = args [i];
			}
			
			if (mr == null) {
				Console.WriteLine ("Binary {0} has no suitable entry point", app);
				return -1;
			}
			
			int ret = mr.Main (strargs);
			
			loop.Start ();
			
			return ret;
		}
	}
}

