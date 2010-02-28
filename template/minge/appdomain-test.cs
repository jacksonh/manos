

using System;
using System.IO;
using System.Collections.Generic;

using Mono.Cecil;
using Mono.Cecil.Cil;



public class AssemblyRunner : MarshalByRefObject 
{

	private static readonly System.Reflection.BindingFlags BINDING_FLAGS = System.Reflection.BindingFlags.Instance |
			System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.CreateInstance;

	public void LoadAndPrint (string the_assembly, string the_type)
	{
		System.Reflection.Assembly asm = System.Reflection.Assembly.LoadFrom (the_assembly);
		
		ITheType ithe_type = (ITheType) Activator.CreateInstanceFrom (the_assembly, the_type, false, BINDING_FLAGS, null, null, null, null, null).Unwrap ();
		
		ithe_type.PrintString ();
	}
}


	public interface ITheType {

		void PrintString ();
	}
	
	[Serializable]
	public class TheType : ITheType {

		public virtual void PrintString ()
		{
		}
	}


public class T {

	private static readonly string DOMAIN_NAME = "THE_DOMAIN";
	private static readonly string ASSEMBLY_NAME = "the_assembly";
	private static readonly string ASSEMBLY_PATH = "the_assembly.dll";
	private static readonly string TYPE_NAME = "TheType";
	private static readonly string NAMESPACE_NAME = "TheNamespace";
	private static readonly string METHOD_NAME = "PrintString";

	private static int domain_count = 0;

	public static void Main ()
	{
		AssemblyDefinition ad = CreateAssembly ("A");
		RunAssembly ();

		ad = CreateAssembly ("B");
		RunAssembly ();
	}

	public static AssemblyDefinition CreateAssembly (string str)
	{
		AssemblyDefinition ad = AssemblyFactory.DefineAssembly (ASSEMBLY_NAME, AssemblyKind.Dll);
		TypeDefinition the_type = new TypeDefinition (TYPE_NAME, NAMESPACE_NAME, TypeAttributes.Public | TypeAttributes.BeforeFieldInit | TypeAttributes.Serializable,
				ad.MainModule.Import (typeof (TheType)));

		ad.MainModule.Types.Add (the_type);

		MethodDefinition the_method = new MethodDefinition (METHOD_NAME, MethodAttributes.Public | MethodAttributes.Virtual, ad.MainModule.Import (typeof (void)));

		the_type.Methods.Add (the_method);

		MethodDefinition ctor = new MethodDefinition (".ctor", MethodAttributes.Public, ad.MainModule.Import (typeof (void)));
		the_type.Methods.Add (ctor);
		ctor.Body.CilWorker.Emit (OpCodes.Ret);


		
		CilWorker worker = the_method.Body.CilWorker;

		worker.Emit (OpCodes.Ldstr, str);
		worker.Emit (OpCodes.Call, ad.MainModule.Import (typeof (System.Console).GetMethod ("WriteLine", new Type [] { typeof (string) })));
		worker.Emit (OpCodes.Ret);

		AssemblyFactory.SaveAssembly (ad, ASSEMBLY_PATH);

		return ad;
	}

	public static void RunAssembly ()
	{
		AppDomainSetup domain_setup = new AppDomainSetup ();
		domain_setup.ApplicationBase = AppDomain.CurrentDomain.BaseDirectory;
		// domain_setup.ShadowCopyFiles = "true";

		AppDomain run_domain = AppDomain.CreateDomain (String.Concat (DOMAIN_NAME, "_", domain_count++), null, domain_setup);

		AssemblyRunner the_runner = (AssemblyRunner) run_domain.CreateInstance (typeof (T).Assembly.FullName, "AssemblyRunner").Unwrap ();

		the_runner.LoadAndPrint (ASSEMBLY_PATH, String.Concat (NAMESPACE_NAME, ".", TYPE_NAME));
		AppDomain.Unload (run_domain);
	}

}

