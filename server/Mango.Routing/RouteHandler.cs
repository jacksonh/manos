

using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;


using Mango.Server;

namespace Mango.Routing {

	public class RouteHandler : IEnumerable<RouteHandler> {

		private List<string> patterns;
		private List<string> methods;

		internal RouteHandler ()
		{
			Children = new List<RouteHandler> ();
		}

		public RouteHandler (string [] patterns, string [] methods) : this (null, patterns, methods, null)
		{
		}

		public RouteHandler (string [] patterns, string [] methods, MangoAction action) : this (null, patterns, methods, action)
		{
		}

		public RouteHandler (string name, string [] patterns, string [] methods, MangoAction action)
		{
			Name = name;
			Action = action;

			this.patterns = new List<string> (patterns);
			this.methods = new List<string> (methods);

			Children = new List<RouteHandler> ();
		}

		public string Name {
			get;
			set;
		}

		public bool IsImplicit {
			get;
			internal set;
		}

		public IList<string> Patterns {
			get { return patterns; }
		}

		public IList<string> Methods {
			get { return methods; }
		}

		public MangoAction Action {
			get;
			set;
		}

		public IList<RouteHandler> Children {
			get;
			private set;
		}

		IEnumerator IEnumerable.GetEnumerator ()
		{
			return GetEnumerator ();
		}

		public IEnumerator<RouteHandler> GetEnumerator ()  
		{  
			yield return this;  
			foreach (RouteHandler child in Children) {  
				foreach (RouteHandler subchild in child) {  
					yield return subchild;  
				}
			}
		}

		public MangoAction Find (IHttpTransaction trans)
		{
			if (!IsMatch (trans.Request))
				return null;

			if (Action != null)
				return Action;

			foreach (RouteHandler handler in Children) {
				MangoAction res = handler.Find (trans);
				if (res != null)
					return res;
			}

			return null;
		}

		public bool IsMatch (HttpRequest request)
		{
			if (methods != null) {
				var meth = methods.Where (m => m == request.Method).FirstOrDefault ();
				if (meth == null)
					return false;
			}

			if (patterns != null) {
				var pat = patterns.Where (p => Regex.IsMatch (request.LocalPath, p)).FirstOrDefault ();
				if (pat == null)
					return false;
			}

			return true;
		}

		internal void Set (string [] patterns, string [] methods)
		{
			if (this.patterns == null)
				this.patterns = new List<string> (patterns);
			else
				this.patterns.AddRange (patterns);

			if (this.methods == null)
				this.methods = new List<string> (methods);
			else
				this.methods.AddRange (methods);
		}
	}
}

