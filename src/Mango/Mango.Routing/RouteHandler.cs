

using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;


using Mango.Server;

namespace Mango.Routing {

	public class RouteHandler : IEnumerable<RouteHandler> {

		private List<string> patterns;
		private List<string> methods;

		private Regex [] regexes;

		internal RouteHandler ()
		{
			Children = new List<RouteHandler> ();
		}

		public RouteHandler (string pattern, string [] methods) : this (new string [] { pattern }, methods)
		{
		}
		
		public RouteHandler (string pattern, string [] methods, IMangoTarget target) : this (new string [] { pattern }, methods, target)
		{
		}
		
		public RouteHandler (string pattern, string method) : this (new string [] { pattern }, new string [] { method })
		{
		}
		
		public RouteHandler (string pattern, string method, IMangoTarget target) : this (new string [] { pattern }, new string [] { method }, target)
		{
		}
		
		public RouteHandler (string [] patterns, string [] methods) : this (patterns, methods, null)
		{
		}

		public RouteHandler (string [] patterns, string [] methods, IMangoTarget target)
		{
			Target = target;

			Patterns = new List<string> (patterns);
			this.methods = new List<string> (methods);

			Children = new List<RouteHandler> ();
		}

		public bool IsImplicit {
			get;
			internal set;
		}

		public IList<string> Patterns {
			get { return patterns; }
			set {
				
				//
				// TODO: This needs to be an observablecollection so 
				// I can raise an event when its changed and rebuild the 
				// regex cache.
				//
				// Only reason it isn't now is because i don't have a 4.0
				// profile installed in monodevelop
				//
				
				if (value == null)
					patterns = null;
				else
					patterns = new List<string> (value);
				UpdateRegexes ();
			}
		}

		public IList<string> Methods {
			get { return methods; }
			set {
				if (value == null)
					methods = null;
				else
					methods = new List<string> (value); 
			}
		}

		public IMangoTarget Target {
			get;
			set;
		}

		public IList<RouteHandler> Children {
			get;
			private set;
		}

		public bool HasPatterns {
			get { 
				return patterns != null && patterns.Count > 0;
			}
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

		public void Add (RouteHandler child)
		{
			Children.Add (child);	
		}
		
		public IMangoTarget Find (IHttpRequest request)
		{
			return Find (request, 0);
		}
		
		private IMangoTarget Find (IHttpRequest request, int uri_start)
		{
			if (!IsMethodMatch (request))
				return null;
			
			Match m = null;
			if (HasPatterns) {
				m = FindPatternMatch (request.LocalPath, uri_start);

				Console.WriteLine (" -- got match:  {0}", m);
				if (m == null)
					return null;
				
				if (Target != null)
					return Target;

				uri_start = m.Index + m.Length;
			}
			
			Console.WriteLine (" checking children:  {0}", Children.Count);
			foreach (RouteHandler handler in Children) {
				IMangoTarget res = handler.Find (request, uri_start);
				if (res != null)
					return res;
			}

			return null;
		}

		public Match FindPatternMatch (string input, int start)
		{
			if (regexes == null)
				return null;
			
			foreach (Regex r in regexes) {
				Console.WriteLine ("checking patterns: {0}  for {1}", r, input.Substring (start));
				Match m = r.Match (input, start);
				if (m.Success && m.Index == start)
					return m;
			}
			
			return null;
		}
		
		public bool IsMethodMatch (IHttpRequest request)
		{
			if (methods != null) {
				var meth = methods.Where (m => m == request.Method).FirstOrDefault ();
				if (meth == null)
					return false;
			}

			return true;
		}

		private void UpdateRegexes ()
		{
			if (patterns == null) {
				regexes = null;
				return;
			}

			regexes = new Regex [patterns.Count];
			for (int i = 0; i < patterns.Count; i++) {
				regexes [i] = new Regex (patterns [i]);
			}
		}
	}
}

