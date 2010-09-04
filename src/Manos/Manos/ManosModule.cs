

using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;

using Manos.Server;
using Manos.Routing;
using Manos.Templates;

namespace Manos {

	public class ManosModule {

		private RouteHandler routes = new RouteHandler ();

		public ManosModule ()
		{
			StartInternal ();
		}

		public RouteHandler Routes {
			get { return routes; }
		}

		private RouteHandler AddRouteHandler (ManosModule module, string [] patterns, string [] methods)
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

		private RouteHandler AddRouteHandler (ManosAction action, string [] patterns, string [] methods)
		{
			// TODO: Need to decide if this is a good or bad idea
			// RemoveImplicitHandlers (action);

			if (action == null)
				throw new ArgumentNullException ("action");
			if (patterns == null)
				throw new ArgumentNullException ("patterns");
			if (methods == null)
				throw new ArgumentNullException ("methods");
			
			RouteHandler res = new RouteHandler (patterns, methods, new ManosTarget (action));
			Routes.Children.Add (res);
			return res;
		}

		private RouteHandler AddImplicitRouteHandler (ManosModule module, string [] patterns, string [] methods)
		{
			module.Routes.IsImplicit = true;
			module.Routes.Patterns = patterns;
			module.Routes.Methods = methods;
			Routes.Children.Add (module.Routes);
			return module.Routes;
		}

		private RouteHandler AddImplicitRouteHandler (ManosAction action, string [] patterns, string [] methods)
		{
			RouteHandler res = new RouteHandler (patterns, methods, new ManosTarget (action)) {
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
		
		public void HandleTransaction (IHttpTransaction con)
		{
			if (con == null)
				throw new ArgumentNullException ("con");

			var handler = Routes.Find (con.Request);
			if (handler == null) {
				con.Response.StatusCode = 404;
				con.Response.Finish ();
				return;
			}

			try {
				handler.Invoke (new ManosContext (con));
			} catch (Exception e) {
				con.Response.StatusCode = 500;
				//
				// TODO: Maybe the cleanest thing to do is
				// have a HandleError, HandleException thing
				// on HttpTransaction, along with an UnhandledException
				// method/event on ManosModule.
				//
			}

			con.Response.Finish ();
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
		
		public static void RenderTemplate (ManosContext context, string template, object data)
		{
			MemoryStream stream = new MemoryStream ();

			using (StreamWriter writer = new StreamWriter (stream)) {
				Manos.Templates.Engine.RenderToStream (template, writer, data);
				writer.Flush ();
			}

			context.Response.Write (stream.GetBuffer ());
		}

		protected void StartInternal ()
		{
			AddImplicitRoutes ();
		}

		private void AddImplicitRoutes ()
		{
			MethodInfo [] methods = this.GetType ().GetMethods ();

			foreach (MethodInfo meth in methods) {
				if (!IsActionSignature (meth))
					continue;
				if (IsIgnoredAction (meth))
					continue;
				AddHandlersForAction (Routes, meth);
			}
		}

		private bool IsActionSignature (MethodInfo meth)
		{
			ParameterInfo [] p = meth.GetParameters ();

			if (p.Length != 1)
				return false;

			if (p [0].ParameterType != typeof (IManosContext))
				return false;

			return true;
		}

		private bool IsIgnoredAction (MethodInfo info)
		{
			var atts = info.GetCustomAttributes (typeof (IgnoreAttribute), false);

			return atts.Length > 0;
		}

		private void AddHandlersForAction (RouteHandler routes, MethodInfo info)
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
			ManosAction action = (ManosAction) Delegate.CreateDelegate (typeof (ManosAction), info);
			AddImplicitRouteHandler (action, new string [] { info.Name }, HttpMethods.RouteMethods);
		}

		private void AddHandlerForAction (RouteHandler routes, HttpMethodAttribute att, MethodInfo info)
		{
			ManosAction action = (ManosAction) Delegate.CreateDelegate (typeof (ManosAction), info);
			AddImplicitRouteHandler (action, att.Patterns, att.Methods);
		}
		
	}
}

