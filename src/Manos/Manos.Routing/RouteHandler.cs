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
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.Collections.Specialized;

using Manos.Http;
using Manos.Collections;

namespace Manos.Routing {

	public class RouteHandler : IEnumerable<RouteHandler> {

		private ObservableCollection<string> patterns;
		private List<HttpMethod> methods;

		private IMatchOperation [] match_ops;

		public RouteHandler ()
		{
			SetupChildrenCollection ();
		}

		void HandleChildrenCollectionChanged (object sender, NotifyCollectionChangedEventArgs e)
		{
			if (Target != null)
				throw new InvalidOperationException ("Can not add Children to a RouteHandler that has a Target set.");	
		}

		public RouteHandler (string pattern, HttpMethod [] methods) : this (new string [] { pattern }, methods)
		{
		}
		
		public RouteHandler (string pattern, HttpMethod [] methods, IManosTarget target) : this (new string [] { pattern }, methods, target)
		{
		}
		
		public RouteHandler (string pattern, HttpMethod method) : this (new string [] { pattern }, new HttpMethod [] { method })
		{
		}
		
		public RouteHandler (string pattern, HttpMethod method, IManosTarget target) : this (new string [] { pattern }, new HttpMethod [] { method }, target)
		{
		}
		
		public RouteHandler (string [] patterns, HttpMethod [] methods) : this (patterns, methods, null)
		{
		}

		public RouteHandler (string [] patterns, HttpMethod [] methods, IManosTarget target)
		{
			Target = target;

			Patterns = new List<string> (patterns);
			this.methods = new List<HttpMethod> (methods);

			SetupChildrenCollection ();
		}
			
		private void SetupChildrenCollection ()
		{
			var children = new ObservableCollection<RouteHandler> ();
			
			children.CollectionChanged += HandleChildrenCollectionChanged;
			Children = children;
		}

		public bool IsImplicit {
			get;
			internal set;
		}

		public IList<string> Patterns {
			get { return patterns; }
			set {
				if (value == null)
				{
					patterns = null;
				}
				else 
				{
					patterns = new ObservableCollection<string> ();
					//
					// TODO: ObservableCollection (IList) isn't implemented yet.
					//
					foreach (string s in value)
					{
						patterns.Add (s);
					}
					patterns.CollectionChanged += delegate(object sender, NotifyCollectionChangedEventArgs e) 
					{
						UpdateMatchOps ();
					};
				}
				UpdateMatchOps ();
			}
		}

		public IList<HttpMethod> Methods {
			get { return methods; }
			set {
				if (value == null)
					methods = null;
				else
					methods = new List<HttpMethod> (value); 
			}
		}

		public IManosTarget Target {
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
		
		public IManosTarget Find (IHttpRequest request)
		{
			return Find (request, 0);
		}
		
		private IManosTarget Find (IHttpRequest request, int uri_start)
		{
			if (!IsMethodMatch (request))
				return null;
						
			DataDictionary uri_data = null;
			if (HasPatterns) {
				int end;
				
				if (!FindPatternMatch (request.Path, uri_start, out uri_data, out end)) {
					return null;
				}

				if (Target != null) {
					if (uri_data != null)
						request.UriData.Children.Add (uri_data);
					return Target;
				}

				uri_start = end;
			}

			foreach (RouteHandler handler in Children) {
				IManosTarget res = handler.Find (request, uri_start);
				if (res != null) {
					if (uri_data != null)
						request.UriData.Children.Add (uri_data);
					return res;
				}
			}

			return null;
		}

		public bool FindPatternMatch (string input, int start, out DataDictionary uri_data, out int end)
		{
			if (match_ops == null) {
				uri_data = null;
				end = start;
				return false;
			}
			
			foreach (IMatchOperation op in match_ops) {
				if (op.IsMatch (input, start, out uri_data, out end)) {
					if (Children.Count () > 0 || end == input.Length) {
						return true;
					}
				}
			}

			uri_data = null;
			end = start;
			return false;
		}
		
		public bool IsMethodMatch (IHttpRequest request)
		{
			if (methods != null)
				return methods.Contains (request.Method);

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

