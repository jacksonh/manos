

using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;


using Mango.Server;
using System.Collections.Specialized;

namespace Mango.Routing {

	public class RouteHandler : IEnumerable<RouteHandler> {

		private List<string> patterns;
		private List<string> methods;

		private IMatchOperation [] match_ops;

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
				UpdateMatchOps ();
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

		//
		// TODO: These also need to be an observable collection
		// so we can throw an exception if someone tries to add 
		// a child when we already have a Target
		//
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
			NameValueCollection uri_data = null;
			if (HasPatterns) {
				int end;
				uri_data = new NameValueCollection ();
				
				if (!FindPatternMatch (request.LocalPath, uri_start, uri_data, out end))
					return null;

				if (Target != null) {
					request.UriData.Add (uri_data);
					return Target;
				}

				uri_start = end;
			}

			foreach (RouteHandler handler in Children) {
				IMangoTarget res = handler.Find (request, uri_start);
				if (res != null) {
					if (uri_data != null)
						request.UriData.Add (uri_data);
					return res;
				}
			}

			return null;
		}

		public bool FindPatternMatch (string input, int start, NameValueCollection uri_data, out int end)
		{
			if (match_ops == null) {
				end = start;
				return false;
			}
			
			foreach (IMatchOperation op in match_ops) {
				if (op.IsMatch (input, start, uri_data, out end)) {
					return true;
				}
			}
			
			end = start;
			return false;
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
		
		private void UpdateMatchOps ()
		{
			if (patterns == null) {
				match_ops = null;
				return;
			}
			
			match_ops = new IMatchOperation [patterns.Count];
			
			for (int i = 0; i < patterns.Count; i++) {
				match_ops [i] = MatchOperationFactory.Create (patterns [i]);	
			}
		}
	}
}

