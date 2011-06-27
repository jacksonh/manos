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
using System.Collections;
using System.Collections.Generic;


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
			
				if (!TryConvertType (ctx, name, parameters [i], out data [i]))
				    return false;
			}
			
			return true;
		}

		public static bool TryConvertType (IManosContext ctx, string name, ParameterInfo param, out object data)
		{
			Type dest = param.ParameterType;
			
			if (dest.IsArray) {
				var list = ctx.Request.Data.GetList (name);
				if (list != null) {
					Type element = dest.GetElementType ();
					IList arr = Array.CreateInstance (element, list.Count);
					for (int i = 0; i < list.Count; i++) {
						object elem_data;
						if (!TryConvertUnsafeString (ctx, element, param, list [i], out elem_data)) {
							data = null;
							return false;
						}
						arr [i] = elem_data;
					}
					data = arr;
					return true;
				}
			}

			if (dest.GetInterface ("IDictionary") != null) {
				var dict = ctx.Request.Data.GetDict (name);
				if (dict != null) {
					Type eltype = typeof (UnsafeString);
					IDictionary dd = (IDictionary) Activator.CreateInstance (dest);
					if (dest.IsGenericType) {
						Type [] args = dest.GetGenericArguments ();
						if (args.Length != 2)
							throw new Exception ("Generic Dictionaries must contain two generic type arguments.");
						if (args [0] != typeof (string))
							throw new Exception ("Generic Dictionaries must use strings for their keys.");
						eltype = args [1]; // ie the TValue in Dictionary<TKey,TValue>
					}
					foreach (string key in dict.Keys) {
						object elem_data;
						if (!TryConvertUnsafeString (ctx, eltype, param, dict [key], out elem_data)) {
							data = null;
							return false;
						}
						dd.Add (key, elem_data);
					}
					data = dd;
					return true;
				}
			}

			UnsafeString strd = ctx.Request.Data.Get (name);
			return TryConvertUnsafeString (ctx, dest, param, strd, out data);
		}

		public static bool TryConvertUnsafeString (IManosContext ctx, Type type, ParameterInfo param, UnsafeString unsafe_str_value, out object data)
		{
			if (type == typeof (UnsafeString)) {
				data = unsafe_str_value;
				return true;
			}

			string str_value = unsafe_str_value == null ? null : unsafe_str_value.SafeValue;
			
			if (TryConvertFormData (type, str_value, out data))
				return true;

			if (str_value == null && param.DefaultValue != DBNull.Value) {
				data = param.DefaultValue;
				return true;
			}
			
			try {
				var underlying_type = Nullable.GetUnderlyingType(type);
				if (underlying_type != null)
					data = Convert.ChangeType (str_value, underlying_type);
				else
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
