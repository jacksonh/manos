
using System;
using NUnit.Framework;

using Mango;
using Mango.Testing;
using Mango.Testing.Server;

namespace Mango.Tests
{


	[TestFixture()]
	public class MangoModuleTest
	{

		private void FakeAction (IMangoContext ctx)
		{
		}
		
		private class FakeModule : MockMangoModule {

			public static void FakeAction (IMangoContext ctx)
			{
			}
		}
		
		[Test()]
		public void TestAddRouteNull ()
		{
			MangoModule m = new MangoModule ();
			
			Assert.Throws<ArgumentNullException> (() => m.Route (new MangoModule (), null, null));
			Assert.Throws<ArgumentNullException> (() => m.Route (new MangoAction (FakeAction), null, null));
			Assert.Throws<ArgumentNullException> (() => m.Route (null, new MangoAction (FakeAction)));
			Assert.Throws<ArgumentNullException> (() => m.Route (null, new MangoModule ()));                          
		}
		
		[Test]
		public void TestRouteToTarget ()
		{
			string [] methods = new string [] {
				"GET", "PUT", "POST", "HEAD", "DELETE", "TRACE", "OPTIONS",
			};
			
			for (int i = 0; i < methods.Length; i++) {
				var m = new MockMangoModule ();
				var req = new MockHttpRequest (methods [i], "Foobar");
			
				m.Route ("Foobar", new MangoAction (FakeAction));
			
				Assert.AreEqual (new MangoAction (FakeAction), m.Routes.Find (req).Action);
			}
		}
		
		[Test]
		public void TestRouteToModule ()
		{
			string [] methods = new string [] {
				"GET", "PUT", "POST", "HEAD", "DELETE", "TRACE", "OPTIONS",
			};
			
			for (int i = 0; i < methods.Length; i++) {
				var m = new MockMangoModule ();
				var req = new MockHttpRequest (methods [i], "FakeModule/FakeAction");
			
				m.Route ("FakeModule/", new FakeModule ());
			
				//
				// I guess technically this is testing the auto name registering too
				//
				Assert.AreEqual (new MangoAction (FakeModule.FakeAction), m.Routes.Find (req).Action);	
			}
		}
	}
}
