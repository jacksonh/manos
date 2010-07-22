

using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;

using Mango.Server;
using Mango.Routing;
using Mango.Templates.Minge;

namespace Mango {

	// GET, HEAD, POST, PUT, DELETE, TRACE
	public class MangoModule {

		private RouteHandler routes = new RouteHandler ();

		public MangoModule ()
		{
			StartInternal ();
		}

		public RouteHandler Routes {
			get { return routes; }
		}

		private RouteHandler AddRouteHandler (MangoModule module, string [] patterns, string [] methods)
		{
			module.Routes.Patterns = patterns;
			module.Routes.Methods = methods;
			Routes.Children.Add (module.Routes);
			return module.Routes;
		}

		private RouteHandler AddRouteHandler (MangoAction action, string [] patterns, string [] methods)
		{
			// TODO: Need to decide if this is a good or bad idea
			// RemoveImplicitHandlers (action);

			RouteHandler res = new RouteHandler (patterns, methods, new MangoTarget (action));
			Routes.Children.Add (res);
			return res;
		}

		private RouteHandler AddImplicitRouteHandler (MangoModule module, string [] patterns, string [] methods)
		{
			module.Routes.IsImplicit = true;
			module.Routes.Patterns = patterns;
			module.Routes.Methods = methods;
			Routes.Children.Add (module.Routes);
			return module.Routes;
		}

		private RouteHandler AddImplicitRouteHandler (MangoAction action, string [] patterns, string [] methods)
		{
			Console.WriteLine ("adding implicit route handler for: {0} {1}", patterns [0], action);
			RouteHandler res = new RouteHandler (patterns, methods, new MangoTarget (action)) {
				IsImplicit = true,
			};

			Routes.Children.Add (res);
			return res;
		}

		public RouteHandler Route (string pattern, MangoModule module)
		{
			return AddRouteHandler (module, new string [] { pattern }, HttpMethods.RouteMethods);
		}

		public RouteHandler Route (string pattern, MangoAction action)
		{
			return AddRouteHandler (action, new string [] { pattern }, HttpMethods.RouteMethods);
		}

		public RouteHandler Route (MangoAction action, params string [] patterns)
		{
			return AddRouteHandler (action, patterns, HttpMethods.RouteMethods);
		}

		public RouteHandler Route (MangoModule module, params string [] patterns)
		{
			return AddRouteHandler (module, patterns, HttpMethods.RouteMethods);
		}

		/*
		public RouteHandler Get (MangoAction action, params string [] patterns)
		{
			return AddRouteHandler (action, null, patterns, HttpMethods.GetMethods);
		}

		public RouteHandler Get (string name, MangoAction action, params string [] patterns)
		{
			return AddRouteHandler (action, name, patterns, HttpMethods.GetMethods);
		}

		public RouteHandler Get (MangoModule module, params string [] patterns)
		{
			return AddRouteHandler (module, null, patterns, HttpMethods.GetMethods);
		}

		public RouteHandler Get (string name, MangoModule module, params string [] patterns)
		{
			return AddRouteHandler (module, name, patterns, HttpMethods.GetMethods);
		}

		public RouteHandler Head (MangoAction action, params string [] patterns)
		{
			return AddRouteHandler (action, null, patterns, HttpMethods.HeadMethods);
		}

		public RouteHandler Head (string name, MangoAction action, params string [] patterns)
		{
			return AddRouteHandler (action, name, patterns, HttpMethods.HeadMethods);
		}

		public RouteHandler Head (MangoModule module, params string [] patterns)
		{
			return AddRouteHandler (module, null, patterns, HttpMethods.HeadMethods);
		}

		public RouteHandler Head (string name, MangoModule module, params string [] patterns)
		{
			return AddRouteHandler (module, name, patterns, HttpMethods.HeadMethods);
		}

		public RouteHandler Post (MangoAction action, params string [] patterns)
		{
			return AddRouteHandler (action, null, patterns, HttpMethods.PostMethods);
		}

		public RouteHandler Post (string name, MangoAction action, params string [] patterns)
		{
			return AddRouteHandler (action, name, patterns, HttpMethods.PostMethods);
		}

		public RouteHandler Post (MangoModule module, params string [] patterns)
		{
			return AddRouteHandler (module, null, patterns, HttpMethods.PostMethods);
		}

		public RouteHandler Post (string name, MangoModule module, params string [] patterns)
		{
			return AddRouteHandler (module, name, patterns, HttpMethods.PostMethods);
		}

		public RouteHandler Put (MangoAction action, params string [] patterns)
		{
			return AddRouteHandler (action, null, patterns, HttpMethods.PutMethods);
		}

		public RouteHandler Put (string name, MangoAction action, params string [] patterns)
		{
			return AddRouteHandler (action, name, patterns, HttpMethods.PutMethods);
		}

		public RouteHandler Put (MangoModule module, params string [] patterns)
		{
			return AddRouteHandler (module, null, patterns, HttpMethods.PutMethods);
		}

		public RouteHandler Put (string name, MangoModule module, params string [] patterns)
		{
			return AddRouteHandler (module, name, patterns, HttpMethods.PutMethods);
		}

		public RouteHandler Delete (MangoAction action, params string [] patterns)
		{
			return AddRouteHandler (action, null, patterns, HttpMethods.DeleteMethods);
		}

		public RouteHandler Delete (string name, MangoAction action, params string [] patterns)
		{
			return AddRouteHandler (action, name, patterns, HttpMethods.DeleteMethods);
		}

		public RouteHandler Delete (MangoModule module, params string [] patterns)
		{
			return AddRouteHandler (module, null, patterns, HttpMethods.DeleteMethods);
		}

		public RouteHandler Delete (string name, MangoModule module, params string [] patterns)
		{
			return AddRouteHandler (module, name, patterns, HttpMethods.DeleteMethods);
		}

		public RouteHandler Trace (MangoAction action, params string [] patterns)
		{
			return AddRouteHandler (action, null, patterns, HttpMethods.TraceMethods);
		}

		public RouteHandler Trace (string name, MangoAction action, params string [] patterns)
		{
			return AddRouteHandler (action, name, patterns, HttpMethods.TraceMethods);
		}

		public RouteHandler Trace (MangoModule module, params string [] patterns)
		{
			return AddRouteHandler (module, null, patterns, HttpMethods.TraceMethods);
		}

		public RouteHandler Trace (string name, MangoModule module, params string [] patterns)
		{
			return AddRouteHandler (module, name, patterns, HttpMethods.TraceMethods);
		}

		public RouteHandler Options (MangoAction action, params string [] patterns)
		{
			return AddRouteHandler (action, null, patterns, HttpMethods.OptionsMethods);
		}

		public RouteHandler Options (string name, MangoAction action, params string [] patterns)
		{
			return AddRouteHandler (action, name, patterns, HttpMethods.OptionsMethods);
		}

		public RouteHandler Options (MangoModule module, params string [] patterns)
		{
			return AddRouteHandler (module, null, patterns, HttpMethods.OptionsMethods);
		}

		public RouteHandler Options (string name, MangoModule module, params string [] patterns)
		{
			return AddRouteHandler (module, name, patterns, HttpMethods.OptionsMethods);
		}

		*/
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

			handler.Invoke (new MangoContext (con));

			con.Response.Finish ();
		}

		public static void RenderTemplate (MangoContext context, string template, object data)
		{
			MemoryStream stream = new MemoryStream ();

			using (StreamWriter writer = new StreamWriter (stream)) {
				Mango.Templates.Minge.Templates.RenderToStream (template, writer, data);
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

			Console.WriteLine ("adding implicit routes");
			foreach (MethodInfo meth in methods) {
				Console.WriteLine (" -- {0} is action: {1}  is ignored:  {2}", meth.Name, IsActionSignature (meth), IsIgnoredAction (meth));
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

			if (p [0].ParameterType != typeof (IMangoContext))
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
			Console.WriteLine ("adding handlers for action:  {0}", info.Name);
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
			Console.WriteLine ("adding default handler for:  {0}", info.Name);
			MangoAction action = (MangoAction) Delegate.CreateDelegate (typeof (MangoAction), info);
			AddImplicitRouteHandler (action, new string [] { info.Name }, HttpMethods.RouteMethods);
		}

		private void AddHandlerForAction (RouteHandler routes, HttpMethodAttribute att, MethodInfo info)
		{
			MangoAction action = (MangoAction) Delegate.CreateDelegate (typeof (MangoAction), info);
			AddImplicitRouteHandler (action, att.Patterns, att.Methods);
		}
		
	}
}

