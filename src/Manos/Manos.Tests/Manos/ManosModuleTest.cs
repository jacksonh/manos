
using System;
using NUnit.Framework;

using Manos;
using Manos.Routing;
using Manos.Testing;
using Manos.Server.Testing;

using Manos.ShouldExt;

namespace Manos.Tests
{
		
	[TestFixture()]
	public class ManosModuleTest
	{

		private void FakeAction (IManosContext ctx)
		{
		}
		
		private class FakeModule : MockManosModule {

			public static void FakeAction (IManosContext ctx)
			{
			}
		}
		
		[Test()]
		public void TestAddRouteNull ()
		{
			ManosModule m = new ManosModule ();
											
			Should.Throw<Exception> (() => Console.WriteLine ());
			
			Should.Throw<ArgumentNullException> (() => m.Route (new ManosModule (), null, null));
			Should.Throw<ArgumentNullException> (() => m.Route (new ManosAction (FakeAction), null, null));
			Should.Throw<ArgumentNullException> (() => m.Route (null, new ManosAction (FakeAction)));
			Should.Throw<ArgumentNullException> (() => m.Route (null, new ManosModule ()));                          
		}
		
		[Test]
		public void TestRouteToTarget ()
		{
			string [] methods = new string [] {
				"GET", "PUT", "POST", "HEAD", "DELETE", "TRACE", "OPTIONS",
			};
			
			for (int i = 0; i < methods.Length; i++) {
				var m = new MockManosModule ();
				var req = new MockHttpRequest (methods [i], "/Foobar");
			
				m.Route ("/Foobar", new ManosAction (FakeAction));
			
				Assert.AreEqual (new ManosAction (FakeAction), m.Routes.Find (req).Action);
			}
		}
		
		[Test]
		public void TestRouteNull ()
		{
			var m = new MockManosModule ();
			
			ManosModule mm = null;
			Should.Throw<ArgumentNullException> (() => m.Route ("foo", mm), "a1");
			Should.Throw<ArgumentNullException> (() => m.Route (mm, "foo", "bar", "baz"), "a2");
			
			ManosAction ma = null;
			Should.Throw<ArgumentNullException> (() => m.Route ("foo", ma), "a3");
			Should.Throw<ArgumentNullException> (() => m.Route (ma, "foo", "bar", "baz"), "a4");
			
			mm = new MockManosModule ();
			Should.Throw<ArgumentNullException> (() => m.Route (null, mm), "a4");
			Should.Throw<ArgumentNullException> (() => m.Route (mm, "foo", "bar", "baz", null), "a5");
			
			ma = FakeAction;
			Should.Throw<ArgumentNullException> (() => m.Route (null, ma), "a6");
			Should.Throw<ArgumentNullException> (() => m.Route (ma, "foo", "bar", "baz", null), "a7");
		}
		
		[Test]
		public void TestRouteToModule ()
		{
			string [] methods = new string [] {
				"GET", "PUT", "POST", "HEAD", "DELETE", "TRACE", "OPTIONS",
			};
			
			for (int i = 0; i < methods.Length; i++) {
				var m = new MockManosModule ();
				var req = new MockHttpRequest (methods [i], "/FakeModule/FakeAction");
			
				m.Route ("/FakeModule", new FakeModule ());
			
				//
				// I guess technically this is testing the auto name registering too
				//
				Assert.AreEqual (new ManosAction (FakeModule.FakeAction), m.Routes.Find (req).Action);	
			}
		}
		
		[Test]
		public void TestGetToTarget ()
		{
			var m = new MockManosModule ();
			var req = new MockHttpRequest ("GET", "/Foobar");
			
			m.Get ("/Foobar", new ManosAction (FakeAction));
			Assert.AreEqual (new ManosAction (FakeAction), m.Routes.Find (req).Action);
			
			req = new MockHttpRequest ("POST", "/Foobar");
			Assert.IsNull (m.Routes.Find (req));
		}
		
		[Test]
		public void TestGetToModule ()
		{
			var m = new MockManosModule ();
			var req = new MockHttpRequest ("GET", "/FakeModule/FakeAction");
			
			m.Get ("/FakeModule", new FakeModule ());
			
			//
			// I guess technically this is testing the auto name registering too
			//
			Assert.AreEqual (new ManosAction (FakeModule.FakeAction), m.Routes.Find (req).Action);
			
			req = new MockHttpRequest ("POST", "/FakeModule/FakeAction");
			Assert.IsNull (m.Routes.Find (req));
		}
		
		[Test]
		public void TestPutToTarget ()
		{
			var m = new MockManosModule ();
			var req = new MockHttpRequest ("PUT", "/Foobar");
			
			m.Put ("/Foobar", new ManosAction (FakeAction));
			Assert.AreEqual (new ManosAction (FakeAction), m.Routes.Find (req).Action);
			
			req = new MockHttpRequest ("POST", "/Foobar");
			Assert.IsNull (m.Routes.Find (req));
		}
		
		[Test]
		public void TestPutToModule ()
		{
			var m = new MockManosModule ();
			var req = new MockHttpRequest ("PUT", "/FakeModule/FakeAction");
			
			m.Put ("/FakeModule", new FakeModule ());
			
			//
			// I guess technically this is testing the auto name registering too
			//
			Assert.AreEqual (new ManosAction (FakeModule.FakeAction), m.Routes.Find (req).Action);
			
			req = new MockHttpRequest ("POST", "/FakeModule/FakeAction");
			Assert.IsNull (m.Routes.Find (req));
		}
		
		[Test]
		public void TestPostToTarget ()
		{
			var m = new MockManosModule ();
			var req = new MockHttpRequest ("POST", "/Foobar");
			
			m.Post ("/Foobar", new ManosAction (FakeAction));
			Assert.AreEqual (new ManosAction (FakeAction), m.Routes.Find (req).Action);
			
			req = new MockHttpRequest ("GET", "/Foobar");
			Assert.IsNull (m.Routes.Find (req));
		}
		
		[Test]
		public void TestPostToModule ()
		{
			var m = new MockManosModule ();
			var req = new MockHttpRequest ("POST", "/FakeModule/FakeAction");
			
			m.Post ("/FakeModule", new FakeModule ());
			
			//
			// I guess technically this is testing the auto name registering too
			//
			Assert.AreEqual (new ManosAction (FakeModule.FakeAction), m.Routes.Find (req).Action);
			
			req = new MockHttpRequest ("GET", "/FakeModule/FakeAction");
			Assert.IsNull (m.Routes.Find (req));
		}
		
		[Test]
		public void TestDeleteToTarget ()
		{
			var m = new MockManosModule ();
			var req = new MockHttpRequest ("DELETE", "/Foobar");
			
			m.Delete ("/Foobar", new ManosAction (FakeAction));
			Assert.AreEqual (new ManosAction (FakeAction), m.Routes.Find (req).Action);
			
			req = new MockHttpRequest ("GET", "/Foobar");
			Assert.IsNull (m.Routes.Find (req));
		}
		
		[Test]
		public void TestDeleteToModule ()
		{
			var m = new MockManosModule ();
			var req = new MockHttpRequest ("DELETE", "/FakeModule/FakeAction");
			
			m.Delete ("/FakeModule", new FakeModule ());
			
			//
			// I guess technically this is testing the auto name registering too
			//
			Assert.AreEqual (new ManosAction (FakeModule.FakeAction), m.Routes.Find (req).Action);
			
			req = new MockHttpRequest ("GET", "/FakeModule/FakeAction");
			Assert.IsNull (m.Routes.Find (req));
		}
		
		[Test]
		public void TestHeadToTarget ()
		{
			var m = new MockManosModule ();
			var req = new MockHttpRequest ("HEAD", "/Foobar");
			
			m.Head ("/Foobar", new ManosAction (FakeAction));
			Assert.AreEqual (new ManosAction (FakeAction), m.Routes.Find (req).Action);
			
			req = new MockHttpRequest ("GET", "/Foobar");
			Assert.IsNull (m.Routes.Find (req));
		}
		
		[Test]
		public void TestHeadToModule ()
		{
			var m = new MockManosModule ();
			var req = new MockHttpRequest ("HEAD", "/FakeModule/FakeAction");
			
			m.Head ("/FakeModule", new FakeModule ());
			
			//
			// I guess technically this is testing the auto name registering too
			//
			Assert.AreEqual (new ManosAction (FakeModule.FakeAction), m.Routes.Find (req).Action);
			
			req = new MockHttpRequest ("GET", "/FakeModule/FakeAction");
			Assert.IsNull (m.Routes.Find (req));
		}
		
		[Test]
		public void TestOptionsToTarget ()
		{
			var m = new MockManosModule ();
			var req = new MockHttpRequest ("OPTIONS", "/Foobar");
			
			m.Options ("/Foobar", new ManosAction (FakeAction));
			Assert.AreEqual (new ManosAction (FakeAction), m.Routes.Find (req).Action);
			
			req = new MockHttpRequest ("GET", "/Foobar");
			Assert.IsNull (m.Routes.Find (req));
		}
		
		[Test]
		public void TestOptionsToModule ()
		{
			var m = new MockManosModule ();
			var req = new MockHttpRequest ("OPTIONS", "/FakeModule/FakeAction");
			
			m.Options ("/FakeModule", new FakeModule ());
			
			//
			// I guess technically this is testing the auto name registering too
			//
			Assert.AreEqual (new ManosAction (FakeModule.FakeAction), m.Routes.Find (req).Action);
			
			req = new MockHttpRequest ("GET", "/FakeModule/FakeAction");
			Assert.IsNull (m.Routes.Find (req));
		}
		
		[Test]
		public void TestTraceToTarget ()
		{
			var m = new MockManosModule ();
			var req = new MockHttpRequest ("TRACE", "/Foobar");
			
			m.Trace ("/Foobar", new ManosAction (FakeAction));
			Assert.AreEqual (new ManosAction (FakeAction), m.Routes.Find (req).Action);
			
			req = new MockHttpRequest ("GET", "/Foobar");
			Assert.IsNull (m.Routes.Find (req));
		}
		
		[Test]
		public void TestTraceToModule ()
		{
			var m = new MockManosModule ();
			var req = new MockHttpRequest ("TRACE", "/FakeModule/FakeAction");
			
			m.Trace ("/FakeModule", new FakeModule ());
			
			//
			// I guess technically this is testing the auto name registering too
			//
			Assert.AreEqual (new ManosAction (FakeModule.FakeAction), m.Routes.Find (req).Action);
			
			req = new MockHttpRequest ("GET", "/FakeModule/FakeAction");
			Assert.IsNull (m.Routes.Find (req));
		}
	}
}
