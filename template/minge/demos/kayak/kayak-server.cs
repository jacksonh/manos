
using System;
using System.Collections.Generic;


using Kayak;
using Kayak.Framework;
using Mango.Templates.Minge;

public class KayakDemo {

	public static void Main(string[] args)
	{
		var server = new KayakServer();

		Minge.Initialize ("templates");
		
		server.UseFramework ();
		server.Start ();
		Console.WriteLine("Kayak is running! Press enter to exit.");
		Console.ReadLine ();
		server.Stop ();
	}

	public class Views : KayakService 
	{
		[Path("/")]
		public void Root () 
		{
			Dictionary<string,object> the_args = new Dictionary<string,object> ();
			the_args.Add ("views", new string [] { "numbers", "properties" });

			Minge.RenderToStream ("root.html", Response.Output, the_args);
		}

		[Path("/numbers")]
		public void Numbers (string word) 
		{
			Minge.RenderToStream ("numbers.html", Response.Output);
		}

		[Path("/properties")]
		public void Properties (string word) 
		{
			Minge.RenderToStream ("properties.html", Response.Output);
		}
	}
}

