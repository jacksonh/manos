
using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;

using NUnit.Framework;


namespace Mango.Templates.Minge.Tests {

	public class MingeTest {

		public static string RenderString (string str, Dictionary<string,object> the_args=null)
		{
			string assembly_path = "minge-test.dll";
			string test_name = Guid.NewGuid ().ToString ();
			string template_name = String.Concat ("templates.page_", test_name);

			AppDomainSetup domain_setup = new AppDomainSetup () {
				ApplicationBase  = Directory.GetCurrentDirectory (),
				ShadowCopyFiles = "true",
			};

			Console.WriteLine ("\nTEST NAME: {0}\n", test_name);

			AppDomain test_domain = AppDomain.CreateDomain (Guid.NewGuid ().ToString (), null, domain_setup);

			Environment environment = new Environment ();
			Application app = new Application (environment, "minge-test", assembly_path);
			MingeParser p = new MingeParser (environment, app);

			MemoryStream the_stream = new MemoryStream (100);
			StreamWriter writer = new StreamWriter (the_stream);
			
			writer.Write (str);
			writer.Flush ();

			the_stream.Seek (0, SeekOrigin.Begin);
			p.ParsePage (test_name, new StreamReader (the_stream));
			app.Save ();

			Assembly asm = test_domain.Load (LoadAssembly (assembly_path));
			MingePage template = (MingePage) test_domain.CreateInstanceAndUnwrap (asm.FullName, template_name);
			// MingePage template = (MingePage) Activator.CreateInstance (asm.GetType (), template_name);

			MemoryStream stream = new MemoryStream (1000);
			writer = new StreamWriter (stream);

			if (the_args == null)
				the_args = new Dictionary<string,object> ();

//			MethodInfo meth = template.GetType ().GetMethod ("RenderToStream");
//			meth.Invoke (template, new object [] { writer, the_args });

			template.RenderToStream (writer, the_args);

			writer.Flush ();
			stream.Seek (0, SeekOrigin.Begin);

			StreamReader reader = new StreamReader (stream);

			string result = reader.ReadToEnd ();

			AppDomain.Unload (test_domain);
			return result;
		}

		static byte [] LoadAssembly (string path)
		{
			FileStream fs = new FileStream (path, FileMode.Open);
			byte [] buffer = new byte [(int) fs.Length];
			fs.Read (buffer, 0, buffer.Length);
			fs.Close ();

			return buffer;
		}
	}
}

