
using System;
using NUnit.Framework;

using Mango.Routing;
using Mango.Testing;
using Mango.Server.Testing;

namespace Mango.Routing.Tests
{


	[TestFixture()]
	public class RouteHandlerTest
	{

		private static void FakeAction (IMangoContext ctx)
		{
		}
		
		[Test()]
		public void TestStrMatch ()
		{
			var target = new MockMangoTarget ();
			var rh = new RouteHandler ("^foo", "GET", target);
			var request = new MockHttpRequest ("GET", "foo");
			
			Assert.AreEqual (target, rh.Find (request), "should-match");
			
			request = new MockHttpRequest ("GET", "garbage-foo");
			Assert.IsNull (rh.Find (request), "garbage-input");
		}
		
		[Test()]
		public void TestStrMatchDeep ()
		{
			var target = new MockMangoTarget ();
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
			
			var target = new MockMangoTarget ();
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
			var target = new MockMangoTarget ();
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
			var rh = new RouteHandler ("(?<name>.+)", "GET", new MangoTarget (FakeAction));
			var request = new MockHttpRequest ("GET", "hello");
			
			Assert.NotNull (rh.Find (request), "target");
			
			Assert.NotNull (request.UriData, "uri-data");
			
			Assert.AreEqual ("hello", request.UriData ["name"]);	
		}
		
		[Test]
		public void UriParamsTestDeep ()
		{
			var rh = new RouteHandler ("(?<animal>.+)/", "GET") {
				new RouteHandler ("(?<name>.+)", "GET", new MangoTarget (FakeAction)),	                                                         
			};
			var request = new MockHttpRequest ("GET", "dog/roxy");
			
			Assert.NotNull (rh.Find (request), "target");
			
			Assert.NotNull (request.UriData, "uri-data");
			
			Assert.AreEqual ("dog", request.UriData ["animal"]);
			Assert.AreEqual ("roxy", request.UriData ["name"]);
		}
		
		[Test]
		public void TestNoChildrenOfTarget ()
		{
			var rh = new RouteHandler ("foo", "GET", new MangoTarget (FakeAction));
			
			Assert.Throws<InvalidOperationException> (() => rh.Children.Add (new RouteHandler ("foo", "POST")));
		}
	}
}
