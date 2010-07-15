

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


