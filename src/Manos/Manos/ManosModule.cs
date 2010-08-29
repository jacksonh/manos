

using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;

using Mango.Server;
using Mango.Routing;
using Mango.Templates;

namespace Mango {

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

		private RouteHandler AddRouteHandler (MangoAction action, string [] patterns, string [] methods)
		{
			// TODO: Need to decide if this is a good or bad idea
			// RemoveImplicitHandlers (action);

			if (action == null)
				throw new ArgumentNullException ("action");
			if (patterns == null)
				throw new ArgumentNullException ("patterns");
			if (methods == null)
				throw new ArgumentNullException ("methods");
			
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

		public RouteHandler Get (string pattern, MangoModule module)
		{
			return AddRouteHandler (module, new string [] { pattern }, HttpMethods.GetMethods);
		}

		public RouteHandler Get (string pattern, MangoAction action)
		{
			return AddRouteHandler (action, new string [] { pattern }, HttpMethods.GetMethods);
		}

		public RouteHandler Get (MangoAction action, params string [] patterns)
		{
			return AddRouteHandler (action, patterns, HttpMethods.GetMethods);
		}

		public RouteHandler Get (MangoModule module, params string [] patterns)
		{
			return AddRouteHandler (module, patterns, HttpMethods.GetMethods);
		}
		
		//
		
		public RouteHandler Put (string pattern, MangoModule module)
		{
			return AddRouteHandler (module, new string [] { pattern }, HttpMethods.PutMethods);
		}

		public RouteHandler Put (string pattern, MangoAction action)
		{
			return AddRouteHandler (action, new string [] { pattern }, HttpMethods.PutMethods);
		}

		public RouteHandler Put (MangoAction action, params string [] patterns)
		{
			return AddRouteHandler (action, patterns, HttpMethods.PutMethods);
		}

		public RouteHandler Put (MangoModule module, params string [] patterns)
		{
			return AddRouteHandler (module, patterns, HttpMethods.PutMethods);
		}
		
		public RouteHandler Post (string pattern, MangoModule module)
		{
			return AddRouteHandler (module, new string [] { pattern }, HttpMethods.PostMethods);
		}

		public RouteHandler Post (string pattern, MangoAction action)
		{
			return AddRouteHandler (action, new string [] { pattern }, HttpMethods.PostMethods);
		}

		public RouteHandler Post (MangoAction action, params string [] patterns)
		{
			return AddRouteHandler (action, patterns, HttpMethods.PostMethods);
		}

		public RouteHandler Post (MangoModule module, params string [] patterns)
		{
			return AddRouteHandler (module, patterns, HttpMethods.PostMethods);
		}
		
		//
		
		public RouteHandler Delete (string pattern, MangoModule module)
		{
			return AddRouteHandler (module, new string [] { pattern }, HttpMethods.DeleteMethods);
		}

		public RouteHandler Delete (string pattern, MangoAction action)
		{
			return AddRouteHandler (action, new string [] { pattern }, HttpMethods.DeleteMethods);
		}

		public RouteHandler Delete (MangoAction action, params string [] patterns)
		{
			return AddRouteHandler (action, patterns, HttpMethods.DeleteMethods);
		}

		public RouteHandler Delete (MangoModule module, params string [] patterns)
		{
			return AddRouteHandler (module, patterns, HttpMethods.DeleteMethods);
		}
		
		//
		
		public RouteHandler Head (string pattern, MangoModule module)
		{
			return AddRouteHandler (module, new string [] { pattern }, HttpMethods.HeadMethods);
		}

		public RouteHandler Head (string pattern, MangoAction action)
		{
			return AddRouteHandler (action, new string [] { pattern }, HttpMethods.HeadMethods);
		}

		public RouteHandler Head (MangoAction action, params string [] patterns)
		{
			return AddRouteHandler (action, patterns, HttpMethods.HeadMethods);
		}

		public RouteHandler Head (MangoModule module, params string [] patterns)
		{
			return AddRouteHandler (module, patterns, HttpMethods.HeadMethods);
		}
		
		//
		
		public RouteHandler Options (string pattern, MangoModule module)
		{
			return AddRouteHandler (module, new string [] { pattern }, HttpMethods.OptionsMethods);
		}

		public RouteHandler Options (string pattern, MangoAction action)
		{
			return AddRouteHandler (action, new string [] { pattern }, HttpMethods.OptionsMethods);
		}

		public RouteHandler Options (MangoAction action, params string [] patterns)
		{
			return AddRouteHandler (action, patterns, HttpMethods.OptionsMethods);
		}

		public RouteHandler Options (MangoModule module, params string [] patterns)
		{
			return AddRouteHandler (module, patterns, HttpMethods.OptionsMethods);
		}
		
		//
		
		public RouteHandler Trace (string pattern, MangoModule module)
		{
			return AddRouteHandler (module, new string [] { pattern }, HttpMethods.TraceMethods);
		}

		public RouteHandler Trace (string pattern, MangoAction action)
		{
			return AddRouteHandler (action, new string [] { pattern }, HttpMethods.TraceMethods);
		}

		public RouteHandler Trace (MangoAction action, params string [] patterns)
		{
			return AddRouteHandler (action, patterns, HttpMethods.TraceMethods);
		}

		public RouteHandler Trace (MangoModule module, params string [] patterns)
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
				handler.Invoke (new MangoContext (con));
			} catch (Exception e) {
				con.Response.StatusCode = 500;
				//
				// TODO: Maybe the cleanest thing to do is
				// have a HandleError, HandleException thing
				// on HttpTransaction, along with an UnhandledException
				// method/event on MangoModule.
				//
			}

			con.Response.Finish ();
		}

		public static void RenderTemplate (MangoContext context, string template, object data)
		{
			MemoryStream stream = new MemoryStream ();

			using (StreamWriter writer = new StreamWriter (stream)) {
				Mango.Templates.Engine.RenderToStream (template, writer, data);
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

