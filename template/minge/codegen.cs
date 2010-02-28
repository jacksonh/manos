
using System;
using System.IO;
using System.Text;
using System.Linq;
using System.Collections.Generic;

using Mono.Cecil;
using Mono.Cecil.Cil;


namespace Mango.Templates.Minge {

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

		public abstract void Emit (Application app, Page page, CilWorker worker);

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

		public override void Emit (Application app, Page page, CilWorker worker)
		{
			if (page.IsForLoopVariable (Name.Name)) {
				ResolvedType = app.Assembly.MainModule.Import (typeof (object)); 
				page.EmitForLoopVariableAccess ();
				return;
			}

			ParameterDefinition pvar = page.FindParameter (Name.Name);
			if (pvar != null) {
				ResolvedType = pvar.ParameterType;
				worker.Emit (OpCodes.Ldarg, pvar);
				return;
			}

			VariableDefinition localvar = page.FindLocalVariable (Name.Name);
			if (localvar != null) {
				ResolvedType = localvar.VariableType;
				worker.Emit (OpCodes.Ldloc, localvar);
				return;
			}

			FieldDefinition pagefield = page.FindField (Name.Name);
			if (pagefield != null) {
				ResolvedType = pagefield.FieldType;
				worker.Emit (OpCodes.Ldarg_0);
				worker.Emit (OpCodes.Ldfld, pagefield);
				return;
			}

			//
			// Attempt to load it from the dictionary
			//

			ResolvedType = app.Assembly.MainModule.Import (typeof (object));
			
			worker.Emit (OpCodes.Ldarg_2);
			worker.Emit (OpCodes.Ldstr, Name.Name);
			worker.Emit (OpCodes.Callvirt, app.CommonMethods.ContainsKeyMethod);

			Instruction contains_branch = worker.Emit (OpCodes.Brfalse, worker.Create (OpCodes.Nop));
			worker.Emit (OpCodes.Ldarg_2);
			worker.Emit (OpCodes.Ldstr, Name.Name);
			worker.Emit (OpCodes.Callvirt, app.CommonMethods.GetArgMethod);

			Instruction gotarg_branch = worker.Emit (OpCodes.Br, worker.Create (OpCodes.Nop));
			Instruction load_empty_target = worker.Emit (OpCodes.Ldsfld, app.CommonMethods.StringEmptyField);
			contains_branch.Operand = load_empty_target;

			Instruction completed_target = worker.Emit (OpCodes.Nop);
			gotarg_branch.Operand = completed_target;
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

		public override void Emit (Application app, Page page, CilWorker worker)
		{
			worker.Emit (OpCodes.Ldstr, Value);
			ResolvedType = app.Assembly.MainModule.Import (typeof (string));
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

		public override void Emit (Application app, Page page, CilWorker worker)
		{
			worker.Emit (OpCodes.Ldc_I4, Value);
			ResolvedType = app.Assembly.MainModule.Import (typeof (int));
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

		public override void Emit (Application app, Page page, CilWorker worker)
		{
			worker.Emit (OpCodes.Ldc_R8, Value);
			ResolvedType = app.Assembly.MainModule.Import (typeof (double));
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

		public override void Emit (Application app, Page page, CilWorker worker)
		{
			Target.Emit (app, page, worker);

			// Attempt to resolve the property on the resolved type
			PropertyDefinition [] props = Target.ResolvedType.Resolve ().Properties.GetProperties (Property);
			if (props.Length > 0) {
				MethodReference get_method = app.Assembly.MainModule.Import (props [0].GetMethod);
				worker.Emit (OpCodes.Call, get_method);
				ResolvedType = props [0].PropertyType;
				return;
			}
			
			ResolvedType = app.Assembly.MainModule.Import (typeof (object));

			worker.Emit (OpCodes.Callvirt, app.CommonMethods.GetTypeMethod);
			worker.Emit (OpCodes.Ldstr, Property);
			worker.Emit (OpCodes.Callvirt, app.CommonMethods.GetPropertyMethod);
			Target.Emit (app, page, worker);
			worker.Emit (OpCodes.Ldnull);
			worker.Emit (OpCodes.Callvirt, app.CommonMethods.GetPropertyValueMethod);
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

		public override void Emit (Application app, Page page, CilWorker worker)
		{
			MethodDefinition meth = page.GetMethod (Name);

			ResolvedType = meth.ReturnType.ReturnType;

			worker.Emit (OpCodes.Ldarg_0);
			worker.Emit (OpCodes.Ldarg_1);
			worker.Emit (OpCodes.Ldarg_2);

			for (int i = 2; i < meth.Parameters.Count; i++) {
				if (i - 2 < Arguments.Count) {
					Arguments [i - 2].Emit (app, page, worker);
					continue;
				}
				ParameterDefinition p = meth.Parameters [i];
				if (!p.HasConstant)
					throw new Exception ("Illegal invoke statement, incorrect number of parameters.");
				object constant = p.Constant;
				if (constant is string)
					worker.Emit (OpCodes.Ldstr, (string) constant);
				else if (constant is int)
					worker.Emit (OpCodes.Ldc_I4, (int) constant);
				else if (constant is double)
					worker.Emit (OpCodes.Ldc_R4, (double) constant);
				else
					throw new Exception (String.Format ("Illegal default argument type {0}", constant));
			}

			worker.Emit (OpCodes.Call, meth);
		}
	}

	public class Filter : Callable {

		public Filter (string name, List<Expression> args)
		{
			Name = name;
			Arguments = args;
		}

		public override void Emit (Application app, Page page, CilWorker worker)
		{
			var filter = MingeFilterManager.GetFilter (Name);

			if (filter == null)
				return;

			for (int i = 0; i < Arguments.Count; i++) {
				Arguments [i].Emit (app, page, worker);
			}

			worker.Emit (OpCodes.Call, app.Assembly.MainModule.Import (filter));
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

		public virtual void Emit (Application app, Page page, CilWorker worker)
		{
			Value.Emit (app, page, worker);

			if (filters.Count < 1)
				ResolvedType = Value.ResolvedType;
			else
				ResolvedType = app.Assembly.MainModule.Import (typeof (string));

			foreach (Filter filter in filters) {
				filter.Emit (app, page, worker);
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

		public Application (MingeContext context, string name, string path)
		{
			Context = context;
			Name = name;
			Path = path;

			Assembly = AssemblyFactory.DefineAssembly (name, AssemblyKind.Dll);
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

		public MingeContext Context {
			get;
			private set;
		}

		public AssemblyDefinition Assembly {
			get;
			private set;
		}

		public CommonMethods CommonMethods {
			get;
			private set;
		}

		public void Save ()
		{
			AssemblyFactory.SaveAssembly (Assembly, Path);
		}

		public Page CreatePage (string path)
		{
			string ns;
			string name = Page.NameForPath (path, out ns);

			TypeDefinition page = new TypeDefinition (name, ns.ToString (), TypeAttributes.Public | TypeAttributes.BeforeFieldInit | TypeAttributes.Serializable,
					Assembly.MainModule.Import (typeof (MingePage)));

			Assembly.MainModule.Types.Add (page);

			return new Page (this, Assembly, page);
		}

		public Page LoadPage (string path)
		{
			string ns;
			string name = Page.NameForPath (path, out ns);

			TypeDefinition def = Assembly.MainModule.Types [String.Concat (ns, ".", name)];

			// if (def != null)
			//	return def;

			Page page = Context.ParsePage (path);
			// MingeParser parser = new MingeParser (Environment, this);
			// Page page = parser.ParsePage (path, new StreamReader (File.OpenRead (path)));

			return page;
		}

	}

	public class CommonMethods {

		public CommonMethods (AssemblyDefinition assembly)
		{
			WriteStringMethod = assembly.MainModule.Import (typeof (System.IO.TextWriter).GetMethod ("Write", new Type [] { typeof (string) }));
			GetArgMethod = assembly.MainModule.Import (typeof (Dictionary<string,object>).GetMethod ("get_Item", new Type [] { typeof (string) }));
			ToStringMethod = assembly.MainModule.Import (typeof (object).GetMethod ("ToString"));
			FormatStringMethod = assembly.MainModule.Import (typeof (string).GetMethod ("Format", new Type [] { typeof (string), typeof (object []) }));
			ContainsKeyMethod = assembly.MainModule.Import (typeof (Dictionary<string,object>).GetMethod ("ContainsKey"));
			GetEnumeratorMethod = assembly.MainModule.Import (typeof (System.Collections.IEnumerable).GetMethod ("GetEnumerator"));
			EnumeratorMoveNextMethod = assembly.MainModule.Import (typeof (System.Collections.IEnumerator).GetMethod ("MoveNext"));
			EnumeratorGetCurrentMethod = assembly.MainModule.Import (typeof (System.Collections.IEnumerator).GetMethod ("get_Current"));

			GetTypeMethod = assembly.MainModule.Import (typeof (object).GetMethod ("GetType"));
			GetPropertyMethod = assembly.MainModule.Import (typeof (Type).GetMethod ("GetProperty", new Type [] { typeof (string) }));
			GetPropertyValueMethod = assembly.MainModule.Import (typeof (System.Reflection.PropertyInfo).GetMethod ("GetValue", new Type [] { typeof (object), typeof (object []) }));

			IsNullOrEmptyMethod = assembly.MainModule.Import (typeof (string).GetMethod ("IsNullOrEmpty"));
			StringEmptyField = assembly.MainModule.Import (typeof (string).GetField ("Empty"));
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

		private Application application;
		private AssemblyDefinition assembly;
		private MethodDefinition render;

		private Page base_type;
		private Instruction first_instruction;

		private Stack<MethodDefinition> method_stack;
		private Stack<ForLoopContext> forloop_stack;
		
		public Page (Application application, AssemblyDefinition assembly, TypeDefinition definition)
		{
			this.application = application;
			this.assembly = assembly;

			Definition = definition;

			MethodDefinition ctor = new MethodDefinition (".ctor", MethodAttributes.Public, assembly.MainModule.Import (typeof (void)));
			Definition.Methods.Add (ctor);
			ctor.Body.CilWorker.Emit (OpCodes.Ret);

			ValueToStringMethod = AddValueToStringMethod ();

			render = AddRenderMethod ("RenderToStream");
			first_instruction = render.Body.CilWorker.Emit (OpCodes.Nop);

			method_stack = new Stack<MethodDefinition> ();
			method_stack.Push (render);

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

			CilWorker worker = CurrentMethod.Body.CilWorker;

			worker.Emit (OpCodes.Ldarg_1);
			worker.Emit (OpCodes.Ldstr, data);
			worker.Emit (OpCodes.Callvirt, application.CommonMethods.WriteStringMethod);
		}

		public void EmitExtends (string base_template)
		{
			if (base_type != null)
				throw new Exception (String.Format ("Multiple extends statements are not allowed. ({0})", base_type));

			base_type = application.LoadPage (base_template);

			if (base_type == null)
				throw new Exception ("Could not find base.");

			Definition.BaseType = base_type.Definition;
			EmitBaseRenderToStreamCall ();
		}

		private void EmitBaseRenderToStreamCall ()
		{
			CilWorker worker = method_stack.Last ().Body.CilWorker;

			MethodReference base_render = base_type.Definition.Methods.GetMethod ("RenderToStream") [0];

			worker.InsertAfter (first_instruction, worker.Create (OpCodes.Ret));
			worker.InsertAfter (first_instruction, worker.Create (OpCodes.Call, base_render));
			worker.InsertAfter (first_instruction, worker.Create (OpCodes.Ldarg_2));
			worker.InsertAfter (first_instruction, worker.Create (OpCodes.Ldarg_1));
			worker.InsertAfter (first_instruction, worker.Create (OpCodes.Ldarg_0));
		}

		public void BeginBlock (string name)
		{
			MethodDefinition meth = GetMethod (name);

			if (meth != null)
				throw new Exception (String.Format ("Invalid block name {0} the name is already in use.", name));

			meth = AddRenderMethod (name);

			CilWorker worker = CurrentMethod.Body.CilWorker;
			worker.Emit (OpCodes.Ldarg_0);
			worker.Emit (OpCodes.Ldarg_1);
			worker.Emit (OpCodes.Ldarg_2);
			Instruction block_call = worker.Emit (OpCodes.Callvirt, meth);

			method_stack.Push (meth);
		}

		public void EndBlock (string name)
		{
			if (name != null && CurrentMethod.Name != name)
				throw new Exception (String.Format ("Unmatched block names, expected {0} but got {1}", CurrentMethod.Name, name));

			CilWorker worker = CurrentMethod.Body.CilWorker;
			worker.Emit (OpCodes.Ret);

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

			CilWorker worker = CurrentMethod.Body.CilWorker;

			StringBuilder format_str = new StringBuilder ();
			for (int i = 0; i < expressions.Count; i++) {
				format_str.AppendFormat ("{{0}} ", i);
			}

			worker.Emit (OpCodes.Ldarg_1);
			worker.Emit (OpCodes.Ldstr, format_str.ToString ());

			worker.Emit (OpCodes.Ldc_I4, expressions.Count);
			worker.Emit (OpCodes.Newarr, assembly.MainModule.Import (typeof (object)));

			for (int i = 0; i < expressions.Count; i++) {
				worker.Emit (OpCodes.Dup);
				worker.Emit (OpCodes.Ldc_I4, i);
				expressions [i].Emit (application, this, worker);
				EmitToString (application, this, worker, expressions [i].ResolvedType);
				worker.Emit (OpCodes.Stelem_Ref);
			}
			worker.Emit (OpCodes.Call, application.CommonMethods.FormatStringMethod);
			worker.Emit (OpCodes.Callvirt, application.CommonMethods.WriteStringMethod);
		}

		public void EmitToString (Application app, Page page, CilWorker worker, TypeReference resolved)
		{
			if (resolved.FullName == "System.String")
				return;

			if (resolved.FullName == "System.Void") {
				worker.Emit (OpCodes.Ldsfld, application.CommonMethods.StringEmptyField);
				return;
			}

			if (resolved.IsValueType) {
				worker.Emit (OpCodes.Box, app.Assembly.MainModule.Import (resolved));
				worker.Emit (OpCodes.Call, app.Assembly.MainModule.Import (page.ValueToStringMethod));
				return;
			}

			TypeDefinition rtype = resolved.Resolve ();
			MethodReference method = rtype.Methods.GetMethod ("ToString", new TypeReference [0]);

			// Import it so we get a method reference
			method = application.Assembly.MainModule.Import (method);
			Instruction inst = worker.Emit (OpCodes.Callvirt, (MethodReference) method);
		}

		public void EmitSinglePrint (Expression expression)
		{
			if (IsChildTemplate)
				return;

			CilWorker worker = CurrentMethod.Body.CilWorker;

			worker.Emit (OpCodes.Ldarg_1);
			expression.Emit (application, this, worker);
			EmitToString (application, this, worker, expression.ResolvedType);
			worker.Emit (OpCodes.Callvirt, application.CommonMethods.WriteStringMethod);
		}

		public void EmitSet (NamedTarget target, Expression expression)
		{
			CilWorker worker = CurrentMethod.Body.CilWorker;

			//
			// For now lets make them all fields
			//

			FieldDefinition field = FindField (target.Name);
			if (field == null)
				field = AddField (target.Name);

			worker.Emit (OpCodes.Ldarg_0);
			expression.Emit (application, this, worker);
			worker.Emit (OpCodes.Stfld, field);
		}

		public void BeginMacro (string name, List<ArgumentDefinition> args)
		{
			MethodDefinition meth = AddRenderMethod (name, args);

			method_stack.Push (meth);
		}

		public void EndMacro (string name)
		{
			if (name != null && CurrentMethod.Name != name)
				throw new Exception (String.Format ("Unmatched macro names, expected {0} but got {1}", CurrentMethod.Name, name));

			CilWorker worker = CurrentMethod.Body.CilWorker;
			worker.Emit (OpCodes.Ret);

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

			CilWorker worker = CurrentMethod.Body.CilWorker;
			expression.Emit (application, this, worker);
			Instruction null_branch = worker.Emit (OpCodes.Brfalse, worker.Create (OpCodes.Nop));

			expression.Emit (application, this, worker);
			worker.Emit (OpCodes.Isinst, string_type);
			Instruction isstr_branch = worker.Emit (OpCodes.Brfalse, worker.Create (OpCodes.Nop));

			expression.Emit (application, this, worker);
			worker.Emit (OpCodes.Castclass, string_type);
			worker.Emit (OpCodes.Call, application.CommonMethods.IsNullOrEmptyMethod);
			Instruction empty_branch = worker.Emit (OpCodes.Brtrue, worker.Create (OpCodes.Nop));

			isstr_branch.Operand = worker.Emit (OpCodes.Nop);

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

			CilWorker worker = CurrentMethod.Body.CilWorker;

			Instruction branch_to_end = worker.Emit (OpCodes.Br, worker.Create (OpCodes.Nop));
			Instruction begin_else = worker.Emit (OpCodes.Nop);

			ifcontext.UpdateNextConditionalBranches (begin_else);
			ifcontext.EndIfBranches.Add (branch_to_end);
		}

		public void EmitEndIf ()
		{
			if (IsChildTemplate)
				return;

			IfContext ifcontext = ifstack.Pop ();

			CilWorker worker = CurrentMethod.Body.CilWorker;

			Instruction end_branch = worker.Emit (OpCodes.Nop);
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
				VariableDefinitionCollection vars = CurrentMethod.Body.Variables;
				enumeratorvar = new VariableDefinition (local_enum_name, vars.Count, CurrentMethod, enum_type);
				vars.Add (enumeratorvar);
			}

			CilWorker worker = CurrentMethod.Body.CilWorker;
			expression.Emit (application, this, worker);
			worker.Emit (OpCodes.Castclass, assembly.MainModule.Import (typeof (System.Collections.IEnumerable)));
			worker.Emit (OpCodes.Callvirt, application.CommonMethods.GetEnumeratorMethod);
			
			worker.Emit (OpCodes.Stloc, enumeratorvar);

			Instruction begin_loop = worker.Emit (OpCodes.Br, worker.Create (OpCodes.Nop));
			forloop_stack.Push (new ForLoopContext (name, begin_loop, enumeratorvar));

			worker.Emit (OpCodes.Nop);
		}

		public void EndForLoop ()
		{
			if (IsChildTemplate)
				return;

			ForLoopContext forloop = forloop_stack.Pop ();

			CilWorker worker = CurrentMethod.Body.CilWorker;

			Instruction enter_loop = worker.Emit (OpCodes.Ldloc, forloop.EnumeratorVariable);
			worker.Emit (OpCodes.Callvirt, application.CommonMethods.EnumeratorMoveNextMethod);
			worker.Emit (OpCodes.Brtrue, forloop.BeginLoopInstruction.Next);

			forloop.BeginLoopInstruction.Operand = enter_loop;
		}

		public void Save ()
		{
		       CilWorker worker = CurrentMethod.Body.CilWorker;

		       worker.Emit (OpCodes.Ret);
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
			CilWorker worker = CurrentMethod.Body.CilWorker;

			ForLoopContext forloop = forloop_stack.Peek ();
			worker.Emit (OpCodes.Ldloc, forloop.EnumeratorVariable);
			worker.Emit (OpCodes.Callvirt, application.CommonMethods.EnumeratorGetCurrentMethod);
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

		private VariableDefinition FindVariable (string name, VariableDefinitionCollection variables)
		{
			foreach (VariableDefinition variable in variables) {
				if (variable.Name == name)
					return variable;
			}

			return null;
		}

		public FieldDefinition FindField (string name)
		{
			if (!Definition.HasFields)
				return null;
			return Definition.Fields.GetField (name);
		}

		public FieldDefinition AddField (string name)
		{
			FieldDefinition field = new FieldDefinition (name, assembly.MainModule.Import (typeof (object)), FieldAttributes.Public);
			Definition.Fields.Add (field);

			return field;
		}

		public MethodDefinition GetMethod (string name)
		{
			MethodDefinition [] methods = Definition.Methods.GetMethod (name);

			if (methods.Length < 1)
				return null;

			return methods [0];
		}

		public MethodDefinition AddRenderMethod (string name, List<ArgumentDefinition> extra_args=null)
		{
			MethodAttributes atts = MethodAttributes.Public | MethodAttributes.Virtual;

			MethodDefinition render = new MethodDefinition (name, atts, assembly.MainModule.Import (typeof (void)));

			render.Parameters.Add (new ParameterDefinition ("stream", -1, (ParameterAttributes) 0, assembly.MainModule.Import (typeof (TextWriter))));
			render.Parameters.Add (new ParameterDefinition ("args", -1, (ParameterAttributes) 0, assembly.MainModule.Import (typeof (Dictionary<string,object>))));

			if (extra_args != null) {
				TypeReference object_type = assembly.MainModule.Import (typeof (object));
				foreach (ArgumentDefinition arg in extra_args) {
					ParameterDefinition pdef = new ParameterDefinition (arg.Name, -1, (ParameterAttributes) 0, object_type);
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
			to_string.Parameters.Add (new ParameterDefinition ("the_value", -1, (ParameterAttributes) 0, assembly.MainModule.Import (typeof (System.ValueType))));

			Definition.Methods.Add (to_string);

			CilWorker worker = to_string.Body.CilWorker;
			worker.Emit (OpCodes.Ldarg_0);
			worker.Emit (OpCodes.Callvirt, assembly.MainModule.Import (typeof (System.ValueType).GetMethod ("ToString")));
			worker.Emit (OpCodes.Ret);
					
			return to_string;
		}

		public static string NameForPath (string path, out string name_space)
		{
			string [] pieces = path.Split (System.IO.Path.DirectorySeparatorChar);
			StringBuilder ns = new StringBuilder ("templates");

			string name = String.Concat ("page_", System.IO.Path.GetFileNameWithoutExtension (pieces [pieces.Length - 1]));
			for (int i = 0; i < pieces.Length - 1; i++) {
				ns.Append (".");
				ns.Append (pieces [0]);
			}

			name_space = ns.ToString ();
			return name;
		}

		public static string FullNameForPath (string path)
		{
			string ns;
			string name = NameForPath (path, out ns);

			return String.Concat (ns, ".", name);
		}
	}
}


		
