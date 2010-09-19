
using System;
using System.Reflection;


namespace Manos.Routing
{
	public class ParameterizedActionTarget : IManosTarget
	{
		private object target;
		private ParameterizedAction action;
		private ParameterInfo [] parameters;
		
		public ParameterizedActionTarget (object target, MethodInfo method, ParameterizedAction action)
		{
			if (method == null)
				throw new ArgumentNullException ("method");
			if (action == null)
				throw new ArgumentNullException ("action");
			
			this.target = target;
			Action = action;
			
			parameters = method.GetParameters ();
		}
		
		public Object Target {
			get { return target; }
		}
		
		public Delegate Action {
			get { return action; }
			set {
				if (value == null)
					throw new ArgumentNullException ("action");
				ParameterizedAction pa = value as ParameterizedAction;
				if (pa == null)
					throw new InvalidOperationException ("ParameterizedActionTarget Action property can only be set to a ParameterizedAction.");
					                                     
				action = pa;
			}
		}
		
		public void Invoke (ManosApp app, IManosContext ctx)
		{
			object [] data;
			
			if (!TryGetDataForParamList (parameters, app, ctx, out data)) {
				// TODO: More graceful way of handling this?
				ctx.Transaction.Abort (400, "Can not convert parameters to match Action argument list.");
				return;
			}
			
			action (target, data);
		}
		
		private ParameterInfo [] GetParamList ()
		{
			MethodInfo method = action.Method;
			
			return method.GetParameters ();
		}
		
		public static bool TryGetDataForParamList (ParameterInfo [] parameters, ManosApp app, IManosContext ctx, out object [] data)
		{
			data = new object [parameters.Length];
			
			data [0] = app;
			data [1] = ctx;
			
			for (int i = 2; i < data.Length; i++) {
				string name = parameters [i].Name;
				UnsafeString strd = ctx.Request.Data.Get (name);
			
				if (!TryConvertType (ctx, parameters [i].ParameterType, strd, out data [i]))
				    return false;
			}
			
			return true;
		}
		
		public static bool TryConvertType (IManosContext ctx, Type type, UnsafeString unsafe_str_value, out object data)
		{
			if (type == typeof (UnsafeString)) {
				data = unsafe_str_value;
				return true;
			}
			
			string str_value = unsafe_str_value.SafeValue;
			
			try {
				data = Convert.ChangeType (str_value, type);
			} catch {
				Console.Error.WriteLine ("Error while converting '{0}' to '{1}'.", str_value, type);
				
				data = null;
				return false;
			}
			
			return true;
		}
	}
}
