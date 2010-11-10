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
using NUnit.Framework;

using Manos.Routing;

using Manos.Http;
using Manos.Testing;
using Manos.Http.Testing;
using Manos.Routing.Testing;


using Manos.ShouldExt;

namespace Manos.Routing.Tests
{


	[TestFixture()]
	public class RouteHandlerTest
	{

		private static void FakeAction (IManosContext ctx)
		{
		}
		
		private static void FakeAction2 (IManosContext ctx)
		{
		}

		[Test()]
		public void TestStrMatch ()
		{
			var target = new MockManosTarget ();
			var rh = new RouteHandler ("^foo", HttpMethod.HTTP_GET, target);
			var request = new MockHttpRequest (HttpMethod.HTTP_GET, "foo");
			
			Assert.AreEqual (target, rh.Find (request), "should-match");
			
			request = new MockHttpRequest (HttpMethod.HTTP_GET, "garbage-foo");
			Assert.IsNull (rh.Find (request), "garbage-input");
		}
		
		[Test()]
		public void TestStrMatchDeep ()
		{
			var target = new MockManosTarget ();
			var rh = new RouteHandler ("foo/", HttpMethod.HTTP_GET) {
				new RouteHandler ("bar", HttpMethod.HTTP_GET, target),
			};

			var request = new MockHttpRequest (HttpMethod.HTTP_GET, "foo/bar");
			Assert.AreEqual (target, rh.Find (request));
			
			request = new MockHttpRequest (HttpMethod.HTTP_GET, "foo/foo");
			Assert.IsNull (rh.Find (request), "repeate-input");
			
			request = new MockHttpRequest (HttpMethod.HTTP_GET, "foo/badbar");
			Assert.IsNull (rh.Find (request), "matched-input");
		}
		
		[Test()]
		public void TestChangePatterns ()
		{
			//
			// Ensure that changing the patterns property works.
			// This is a bit of an edge case because internally
			// the patterns strings are cached as an array of 
			// regexes.
			//
			
			var target = new MockManosTarget ();
			var rh = new RouteHandler ("^foo", HttpMethod.HTTP_GET, target);
			var request = new MockHttpRequest (HttpMethod.HTTP_GET, "foo");

			Assert.AreEqual (target, rh.Find (request), "sanity-1");
			
			rh.Patterns [0] = "baz";
			Assert.IsNull (rh.Find (request), "sanity-2");
			
			request = new MockHttpRequest (HttpMethod.HTTP_GET, "baz");
			Assert.AreEqual (target, rh.Find (request), "changed");
		}
		
		[Test]
		public void TestSetPatternsNull ()
		{
			var target = new MockManosTarget ();
			var rh = new RouteHandler ("^foo", HttpMethod.HTTP_GET, target);
			var request = new MockHttpRequest (HttpMethod.HTTP_GET, "foo");

			Assert.AreEqual (target, rh.Find (request), "sanity-1");
			
			rh.Patterns = null;
			
			Assert.IsNull (rh.Find (request), "is null");
		}
		
		[Test]
		public void HasPatternsTest ()
		{
			var rh = new RouteHandler ("foo", HttpMethod.HTTP_GET);
			
			Assert.IsTrue (rh.HasPatterns, "a1");
			
			rh.Patterns.Clear ();
			Assert.IsFalse (rh.HasPatterns, "a2");
			
			rh.Patterns.Add ("foobar");
			Assert.IsTrue (rh.HasPatterns, "a3");
			
			rh.Patterns = null;
			Assert.IsFalse (rh.HasPatterns, "a4");
		}
		
		[Test]
		public void UriParamsTest ()
		{
			var rh = new RouteHandler ("(?<name>.+)", HttpMethod.HTTP_GET, new ActionTarget (FakeAction));
			var request = new MockHttpRequest (HttpMethod.HTTP_GET, "hello");
			
			Should.NotBeNull (rh.Find (request), "target");
			
			Should.NotBeNull (request.UriData, "uri-data");
			
			Assert.AreEqual ("hello", request.UriData ["name"]);	
		}
		
		[Test]
		public void UriParamsTestDeep ()
		{
			var rh = new RouteHandler ("(?<animal>.+)/", HttpMethod.HTTP_GET) {
				new RouteHandler ("(?<name>.+)", HttpMethod.HTTP_GET, new ActionTarget (FakeAction)),	                                                         
			};
			var request = new MockHttpRequest (HttpMethod.HTTP_GET, "dog/roxy");
			
			Should.NotBeNull (rh.Find (request), "target");
			
			Should.NotBeNull (request.UriData, "uri-data");
			
			Assert.AreEqual ("dog", request.UriData ["animal"]);
			Assert.AreEqual ("roxy", request.UriData ["name"]);
		}
		
		[Test]
		public void TestNoChildrenOfTarget ()
		{
			var rh = new RouteHandler ("foo", HttpMethod.HTTP_GET, new ActionTarget (FakeAction));
			
			Should.Throw<InvalidOperationException> (() => rh.Children.Add (new RouteHandler ("foo", HttpMethod.HTTP_POST)));
		}

		[Test]
		public void Find_PartialMatchAtBeginningOfChildlessHandler_ReturnsProperRoute ()
		{
			var rh_bad = new RouteHandler ("foo", HttpMethod.HTTP_GET, new ActionTarget (FakeAction));
			var rh_good = new RouteHandler ("foobar", HttpMethod.HTTP_GET, new ActionTarget (FakeAction2));
			var rh = new RouteHandler ();
			
			rh.Children.Add (rh_bad);
			rh.Children.Add (rh_good);

			
			var request = new MockHttpRequest (HttpMethod.HTTP_GET, "foobar");
			var res = rh.Find (request);
			
			Assert.AreEqual (rh_good.Target, res);
		}
		
		[Test]
		public void Find_PartialMatchAtBeginningOfHandlerWithChildren_ReturnsProperRoute ()
		{
			var rh_bad = new RouteHandler ("foo", HttpMethod.HTTP_GET);
			var rh_good = new RouteHandler ("foobar", HttpMethod.HTTP_GET, new ActionTarget (FakeAction2));
			var rh = new RouteHandler ();
			
			rh_bad.Children.Add (new RouteHandler ("blah", HttpMethod.HTTP_GET, new ActionTarget (FakeAction)));
			
			rh.Children.Add (rh_bad);
			rh.Children.Add (rh_good);

			var request = new MockHttpRequest (HttpMethod.HTTP_GET, "foobar");
			var res = rh.Find (request);
			
			Assert.AreEqual (rh_good.Target, res);
		}
	}
}
