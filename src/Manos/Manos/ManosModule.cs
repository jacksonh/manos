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
using System.IO;
using System.Linq;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;

using Manos.Http;
using Manos.Server;
using Manos.Routing;
using Manos.Caching;
using Manos.Templates;


namespace Manos {
	
	/// <summary>
	/// A pre-packaged set of routes/actions that can be registered in the constructor of a ManoApp-derived class.
	/// </summary>
	public class ManosModule {

		private RouteHandler routes = new RouteHandler ();

		public ManosModule ()
		{
			StartInternal ();
		}

		public RouteHandler Routes {
			get { return routes; }
		}

		public IManosCache Cache {
			get {
				return AppHost.Cache;	
			}
		}
		
		private RouteHandler AddRouteHandler (ManosModule module, string [] patterns, HttpMethod [] methods)
		{
			if (module == null)
				throw new ArgumentNullException ("module");
			if (patterns == null)
				throw new ArgumentNullException ("patterns");
			if (methods == null)
				throw new ArgumentNullException ("methods");
			
			module.Routes.Patterns = patterns;
			module.Routes.Methods = methods;
			Routes.Children.Add (module.Routes);
			return module.Routes;
		}

		private RouteHandler AddRouteHandler (ManosAction action, string [] patterns, HttpMethod [] methods)
		{
			return AddRouteHandler (new ActionTarget (action), patterns, methods);
		}
		
		private RouteHandler AddRouteHandler (IManosTarget target, string [] patterns, HttpMethod [] methods)
		{
			// TODO: Need to decide if this is a good or bad idea
			// RemoveImplicitHandlers (action);

			if (target == null)
				throw new ArgumentNullException ("action");
			if (patterns == null)
				throw new ArgumentNullException ("patterns");
			if (methods == null)
				throw new ArgumentNullException ("methods");
			
			RouteHandler res = new RouteHandler (patterns, methods, target);
			Routes.Children.Add (res);
			return res;
		}

		private RouteHandler AddImplicitRouteHandler (ManosModule module, string [] patterns, HttpMethod [] methods)
		{
			module.Routes.IsImplicit = true;
			module.Routes.Patterns = patterns;
			module.Routes.Methods = methods;
			Routes.Children.Add (module.Routes);
			return module.Routes;
		}

		private RouteHandler AddImplicitRouteHandler (IManosTarget target, string [] patterns, HttpMethod [] methods)
		{
			RouteHandler res = new RouteHandler (patterns, methods, target) {
				IsImplicit = true,
			};

			Routes.Children.Add (res);
			return res;
		}
		
		public RouteHandler Route (string pattern, ManosModule module)
		{
			return AddRouteHandler (module, new string [] { pattern }, HttpMethods.RouteMethods);
		}

		public RouteHandler Route (string pattern, ManosAction action)
		{
			return AddRouteHandler (action, new string [] { pattern }, HttpMethods.RouteMethods);
		}

		public RouteHandler Route (ManosAction action, params string [] patterns)
		{
			return AddRouteHandler (action, patterns, HttpMethods.RouteMethods);
		}

		public RouteHandler Route (ManosModule module, params string [] patterns)
		{
			return AddRouteHandler (module, patterns, HttpMethods.RouteMethods);
		}

		public RouteHandler Get (string pattern, ManosModule module)
		{
			return AddRouteHandler (module, new string [] { pattern }, HttpMethods.GetMethods);
		}

		public RouteHandler Get (string pattern, ManosAction action)
		{
			return AddRouteHandler (action, new string [] { pattern }, HttpMethods.GetMethods);
		}

		public RouteHandler Get (ManosAction action, params string [] patterns)
		{
			return AddRouteHandler (action, patterns, HttpMethods.GetMethods);
		}

		public RouteHandler Get (ManosModule module, params string [] patterns)
		{
			return AddRouteHandler (module, patterns, HttpMethods.GetMethods);
		}
		
		//
		
		public RouteHandler Put (string pattern, ManosModule module)
		{
			return AddRouteHandler (module, new string [] { pattern }, HttpMethods.PutMethods);
		}

		public RouteHandler Put (string pattern, ManosAction action)
		{
			return AddRouteHandler (action, new string [] { pattern }, HttpMethods.PutMethods);
		}

		public RouteHandler Put (ManosAction action, params string [] patterns)
		{
			return AddRouteHandler (action, patterns, HttpMethods.PutMethods);
		}

		public RouteHandler Put (ManosModule module, params string [] patterns)
		{
			return AddRouteHandler (module, patterns, HttpMethods.PutMethods);
		}
		
		public RouteHandler Post (string pattern, ManosModule module)
		{
			return AddRouteHandler (module, new string [] { pattern }, HttpMethods.PostMethods);
		}

		public RouteHandler Post (string pattern, ManosAction action)
		{
			return AddRouteHandler (action, new string [] { pattern }, HttpMethods.PostMethods);
		}

		public RouteHandler Post (ManosAction action, params string [] patterns)
		{
			return AddRouteHandler (action, patterns, HttpMethods.PostMethods);
		}

		public RouteHandler Post (ManosModule module, params string [] patterns)
		{
			return AddRouteHandler (module, patterns, HttpMethods.PostMethods);
		}
		
		//
		
		public RouteHandler Delete (string pattern, ManosModule module)
		{
			return AddRouteHandler (module, new string [] { pattern }, HttpMethods.DeleteMethods);
		}

		public RouteHandler Delete (string pattern, ManosAction action)
		{
			return AddRouteHandler (action, new string [] { pattern }, HttpMethods.DeleteMethods);
		}

		public RouteHandler Delete (ManosAction action, params string [] patterns)
		{
			return AddRouteHandler (action, patterns, HttpMethods.DeleteMethods);
		}

		public RouteHandler Delete (ManosModule module, params string [] patterns)
		{
			return AddRouteHandler (module, patterns, HttpMethods.DeleteMethods);
		}
		
		//
		
		public RouteHandler Head (string pattern, ManosModule module)
		{
			return AddRouteHandler (module, new string [] { pattern }, HttpMethods.HeadMethods);
		}

		public RouteHandler Head (string pattern, ManosAction action)
		{
			return AddRouteHandler (action, new string [] { pattern }, HttpMethods.HeadMethods);
		}

		public RouteHandler Head (ManosAction action, params string [] patterns)
		{
			return AddRouteHandler (action, patterns, HttpMethods.HeadMethods);
		}

		public RouteHandler Head (ManosModule module, params string [] patterns)
		{
			return AddRouteHandler (module, patterns, HttpMethods.HeadMethods);
		}
		
		//
		
		public RouteHandler Options (string pattern, ManosModule module)
		{
			return AddRouteHandler (module, new string [] { pattern }, HttpMethods.OptionsMethods);
		}

		public RouteHandler Options (string pattern, ManosAction action)
		{
			return AddRouteHandler (action, new string [] { pattern }, HttpMethods.OptionsMethods);
		}

		public RouteHandler Options (ManosAction action, params string [] patterns)
		{
			return AddRouteHandler (action, patterns, HttpMethods.OptionsMethods);
		}

		public RouteHandler Options (ManosModule module, params string [] patterns)
		{
			return AddRouteHandler (module, patterns, HttpMethods.OptionsMethods);
		}
		
		//
		
		public RouteHandler Trace (string pattern, ManosModule module)
		{
			return AddRouteHandler (module, new string [] { pattern }, HttpMethods.TraceMethods);
		}

		public RouteHandler Trace (string pattern, ManosAction action)
		{
			return AddRouteHandler (action, new string [] { pattern }, HttpMethods.TraceMethods);
		}

		public RouteHandler Trace (ManosAction action, params string [] patterns)
		{
			return AddRouteHandler (action, patterns, HttpMethods.TraceMethods);
		}

		public RouteHandler Trace (ManosModule module, params string [] patterns)
		{
			return AddRouteHandler (module, patterns, HttpMethods.TraceMethods);
		}
		
		public static void AddTimeout (TimeSpan timespan, TimeoutCallback callback)
		{
			AddTimeout (timespan, RepeatBehavior.Single, null, callback);
		}
		
		public static void AddTimeout (TimeSpan timespan, IRepeatBehavior repeat, TimeoutCallback callback)
		{
			AddTimeout (timespan, repeat, null, callback);
		}
		
		public static void AddTimeout (TimeSpan timespan, object data, TimeoutCallback callback)
		{
			AddTimeout (timespan, RepeatBehavior.Single, data, callback);
		}
		
		public static void AddTimeout (TimeSpan timespan, IRepeatBehavior repeat, object data, TimeoutCallback callback)
		{
			AppHost.AddTimeout (timespan, repeat, data, callback);
		}

		public static void AddPipe (ManosPipe pipe)
		{
			AppHost.AddPipe (pipe);
		}

		public static void RenderTemplate (ManosContext ctx, string template, object data)
		{
			TemplateEngine.RenderToStream (template, ctx.Response.Writer, data);
		}

		protected void StartInternal ()
		{
			AddImplicitRoutes ();
		}

		private void AddImplicitRoutes ()
		{
			MethodInfo [] methods = GetType ().GetMethods ();

			foreach (MethodInfo meth in methods) {
				if (meth.ReturnType != typeof (void))
					continue;
				
				if (IsIgnored (meth))
					continue;
				
				ParameterInfo [] parameters = meth.GetParameters ();
				if (IsActionSignature (meth, parameters)) {
					AddActionHandler (Routes, meth);
					continue;	
				}
				
				if (IsParameterizedActionSignature (meth, parameters)) {
					AddParameterizedActionHandler (Routes, meth);
					continue;
				}
			}
			
			PropertyInfo [] properties = GetType ().GetProperties ();
			
			foreach (PropertyInfo prop in properties) {
				if (!typeof (ManosModule).IsAssignableFrom (prop.PropertyType))
					continue;
				
				if (IsIgnored (prop))
					continue;
				
				AddImplicitModule (prop);
			}
		}

		private bool IsActionSignature (MethodInfo method, ParameterInfo [] parameters)
		{
			if (parameters.Length != 1)
				return false;

			if (method.DeclaringType.Assembly == typeof (ManosModule).Assembly)
				return false;

			if (parameters [0].ParameterType != typeof (IManosContext))
				return false;

			return true;
		}

		private bool IsParameterizedActionSignature (MethodInfo method, ParameterInfo [] parameters)
		{
			if (parameters.Length < 1) {
				return false;
			}

			if (method.DeclaringType.Assembly == typeof (ManosModule).Assembly)
				return false;

			if (parameters [0].ParameterType != typeof (IManosContext))
				return false;
			
			return true;
		}
		
		private bool IsIgnored (MemberInfo info)
		{
			var atts = info.GetCustomAttributes (typeof (IgnoreAttribute), false);

			return atts.Length > 0;
		}
		
		private void AddActionHandler (RouteHandler routes, MethodInfo info)
		{
			HttpMethodAttribute [] atts = (HttpMethodAttribute []) info.GetCustomAttributes (typeof (HttpMethodAttribute), false);

			if (atts.Length == 0) {
				AddDefaultHandlerForAction (routes, info);
				return;
			}

			foreach (HttpMethodAttribute att in atts) {
				AddHandlerForAction (routes, att, info);
			}

		}

		private void AddDefaultHandlerForAction (RouteHandler routes, MethodInfo info)
		{
			ManosAction action = ActionForMethod (info);

			ActionTarget target = new ActionTarget (action);
			AddImplicitRouteHandler (target, new string [] { "/" + info.Name }, HttpMethods.RouteMethods);
		}

		private void AddHandlerForAction (RouteHandler routes, HttpMethodAttribute att, MethodInfo info)
		{
			ManosAction action = ActionForMethod (info);
			
			ActionTarget target = new ActionTarget (action);
         	AddImplicitRouteHandler (target, att.Patterns, att.Methods);
		}
		
		private ManosAction ActionForMethod (MethodInfo info)
		{
			ManosAction action;
			
			if (info.IsStatic)
				action = (ManosAction) Delegate.CreateDelegate (typeof (ManosAction), info);
			else
				action = (ManosAction) Delegate.CreateDelegate (typeof (ManosAction), this, info);
			
			return action;
		}
		
		private void AddParameterizedActionHandler (RouteHandler routes, MethodInfo info)
		{
			HttpMethodAttribute [] atts = (HttpMethodAttribute []) info.GetCustomAttributes (typeof (HttpMethodAttribute), false);

			if (atts.Length == 0) {
				AddDefaultHandlerForParameterizedAction (routes, info);
				return;
			}

			foreach (HttpMethodAttribute att in atts) {
				AddHandlerForParameterizedAction (routes, att, info);
			}

		}
		
		private void AddDefaultHandlerForParameterizedAction (RouteHandler routes, MethodInfo info)
		{
			ParameterizedAction action = ParameterizedActionFactory.CreateAction (info);
			ParameterizedActionTarget target = new ParameterizedActionTarget (this, info, action);
			
			AddImplicitRouteHandler (target, new string [] { "/" + info.Name }, HttpMethods.RouteMethods);
		}
		
		private void AddHandlerForParameterizedAction (RouteHandler routes, HttpMethodAttribute att, MethodInfo info)
		{
			ParameterizedAction action = ParameterizedActionFactory.CreateAction (info);
			ParameterizedActionTarget target = new ParameterizedActionTarget (this, info, action);
			
			AddImplicitRouteHandler (target, att.Patterns, att.Methods);
		}
		
		private void AddImplicitModule (PropertyInfo prop)
		{
			var value = (ManosModule) prop.GetValue (this, null);
			
			if (value == null) {
				try {
					value = (ManosModule) Activator.CreateInstance (prop.PropertyType);
				} catch (Exception e) {
					throw new Exception (String.Format ("Unable to create default property value for '{0}'", prop), e);
				}
				
				prop.SetValue (this, value, null);
			}
			
			AddImplicitRouteHandler (value, new string [] { "/" + prop.Name }, HttpMethods.RouteMethods);
		}
		
	}
}

