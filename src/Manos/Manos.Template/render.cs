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
using System.Collections.Generic;

public class T {

	public static int Main (string [] args)
	{
		if (args.Length < 2)
			return Usage ();

		Dictionary<string,object> targs = new Dictionary<string,object> ();

		Assembly asm = Assembly.LoadFrom (args [0]);
		Type template_type = asm.GetType (args [1]);

		for (int i = 2; i + 1 < args.Length; i += 2) {
			targs.Add (args [i], args [i + 1]);
		}

		targs ["test_enumerable"] = new List<string> () { "one", "two", "three", "four" };

		Console.WriteLine ("TEMPLATE TYPE:  {0}", template_type);
		MethodInfo meth = template_type.GetMethod ("RenderToStream");
		object template = Activator.CreateInstance (template_type);

		MemoryStream stream = new MemoryStream ();
		StreamWriter writer = new StreamWriter (stream);

		meth.Invoke (template, new object [] { Console.Out, targs });

		
		return 0;
	}

	public static int Usage ()
	{
		Console.WriteLine ("render template-assembly template-name [key value,...]");

		return -1;
	}
}


