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
			
			int param_start = 1;
			data [0] = ctx;
			
			if (typeof (ManosApp).IsAssignableFrom (parameters [1].ParameterType)) {
				data [1] = app;
				++param_start;
			}
			
			for (int i = param_start; i < data.Length; i++) {
				string name = parameters [i].Name;
				UnsafeString strd = ctx.Request.Data.Get (name);
			
				if (!TryConvertType (ctx, parameters [i].ParameterType, parameters [i], strd, out data [i]))
				    return false;
			}
			
			return true;
		}
		
		public static bool TryConvertType (IManosContext ctx, Type type, ParameterInfo param, UnsafeString unsafe_str_value, out object data)
		{
			if (type == typeof (UnsafeString)) {
				data = unsafe_str_value;
				return true;
			}

			string str_value = unsafe_str_value == null ? null : unsafe_str_value.SafeValue;
			
			if (TryConvertFormData (type, str_value, out data))
				return true;

			if (str_value == null && param.DefaultValue != null) {
				data = param.DefaultValue;
				return true;
			}
			
			try {
				data = Convert.ChangeType (str_value, type);
			} catch {
				Console.Error.WriteLine ("Error while converting '{0}' to '{1}'.", str_value, type);
				
				data = null;
				return false;
			}
			
			return true;
		}
		
		public static bool TryConvertFormData (Type type, string str_value, out object data)
		{
			var converter = new HtmlFormDataTypeConverter (type);
			
			data = converter.ConvertFrom (str_value);
			return data != null;
		}
	}
}
