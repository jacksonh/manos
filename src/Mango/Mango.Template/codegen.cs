
using System;
using System.IO;
using System.Text;

using System.Linq;
using System.Collections;
using System.Collections.Generic;

using Mono.Cecil;
using Mono.Cecil.Cil;
using Mango.Server;

namespace Mango.Templates {

	public enum CompareOperator {
		Invalid,

		Is,
		Equal,
		NotEqual,
		GreaterThan,
		LessThan,
		GreaterThanOrEqual,
		LessThanOrEqual,
	}

	public class Target {

	}

	public class NamedTarget : Target {

		public NamedTarget (string name)
		{
			Name = name;
		}

		public string Name {
			get;
			private set;
		}
	}

	public abstract class Value {

		public TypeReference ResolvedType {
			get;
			protected set;
		}

		public abstract TypeReference ResolveType (Application app, Page page);
		public abstract void Emit (Application app, Page page, ILProcessor processor);
	}

	public class VariableValue : Value {

		public VariableValue (NamedTarget name)
		{
			Name = name;
		}

		public NamedTarget Name {
			get;
			private set;
		}

		public override TypeReference ResolveType (Application app, Page page)
		{
			ParameterDefinition pvar = page.FindParameter (Name.Name);
			if (pvar != null) {
				ResolvedType = pvar.ParameterType;
				return ResolvedType;
			}

			VariableDefinition localvar = page.FindLocalVariable (Name.Name);
			if (localvar != null) {
				ResolvedType = localvar.VariableType;
				return ResolvedType;
			}
			
			ResolvedType = app.Assembly.MainModule.Import (typeof (object));
			return ResolvedType;
		}
		
		public override void Emit (Application app, Page page, ILProcessor processor)
		{
			if (page.IsForLoopVariable (Name.Name)) {
				page.EmitForLoopVariableAccess ();
				return;
			}

			if (page.IsBuiltInVariable (Name.Name)) {
				page.EmitBuiltInVariable (Name.Name);
				return;
			}

			ParameterDefinition pvar = page.FindParameter (Name.Name);
			if (pvar != null) {
				ResolvedType = pvar.ParameterType;
				processor.Emit (OpCodes.Ldarg, pvar);
				return;
			}

			VariableDefinition localvar = page.FindLocalVariable (Name.Name);
			if (localvar != null) {
				ResolvedType = localvar.VariableType;
				processor.Emit (OpCodes.Ldloc, localvar);
				return;
			}

			// Attempt to resolve the property on the resolved type
			PropertyDefinition prop = page.Definition.Properties.Where (p => p.Name == Name.Name).FirstOrDefault ();
			if (prop != null) {
				MethodReference get_method = app.Assembly.MainModule.Import (prop.GetMethod);
				processor.Emit (OpCodes.Call, get_method);
				return;
			}
			
			//
			// Attempt to load it from the supplied type, look for a property
			// on the type with the correct name.
			//

			processor.Emit (OpCodes.Ldarg_2);
			processor.Emit (OpCodes.Callvirt, app.CommonMethods.GetTypeMethod);
			processor.Emit (OpCodes.Ldstr, Name.Name);

			processor.Emit (OpCodes.Ldc_I4, (int) (System.Reflection.BindingFlags.IgnoreCase |
					System.Reflection.BindingFlags.Instance |
				        System.Reflection.BindingFlags.Public));
			processor.Emit (OpCodes.Callvirt, app.CommonMethods.GetPropertyMethod);

			processor.Emit (OpCodes.Ldarg_2);
			processor.Emit (OpCodes.Ldnull);
			processor.Emit (OpCodes.Callvirt, app.CommonMethods.GetPropertyValueMethod);

			/*
			ResolvedType = app.Assembly.MainModule.Import (typeof (object));
			
			processor.Emit (OpCodes.Ldarg_2);
			processor.Emit (OpCodes.Ldstr, Name.Name);
			processor.Emit (OpCodes.Callvirt, app.CommonMethods.ContainsKeyMethod);

			Instruction contains_branch = processor.Emit (OpCodes.Brfalse, processor.Create (OpCodes.Nop));
			processor.Emit (OpCodes.Ldarg_2);
			processor.Emit (OpCodes.Ldstr, Name.Name);
			processor.Emit (OpCodes.Callvirt, app.CommonMethods.GetArgMethod);

			Instruction gotarg_branch = processor.Emit (OpCodes.Br, processor.Create (OpCodes.Nop));
			Instruction load_empty_target = processor.Emit (OpCodes.Ldsfld, app.CommonMethods.StringEmptyField);
			contains_branch.Operand = load_empty_target;

			Instruction completed_target = processor.Emit (OpCodes.Nop);
			gotarg_branch.Operand = completed_target;
			*/
		}
	}

	public abstract class ConstantValue : Value {

		public abstract object GetValue ();
	}

	public class ConstantStringValue : ConstantValue {

		public ConstantStringValue (string value)
		{
			Value = value;
		}

		public string Value {
			get;
			private set;
		}

		public override object GetValue ()
		{
			return Value;
		}

		public override TypeReference ResolveType (Application app, Page page)
		{
			ResolvedType = app.Assembly.MainModule.Import (typeof (string));
			return ResolvedType;
		}
		
		public override void Emit (Application app, Page page, ILProcessor processor)
		{
			processor.Emit (OpCodes.Ldstr, Value);
		}
	}

	public class ConstantIntValue : ConstantValue {

		public ConstantIntValue (int value)
		{
			Value = value;
		}

		public int Value {
			get;
			private set;
		}

		public override object GetValue ()
		{
			return Value;
		}

		public override TypeReference ResolveType (Application app, Page page)
		{
			ResolvedType = app.Assembly.MainModule.Import (typeof (int));
			return ResolvedType;
		}
		
		public override void Emit (Application app, Page page, ILProcessor processor)
		{
			processor.Emit (OpCodes.Ldc_I4, Value);
		}
	}

	public class ConstantDoubleValue : ConstantValue {

		public ConstantDoubleValue (double value)
		{
			Value = value;
		}

		public double Value {
			get;
			private set;
		}

		public override object GetValue ()
		{
			return Value;
		}

		public override TypeReference ResolveType (Application app, Page page)
		{
			ResolvedType = app.Assembly.MainModule.Import (typeof (double));
			return ResolvedType;
		}
		
		public override void Emit (Application app, Page page, ILProcessor processor)
		{
			processor.Emit (OpCodes.Ldc_R8, Value);
		}
	}

	public class PropertyAccessValue : Value {

		public PropertyAccessValue (Value target, string property)
		{
			Target = target;
			Property = property;
		}

		public Value Target {
			get;
			private set;
		}

		public string Property {
			get;
			private set;
		}

		public override TypeReference ResolveType (Application app, Page page)
		{
			PropertyDefinition prop = Target.ResolvedType.Resolve ().Properties.Where (p => p.Name == Property).FirstOrDefault ();
			if (prop != null) {
				MethodReference get_method = app.Assembly.MainModule.Import (prop.GetMethod);
				ResolvedType = prop.PropertyType;
				return ResolvedType;
			}
			
			ResolvedType = app.Assembly.MainModule.Import (typeof (object));
			return ResolvedType;
		}
		
		public override void Emit (Application app, Page page, ILProcessor processor)
		{
			Target.Emit (app, page, processor);

			// Attempt to resolve the property on the resolved type
			PropertyDefinition prop = Target.ResolvedType.Resolve ().Properties.Where (p => p.Name == Property).FirstOrDefault ();
			if (prop != null) {
				MethodReference get_method = app.Assembly.MainModule.Import (prop.GetMethod);
				processor.Emit (OpCodes.Call, get_method);
				return;
			}

			processor.Emit (OpCodes.Callvirt, app.CommonMethods.GetTypeMethod);
			processor.Emit (OpCodes.Ldstr, Property);
			processor.Emit (OpCodes.Ldc_I4, (int) (System.Reflection.BindingFlags.IgnoreCase | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public));
			processor.Emit (OpCodes.Callvirt, app.CommonMethods.GetPropertyMethod);
			Target.Emit (app, page, processor);
			processor.Emit (OpCodes.Ldnull);
			processor.Emit (OpCodes.Callvirt, app.CommonMethods.GetPropertyValueMethod);
		}

	}

	public abstract class Callable : Value {

		public string Name {
			get;
			protected set;
		}

		public List<Expression> Arguments {
			get;
			protected set;
		}
	}

	public class InvokeValue : Callable {

		public InvokeValue (string name, List<Expression> args)
		{
			Name = name;
			Arguments = args;
		}

		public override TypeReference ResolveType (Application app, Page page)
		{
			MethodDefinition meth = page.GetMethod (Name);

			ResolvedType = meth.ReturnType;
			return ResolvedType;
		}
		
		public override void Emit (Application app, Page page, ILProcessor processor)
		{
			MethodDefinition meth = page.GetMethod (Name);

			processor.Emit (OpCodes.Ldarg_0);
			processor.Emit (OpCodes.Ldarg_1);
			processor.Emit (OpCodes.Ldarg_2);

			for (int i = 2; i < meth.Parameters.Count; i++) {
				if (i - 2 < Arguments.Count) {
					Arguments [i - 2].Emit (app, page, processor);
					continue;
				}
				ParameterDefinition p = meth.Parameters [i];
				if (!p.HasConstant)
					throw new Exception ("Illegal invoke statement, incorrect number of parameters.");
				object constant = p.Constant;
				if (constant is string)
					processor.Emit (OpCodes.Ldstr, (string) constant);
				else if (constant is int)
					processor.Emit (OpCodes.Ldc_I4, (int) constant);
				else if (constant is double)
					processor.Emit (OpCodes.Ldc_R4, (double) constant);
				else
					throw new Exception (String.Format ("Illegal default argument type {0}", constant));
			}

			processor.Emit (OpCodes.Call, meth);
		}
	}

	public class Filter : Callable {

		public Filter (string name, List<Expression> args)
		{
			Name = name;
			Arguments = args;
		}

		public override TypeReference ResolveType (Application app, Page page)
		{
			ResolvedType = app.Assembly.MainModule.Import (typeof (string));
			return ResolvedType;
		}
		
		public override void Emit (Application app, Page page, ILProcessor processor)
		{
			var filter = MingeFilterManager.GetFilter (Name);

			if (filter == null)
				return;

			for (int i = 0; i < Arguments.Count; i++) {
				Arguments [i].Emit (app, page, processor);
			}

			processor.Emit (OpCodes.Call, app.Assembly.MainModule.Import (filter));
		}
	}

	public class Expression {

		private List<Filter> filters = new List<Filter> ();

		public Expression (Value value)
		{
			Value = value;
		}

		public Value Value {
			get;
			private set;
		}

		public TypeReference ResolvedType {
			get;
			private set;
		}

		public virtual TypeReference ResolveType (Application app, Page page)
		{
			if (filters.Count < 1)
				ResolvedType =  Value.ResolveType (app, page);
			else
				ResolvedType = app.Assembly.MainModule.Import (typeof (string));
			return ResolvedType;
		}
		
		public virtual void Emit (Application app, Page page, ILProcessor processor)
		{
			Value.Emit (app, page, processor);

			ResolveType (app, page);
		
			foreach (Filter filter in filters) {
				filter.Emit (app, page, processor);
			}
		}

		public void AddFilter (Filter filter)
		{
			filters.Add (filter);
		}
	}

	public class ArgumentDefinition {

		public ArgumentDefinition (string name, ConstantValue default_value)
		{
			Name = name;
			DefaultValue = default_value;
		}

		public string Name {
			get;
			private set;
		}

		public ConstantValue DefaultValue {
			get;
			private set;
		}
	}

	public class Application {

		public Application (MingeCompiler compiler, string name, string path)
		{
			Console.WriteLine ("created application:  {0}  {1}", name, path);
			Compiler = compiler;
			Name = name;
			Path = path;

			Assembly = AssemblyDefinition.CreateAssembly (new AssemblyNameDefinition (name, new Version ()),
			                                              name, ModuleKind.Dll);
			CommonMethods = new CommonMethods (Assembly);
		}

		public string Name {
			get;
			private set;
		}

		public string Path {
			get;
			private set;
		}

		public MingeCompiler Compiler {
			get;
			private set;
		}

		public AssemblyDefinition Assembly {
			get;
			private set;
		}

		public string ApplicationName {
			get;
			private set;
		}
		
		public CommonMethods CommonMethods {
			get;
			private set;
		}

		public void Save ()
		{
			Console.WriteLine ("saving:  {0}  PATH:  {1}", Assembly, Path);
			Assembly.Write (Path);
		}

		public Page CreatePage (string path)
		{
			string ns;
			string name = Page.TypeNameForPath (Name, path, out ns);

			TypeDefinition page = new TypeDefinition (name, ns.ToString (), TypeAttributes.Public | TypeAttributes.BeforeFieldInit | TypeAttributes.Serializable,
					Assembly.MainModule.Import (typeof (MingePage)));

			Assembly.MainModule.Types.Add (page);

			return new Page (this, Assembly, page, path);
		}

		public Page LoadPage (string path)
		{
			string ns;
			string name = Page.TypeNameForPath (Name, path, out ns);

			Page page = Compiler.ParsePage (path);
			return page;
		}

	}

	public class CommonMethods {

		public CommonMethods (AssemblyDefinition assembly)
		{
			WriteStringMethod = assembly.MainModule.Import (typeof (IHttpResponse).GetMethod ("Write", new Type [] { typeof (string) }));
			WriteBytesMethod = assembly.MainModule.Import (typeof (IHttpResponse).GetMethod ("Write", new Type [] { typeof (byte []) }));
			GetArgMethod = assembly.MainModule.Import (typeof (Dictionary<string,object>).GetMethod ("get_Item", new Type [] { typeof (string) }));
			ToStringMethod = assembly.MainModule.Import (typeof (object).GetMethod ("ToString"));
			FormatStringMethod = assembly.MainModule.Import (typeof (string).GetMethod ("Format", new Type [] { typeof (string), typeof (object []) }));
			ContainsKeyMethod = assembly.MainModule.Import (typeof (Dictionary<string,object>).GetMethod ("ContainsKey"));
			GetEnumeratorMethod = assembly.MainModule.Import (typeof (System.Collections.IEnumerable).GetMethod ("GetEnumerator"));
			EnumeratorMoveNextMethod = assembly.MainModule.Import (typeof (System.Collections.IEnumerator).GetMethod ("MoveNext"));
			EnumeratorGetCurrentMethod = assembly.MainModule.Import (typeof (System.Collections.IEnumerator).GetMethod ("get_Current"));

			GetTypeMethod = assembly.MainModule.Import (typeof (object).GetMethod ("GetType"));
			GetPropertyMethod = assembly.MainModule.Import (typeof (Type).GetMethod ("GetProperty", new Type [] { typeof (string), typeof (System.Reflection.BindingFlags) }));
			GetPropertyValueMethod = assembly.MainModule.Import (typeof (System.Reflection.PropertyInfo).GetMethod ("GetValue", new Type [] { typeof (object), typeof (object []) }));

			IsNullOrEmptyMethod = assembly.MainModule.Import (typeof (string).GetMethod ("IsNullOrEmpty"));
			StringEmptyField = assembly.MainModule.Import (typeof (string).GetField ("Empty"));

			ConsoleWriteLineMethod = assembly.MainModule.Import (typeof (Console).GetMethod ("WriteLine", new Type [] { typeof (object) }));
		}

		public MethodReference ConsoleWriteLineMethod {
			get;
			private set;
		}
			
		public MethodReference GetArgMethod {
			get;
			private set;
		}

		public MethodReference ContainsKeyMethod {
			get;
			private set;
		}

		public MethodReference ToStringMethod {
			get;
			private set;
		}

		public MethodReference WriteStringMethod {
			get;
			private set;
		}

		public MethodReference WriteBytesMethod {
			get;
			private set;
		}

		public MethodReference FormatStringMethod {
			get;
			private set;
		}

		public MethodReference GetEnumeratorMethod {
			get;
			private set;
		}

		public MethodReference EnumeratorMoveNextMethod {
			get;
			private set;
		}

		public MethodReference EnumeratorGetCurrentMethod {
			get;
			private set;
		}

		public MethodReference GetTypeMethod {
			get;
			private set;
		}

		public MethodReference GetPropertyMethod {
			get;
			private set;
		}

		public MethodReference GetPropertyValueMethod {
			get;
			private set;
		}

		public MethodReference IsNullOrEmptyMethod {
			get;
			private set;
		}

		public FieldReference StringEmptyField {
			get;
			private set;
		}
	}

	public class Page {

		private string path;
		private Application application;
		private AssemblyDefinition assembly;
		private MethodDefinition render_to_stream;
		private MethodDefinition ctor;

		private Page base_type;
		private Instruction first_instruction;

		private Stack<MethodDefinition> method_stack;
		private Stack<ForLoopContext> forloop_stack;
		
		public Page ()
		{
		}
		
		public Page (Application application, AssemblyDefinition assembly, TypeDefinition definition, string path)
		{
			this.application = application;
			this.assembly = assembly;
			this.path = path;

			Definition = definition;

			ctor = new MethodDefinition (".ctor", MethodAttributes.Public, assembly.MainModule.Import (typeof (void)));
			Definition.Methods.Add (ctor);

			ValueToStringMethod = AddValueToStringMethod ();

			render_to_stream = AddRenderToResponseMethod ("RenderToResponse");
			ILProcessor p = render_to_stream.Body.GetILProcessor ();

			first_instruction = p.Create (OpCodes.Nop);
			
			p.Append (first_instruction);

			method_stack = new Stack<MethodDefinition> ();
			method_stack.Push (render_to_stream);

			forloop_stack = new Stack<ForLoopContext> ();
		}

		public TypeDefinition Definition {
			get;
			private set;
		}

		public MethodDefinition ValueToStringMethod {
			get;
			private set;
		}

		public MethodDefinition CurrentMethod {
			get {
				return method_stack.Peek ();
			}
		}

		public bool IsChildTemplate {
			get {
				return base_type != null && method_stack.Count == 1;
			}
		}

		public bool InForLoop {
			get {
				return forloop_stack.Count > 0;
			}
		}

		public void AddData (string data)
		{
			if (IsChildTemplate)
				return;

			ILProcessor processor = CurrentMethod.Body.GetILProcessor ();

			processor.Emit (OpCodes.Ldarg_1);
			processor.Emit (OpCodes.Ldstr, data);
			processor.Emit (OpCodes.Callvirt, application.CommonMethods.WriteStringMethod);
		}

		public void EmitExtends (string base_template)
		{
			if (base_type != null)
				throw new Exception (String.Format ("Multiple extends statements are not allowed. ({0})", base_type));

			base_type = application.LoadPage (base_template);

			if (base_type == null)
				throw new Exception ("Could not find base.");

			Definition.BaseType = base_type.Definition;
			EmitBaseRenderToResponseCall ();
		}

		private void EmitBaseRenderToResponseCall ()
		{
			ILProcessor processor = method_stack.Last ().Body.GetILProcessor ();

			MethodReference base_render = base_type.Definition.Methods.Where (m => m.Name == "RenderToResponse").FirstOrDefault ();

			processor.InsertAfter (first_instruction, processor.Create (OpCodes.Ret));
			processor.InsertAfter (first_instruction, processor.Create (OpCodes.Call, base_render));
			processor.InsertAfter (first_instruction, processor.Create (OpCodes.Ldarg_2));
			processor.InsertAfter (first_instruction, processor.Create (OpCodes.Ldarg_1));
			processor.InsertAfter (first_instruction, processor.Create (OpCodes.Ldarg_0));
		}

		public void BeginBlock (string name)
		{
			MethodDefinition meth = GetMethod (name);

			if (meth != null)
				throw new Exception (String.Format ("Invalid block name {0} the name is already in use.", name));

			meth = AddRenderToResponseMethod (name);

			ILProcessor processor = CurrentMethod.Body.GetILProcessor ();
			processor.Emit (OpCodes.Ldarg_0);
			processor.Emit (OpCodes.Ldarg_1);
			processor.Emit (OpCodes.Ldarg_2);
			Instruction block_call = processor.Create (OpCodes.Callvirt, meth);
			processor.Append (block_call);
			
			method_stack.Push (meth);
		}

		public void EndBlock (string name)
		{
			if (name != null && CurrentMethod.Name != name)
				throw new Exception (String.Format ("Unmatched block names, expected {0} but got {1}", CurrentMethod.Name, name));

			ILProcessor processor = CurrentMethod.Body.GetILProcessor ();
			processor.Emit (OpCodes.Ret);

			method_stack.Pop ();
		}

		public void EmitPrint (List<Expression> expressions)
		{
			if (IsChildTemplate)
				return;

			if (expressions.Count == 1) {
				EmitSinglePrint (expressions [0]);
				return;
			}

			ILProcessor processor = CurrentMethod.Body.GetILProcessor ();

			StringBuilder format_str = new StringBuilder ();
			for (int i = 0; i < expressions.Count; i++) {
				format_str.AppendFormat ("{{0}} ", i);
			}

			processor.Emit (OpCodes.Ldarg_1);
			processor.Emit (OpCodes.Ldstr, format_str.ToString ());

			processor.Emit (OpCodes.Ldc_I4, expressions.Count);
			processor.Emit (OpCodes.Newarr, assembly.MainModule.Import (typeof (object)));

			for (int i = 0; i < expressions.Count; i++) {
				processor.Emit (OpCodes.Dup);
				processor.Emit (OpCodes.Ldc_I4, i);
				expressions [i].Emit (application, this, processor);
				EmitToString (application, this, processor, expressions [i].ResolvedType);
				processor.Emit (OpCodes.Stelem_Ref);
			}
			processor.Emit (OpCodes.Call, application.CommonMethods.FormatStringMethod);
			processor.Emit (OpCodes.Callvirt, application.CommonMethods.WriteStringMethod);
		}

		public void EmitToString (Application app, Page page, ILProcessor processor, TypeReference resolved)
		{
			if (resolved.FullName == "System.String")
				return;

			if (resolved.FullName == "System.Void") {
				processor.Emit (OpCodes.Ldsfld, application.CommonMethods.StringEmptyField);
				return;
			}

			if (resolved.IsValueType) {
				processor.Emit (OpCodes.Box, app.Assembly.MainModule.Import (resolved));
				processor.Emit (OpCodes.Call, app.Assembly.MainModule.Import (page.ValueToStringMethod));
				return;
			}

			TypeDefinition rtype = resolved.Resolve ();
			MethodReference method = rtype.Methods.Where (m => m.Name == "ToString").First ();

			// Import it so we get a method reference
			method = application.Assembly.MainModule.Import (method);
			Instruction inst = processor.Create (OpCodes.Callvirt, (MethodReference) method);
			processor.Append (inst);
		}

		public void EmitSinglePrint (Expression expression)
		{
			if (IsChildTemplate)
				return;

			ILProcessor processor = CurrentMethod.Body.GetILProcessor ();

			processor.Emit (OpCodes.Ldarg_1);
			expression.Emit (application, this, processor);
			EmitToString (application, this, processor, expression.ResolvedType);
			processor.Emit (OpCodes.Callvirt, application.CommonMethods.WriteStringMethod);
		}

		public void EmitSet (NamedTarget target, Expression expression)
		{
			

			expression.ResolveType (application, this);
			
			PropertyDefinition property = FindProperty (target.Name, expression.ResolvedType);
			if (property == null)
				property = AddProperty (target.Name, expression.ResolvedType);

			//
			// Property setting happens in the ctor
			//
			
			ILProcessor processor = ctor.Body.GetILProcessor ();
			processor.Emit (OpCodes.Ldarg_0);
			expression.Emit (application, this, processor);
			processor.Emit (OpCodes.Call, property.SetMethod);
		}

		public void BeginMacro (string name, List<ArgumentDefinition> args)
		{
			MethodDefinition meth = AddRenderToResponseMethod (name, args);

			method_stack.Push (meth);
		}

		public void EndMacro (string name)
		{
			if (name != null && CurrentMethod.Name != name)
				throw new Exception (String.Format ("Unmatched macro names, expected {0} but got {1}", CurrentMethod.Name, name));

			ILProcessor processor = CurrentMethod.Body.GetILProcessor ();
			processor.Emit (OpCodes.Ret);

			method_stack.Pop ();
		}

		private class IfContext {

			public IfContext ()
			{
				NextConditionalBranches = new List<Instruction> ();
				EndIfBranches = new List<Instruction> ();
			}

			public void UpdateNextConditionalBranches (Instruction new_target)
			{
				foreach (Instruction inst in NextConditionalBranches) {
					inst.Operand = new_target;
				}
				NextConditionalBranches.Clear ();
			}

			public void UpdateEndIfBranches (Instruction new_target)
			{
				foreach (Instruction inst in EndIfBranches) {
					inst.Operand = new_target;
				}
				EndIfBranches.Clear ();
			}

			public List<Instruction> NextConditionalBranches {
				get;
				private set;
			}

			public List<Instruction> EndIfBranches {
				get;
				private set;
			}
		}

		private Stack<IfContext> ifstack = new Stack<IfContext> ();

		public void EmitIf (Expression expression)
		{
			if (IsChildTemplate)
				return;

			TypeReference string_type = application.Assembly.MainModule.Import (typeof (string));

			ILProcessor processor = CurrentMethod.Body.GetILProcessor ();
			expression.Emit (application, this, processor);
			Instruction null_branch = processor.Create (OpCodes.Brfalse, processor.Create (OpCodes.Nop));
			processor.Append (null_branch);

			expression.Emit (application, this, processor);
			processor.Emit (OpCodes.Isinst, string_type);
			Instruction isstr_branch = processor.Create (OpCodes.Brfalse, processor.Create (OpCodes.Nop));
			processor.Append (isstr_branch);

			expression.Emit (application, this, processor);
			processor.Emit (OpCodes.Castclass, string_type);
			processor.Emit (OpCodes.Call, application.CommonMethods.IsNullOrEmptyMethod);
			Instruction empty_branch = processor.Create (OpCodes.Brtrue, processor.Create (OpCodes.Nop));
			processor.Append (empty_branch);

			isstr_branch.Operand = processor.Create (OpCodes.Nop);
			processor.Append ((Instruction) isstr_branch.Operand);

			IfContext ifcontext = new IfContext ();
			ifcontext.NextConditionalBranches.Add (null_branch);
			ifcontext.NextConditionalBranches.Add (empty_branch);

			ifstack.Push (ifcontext);
		}

		public void EmitElseIf (Expression expression)
		{
			if (IsChildTemplate)
				return;
		}

		public void EmitElse ()
		{
			if (IsChildTemplate)
				return;

			IfContext ifcontext = ifstack.Peek ();

			ILProcessor processor = CurrentMethod.Body.GetILProcessor ();

			Instruction branch_to_end = processor.Create (OpCodes.Br, processor.Create (OpCodes.Nop));
			Instruction begin_else = processor.Create (OpCodes.Nop);

			processor.Append (branch_to_end);
			processor.Append (begin_else);
			
			ifcontext.UpdateNextConditionalBranches (begin_else);
			ifcontext.EndIfBranches.Add (branch_to_end);
		}

		public void EmitEndIf ()
		{
			if (IsChildTemplate)
				return;

			IfContext ifcontext = ifstack.Pop ();

			ILProcessor processor = CurrentMethod.Body.GetILProcessor ();

			Instruction end_branch = processor.Create (OpCodes.Nop);
			processor.Append (end_branch);
			
			ifcontext.UpdateNextConditionalBranches (end_branch);
			ifcontext.UpdateEndIfBranches (end_branch);
		}

		private class ForLoopContext {

			public ForLoopContext (string varname, Instruction begin_loop, VariableDefinition enumvar)
			{
				VariableName = varname;
				BeginLoopInstruction = begin_loop;
				EnumeratorVariable = enumvar;
			}

			public string VariableName {
				get;
				private set;
			}

			public Instruction BeginLoopInstruction {
				get;
				private set;
			}

			public VariableDefinition EnumeratorVariable {
				get;
				private set;
			}
		}

		public void BeginForLoop (string name, Expression expression)
		{
			if (IsChildTemplate)
				return;

			TypeReference enum_type = assembly.MainModule.Import (typeof (System.Collections.IEnumerable));

			string local_enum_name = String.Format ("__enum_{0}", forloop_stack.Count);
			VariableDefinition enumeratorvar = FindLocalVariable (local_enum_name);
			if (enumeratorvar == null) {
				Mono.Collections.Generic.Collection<VariableDefinition> vars = CurrentMethod.Body.Variables;
				enumeratorvar = new VariableDefinition (local_enum_name, enum_type);
				vars.Add (enumeratorvar);
			}

			ILProcessor processor = CurrentMethod.Body.GetILProcessor ();
			expression.Emit (application, this, processor);
			processor.Emit (OpCodes.Castclass, assembly.MainModule.Import (typeof (System.Collections.IEnumerable)));
			processor.Emit (OpCodes.Callvirt, application.CommonMethods.GetEnumeratorMethod);
			
			processor.Emit (OpCodes.Stloc, enumeratorvar);

			Instruction begin_loop = processor.Create (OpCodes.Br, processor.Create (OpCodes.Nop));
			processor.Append (begin_loop);
			
			forloop_stack.Push (new ForLoopContext (name, begin_loop, enumeratorvar));

			processor.Emit (OpCodes.Nop);
		}

		public void EndForLoop ()
		{
			if (IsChildTemplate)
				return;

			ForLoopContext forloop = forloop_stack.Pop ();

			ILProcessor processor = CurrentMethod.Body.GetILProcessor ();

			Instruction enter_loop = processor.Create (OpCodes.Ldloc, forloop.EnumeratorVariable);
			processor.Append (enter_loop);
			
			processor.Emit (OpCodes.Callvirt, application.CommonMethods.EnumeratorMoveNextMethod);
			processor.Emit (OpCodes.Brtrue, forloop.BeginLoopInstruction.Next);

			forloop.BeginLoopInstruction.Operand = enter_loop;
		}

		private void CloseCtor ()
		{
			ctor.Body.GetILProcessor ().Emit (OpCodes.Ret);
		}
		
		public void Save ()
		{
			CloseCtor ();
			
			ILProcessor processor = CurrentMethod.Body.GetILProcessor ();
			processor.Emit (OpCodes.Ret);
		}

		public bool IsForLoopVariable (string name)
		{
			if (forloop_stack.Count == 0)
				return false;

			ForLoopContext forloop = forloop_stack.Peek ();

			return name == forloop.VariableName;
		}

		public void EmitForLoopVariableAccess ()
		{
			ILProcessor processor = CurrentMethod.Body.GetILProcessor ();

			ForLoopContext forloop = forloop_stack.Peek ();
			processor.Emit (OpCodes.Ldloc, forloop.EnumeratorVariable);
			processor.Emit (OpCodes.Callvirt, application.CommonMethods.EnumeratorGetCurrentMethod);
		}

		public bool IsBuiltInVariable (string name)
		{
			bool res = false;

			switch (name) {
			case "template_path":
				res = true;
				break;
			}

			return res;
		}
				
		public void EmitBuiltInVariable (string name)
		{
			ILProcessor processor = CurrentMethod.Body.GetILProcessor ();

			switch (name) {
			case "template_path":
				processor.Emit (OpCodes.Ldstr, path);
				break;
			}
		}

		public ParameterDefinition FindParameter (string name)
		{
			if (!CurrentMethod.HasParameters)
				return null;

			foreach (ParameterDefinition p in CurrentMethod.Parameters) {
				if (p.Name == name)
					return p;
			}

			return null;
		}

		public VariableDefinition FindLocalVariable (string name)
		{
			if (!CurrentMethod.Body.HasVariables)
				return null;
			return FindVariable (name, CurrentMethod.Body.Variables);
		}

		private VariableDefinition FindVariable (string name, Mono.Collections.Generic.Collection<VariableDefinition> variables)
		{
			foreach (VariableDefinition variable in variables) {
				if (variable.Name == name)
					return variable;
			}

			return null;
		}

		public PropertyDefinition FindProperty (string name, TypeReference t)
		{
			TypeDefinition klass = base_type != null ? base_type.Definition : Definition;
			
			if (!klass.HasProperties)
				return null;
			return klass.Properties.Where (p => name == p.Name).FirstOrDefault ();
		}

		public PropertyDefinition AddProperty (string name, TypeReference t)
		{
			TypeDefinition klass = base_type != null ? base_type.Definition : Definition;
			
			PropertyDefinition prop = new PropertyDefinition (name, (PropertyAttributes) 0, t);
			klass.Properties.Add (prop);

			FieldDefinition field = new FieldDefinition ("mango_" + name, FieldAttributes.Private | FieldAttributes.CompilerControlled, t);
			klass.Fields.Add (field);
			
			prop.GetMethod = new MethodDefinition ("get_" + name, MethodAttributes.CompilerControlled | MethodAttributes.Public, t);
			klass.Methods.Add (prop.GetMethod);
			ILProcessor processor = prop.GetMethod.Body.GetILProcessor ();
			processor.Emit (OpCodes.Ldarg_0);
			processor.Emit (OpCodes.Ldfld, field);
			processor.Emit (OpCodes.Ret);
			
			prop.SetMethod = new MethodDefinition ("set_" + name, MethodAttributes.CompilerControlled | MethodAttributes.Public, t.Module.Import (typeof (void)));
			prop.SetMethod.Parameters.Add (new ParameterDefinition (t));
			klass.Methods.Add (prop.SetMethod);
			processor = prop.SetMethod.Body.GetILProcessor ();
			processor.Emit (OpCodes.Ldarg_0);
			processor.Emit (OpCodes.Ldarg_1);
			processor.Emit (OpCodes.Stfld, field);
			processor.Emit (OpCodes.Ret);
			
			return prop;
		}

		public MethodDefinition GetMethod (string name)
		{
			MethodDefinition method = Definition.Methods.Where (m => m.Name == name).FirstOrDefault ();

			return method;
		}

		public MethodDefinition AddRenderToResponseMethod (string name, List<ArgumentDefinition> extra_args=null)
		{
			MethodAttributes atts = MethodAttributes.Public | MethodAttributes.Virtual;

			MethodDefinition render = new MethodDefinition (name, atts, assembly.MainModule.Import (typeof (void)));

			render.Parameters.Add (new ParameterDefinition ("stream", (ParameterAttributes) 0,
					assembly.MainModule.Import (typeof (IHttpResponse))));
			render.Parameters.Add (new ParameterDefinition ("args", (ParameterAttributes) 0,
					assembly.MainModule.Import (typeof (object))));

			if (extra_args != null) {
				TypeReference object_type = assembly.MainModule.Import (typeof (object));
				foreach (ArgumentDefinition arg in extra_args) {
					ParameterDefinition pdef = new ParameterDefinition (arg.Name, (ParameterAttributes) 0, object_type);
					if (arg.DefaultValue != null) {
						pdef.Constant = arg.DefaultValue.GetValue ();
						pdef.HasDefault = true;
					}
					render.Parameters.Add (pdef);
				}
			}

			Definition.Methods.Add (render);

			return render;
		}

		public MethodDefinition AddValueToStringMethod ()
		{
			MethodAttributes atts = MethodAttributes.Public | MethodAttributes.Static;

			MethodDefinition to_string = new MethodDefinition ("ValueToString", atts, assembly.MainModule.Import (typeof (string)));
			to_string.Parameters.Add (new ParameterDefinition ("the_value", (ParameterAttributes) 0, assembly.MainModule.Import (typeof (System.ValueType))));

			Definition.Methods.Add (to_string);

			ILProcessor processor = to_string.Body.GetILProcessor ();
			processor.Emit (OpCodes.Ldarg_0);
			processor.Emit (OpCodes.Callvirt, assembly.MainModule.Import (typeof (System.ValueType).GetMethod ("ToString")));
			processor.Emit (OpCodes.Ret);
					
			return to_string;
		}

		public static string TypeNameForPath (string app_name, string path, out string name_space)
		{
			if (String.IsNullOrEmpty (app_name))
				throw new ArgumentNullException ("app_name");
			if (String.IsNullOrEmpty (path))
				throw new ArgumentNullException ("path");
			
			if (Path.IsPathRooted (path))
				throw new ArgumentException ("Attempt to create a type name from a rooted path.");
			
			StringBuilder res = new StringBuilder ();
			
			res.Append (app_name);
			res.Append (".Templates.");

			int start = res.Length;
			int last = -1;
			int ext = path.LastIndexOf ('.');

			if (ext < 2)
				throw new ArgumentException ("Path must have an extension.");

			for (int i = 0; i < ext; i++) {
				// Don't use Path.DirectorySepChar because we always want '/' to work
				if (path [i] == '/' || path [i] == '\\') {
					if (last == i - 1)
						throw new ArgumentException ("Template paths can not have multiple consecutive '.'s");
					res.Append ('.');
					last = i;
					continue;
				}
				if (path [i] == '.') {
					if (last == i - 1 || i == ext - 1)
						throw new ArgumentException ("Template paths can not have multiple consecutive '.'s");
					res.Append ('.');
					last = i;
					continue;
				}
				if (!Char.IsLetterOrDigit (path [i])) {
					res.Append (path [i]);
					continue;
				}
				if (last == i - 1) {
					res.Append (Char.ToUpper (path [i]));
					continue;
				}
				res.Append (Char.ToLower (path [i]));
			}

			res.Append (Char.ToUpper (path [ext + 1]));

			for (int i = ext + 2; i < path.Length; i++) {
				if (path [i] == '/' || path [i] == '\\')
					throw new ArgumentException ("Invalid Path, no extension found.");
				res.Append (Char.ToLower (path [i]));					
			}

			name_space = res.ToString (0, last + start);
			string type = res.ToString (last + start + 1, res.Length - last - start -1);

			Console.WriteLine ("full name:  {0}", res.ToString ());
			Console.WriteLine ("namespace:  {0}", name_space);
			Console.WriteLine ("type name:  {0}", type);

			return type;
		}

		public static string FullTypeNameForPath (string app_name, string path)
		{
			string ns;
			string name = TypeNameForPath (app_name, path, out ns);

			return String.Concat (ns, ".", name);
		}
	}
}

