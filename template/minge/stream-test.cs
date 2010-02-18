
using System;
using System.IO;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;

using Mango.Templates.Minge;


public class B {

	public virtual void RenderToStream (TextWriter writer, Dictionary<string,object> args)
	{

	}
	
	public virtual void some_block (TextWriter writer, Dictionary<string,object> args, object foo, string bar="jackson")
	{
		writer.Write ("HI I AM THE SOME BLOCK STRING");
	}
}

public class T : B {

	private object some_field;


	public override void RenderToStream (TextWriter writer, Dictionary<string,object> args)
	{
		base.RenderToStream (writer, args);

		MemoryStream mem_stream = new MemoryStream ();
		some_block (new StreamWriter (mem_stream), args, "first param");
		Console.WriteLine (mem_stream.ToString ());

		/*
		foreach (object the_var in (IEnumerable) args ["the_iter"]) {
			writer.Write (args ["the_var"].ToString ());
		}

		IEnumerator enumerator = ((IEnumerable) args ["the_iter"]).GetEnumerator ();
		while (enumerator.MoveNext ()) {
			writer.Write (enumerator.Current);	
		}
		*/

		if (some_field != null && !((some_field is string) && !(String.IsNullOrEmpty ((string) some_field)))) 
			writer.Write ("this is a string");
	}

}

