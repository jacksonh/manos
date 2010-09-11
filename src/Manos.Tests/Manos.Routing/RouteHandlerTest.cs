
using System;
using NUnit.Framework;

using Manos.Routing;

using Manos.Testing;
using Manos.Server.Testing;
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
		
		[Test()]
		public void TestStrMatch ()
		{
			var target = new MockManosTarget ();
			var rh = new RouteHandler ("^foo", "GET", target);
			var request = new MockHttpRequest ("GET", "foo");
			
			Assert.AreEqual (target, rh.Find (request), "should-match");
			
			request = new MockHttpRequest ("GET", "garbage-foo");
			Assert.IsNull (rh.Find (request), "garbage-input");
		}
		
		[Test()]
		public void TestStrMatchDeep ()
		{
			var target = new MockManosTarget ();
			var rh = new RouteHandler ("foo/", "GET") {
				new RouteHandler ("bar", "GET", target),
			};

			var request = new MockHttpRequest ("GET", "foo/bar");
			Assert.AreEqual (target, rh.Find (request));
			
			request = new MockHttpRequest ("GET", "foo/foo");
			Assert.IsNull (rh.Find (request), "repeate-input");
			
			request = new MockHttpRequest ("GET", "foo/badbar");
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
			var rh = new RouteHandler ("^foo", "GET", target);
			var request = new MockHttpRequest ("GET", "foo");

			Assert.AreEqual (target, rh.Find (request), "sanity-1");
			
			rh.Patterns [0] = "baz";
			Assert.IsNull (rh.Find (request), "sanity-2");
			
			request = new MockHttpRequest ("GET", "baz");
			Assert.AreEqual (target, rh.Find (request), "changed");
		}
		
		[Test]
		public void TestSetPatternsNull ()
		{
			var target = new MockManosTarget ();
			var rh = new RouteHandler ("^foo", "GET", target);
			var request = new MockHttpRequest ("GET", "foo");

			Assert.AreEqual (target, rh.Find (request), "sanity-1");
			
			rh.Patterns = null;
			
			Assert.IsNull (rh.Find (request), "is null");
		}
		
		[Test]
		public void HasPatternsTest ()
		{
			var rh = new RouteHandler ("foo", "GET");
			
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
			var rh = new RouteHandler ("(?<name>.+)", "GET", new ActionTarget (FakeAction));
			var request = new MockHttpRequest ("GET", "hello");
			
			Should.NotBeNull (rh.Find (request), "target");
			
			Should.NotBeNull (request.UriData, "uri-data");
			
			Assert.AreEqual ("hello", request.UriData ["name"]);	
		}
		
		[Test]
		public void UriParamsTestDeep ()
		{
			var rh = new RouteHandler ("(?<animal>.+)/", "GET") {
				new RouteHandler ("(?<name>.+)", "GET", new ActionTarget (FakeAction)),	                                                         
			};
			var request = new MockHttpRequest ("GET", "dog/roxy");
			
			Should.NotBeNull (rh.Find (request), "target");
			
			Should.NotBeNull (request.UriData, "uri-data");
			
			Assert.AreEqual ("dog", request.UriData ["animal"]);
			Assert.AreEqual ("roxy", request.UriData ["name"]);
		}
		
		[Test]
		public void TestNoChildrenOfTarget ()
		{
			var rh = new RouteHandler ("foo", "GET", new ActionTarget (FakeAction));
			
			Should.Throw<InvalidOperationException> (() => rh.Children.Add (new RouteHandler ("foo", "POST")));
		}
	}
}
