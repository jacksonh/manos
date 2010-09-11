using System;
using System.Reflection;

using Manos.Testing;

using NUnit.Framework;

namespace Manos.Routing.Tests
{
	[TestFixture]
	public class ParameterizedActionTest
	{
		private static bool action_invoked_properly;
		
		[SetUp]
		public void Setup ()
		{
			action_invoked_properly = false;
		}
		
		public void SomeActionNoParams (ManosApp app, IManosContext ctx)
		{
			action_invoked_properly = true;
		}
		
		public void SomeActionBoolParam (ManosApp app, IManosContext ctx, bool b)
		{
			action_invoked_properly = b;
		}
		
		public void SomeStaticActionNoParams (ManosApp app, IManosContext ctx)
		{
			action_invoked_properly = true;	
		}
		
		public void SomeStaticActionBoolParam (ManosApp app, IManosContext ctx, bool b)
		{
			action_invoked_properly = b;	
		}
		
		public void SomeStaticActionTwoStringsShouldMatch (ManosApp app, IManosContext ctx, string a, string b)
		{
			action_invoked_properly = (a == b);	
		}
		
		private MethodInfo GetMethod (string name)
		{
			return GetType ().GetMethod (name);	
		}
		
		[Test]
		public void Invoke_NonStaticMethodNoParams_IsInvoked ()
		{
			var method = GetMethod ("SomeActionNoParams");
			var pa = ParameterizedActionFactory.CreateAction (method);
			
			pa (this, new object [] { new ManosAppStub (), new ManosContextStub () });
			
			Assert.IsTrue (action_invoked_properly);
		}
		
		[Test]
		public void Invoke_NonStaticMethodSingleBoolParam_IsInvoked ()
		{
			var method = GetMethod ("SomeActionBoolParam");
			var pa = ParameterizedActionFactory.CreateAction (method);
			
			pa (this, new object [] { new ManosAppStub (), new ManosContextStub (), true });
			
			Assert.IsTrue (action_invoked_properly);
		}
		
		[Test]
		public void Invoke_StaticMethodNoParams_IsInvoked ()
		{
			var method = GetMethod ("SomeStaticActionNoParams");
			var pa = ParameterizedActionFactory.CreateAction (method);
			
			pa (this, new object [] { new ManosAppStub (), new ManosContextStub () });
			
			Assert.IsTrue (action_invoked_properly);
		}
		
		[Test]
		public void Invoke_StaticMethodSingleBoolParam_IsInvoked ()
		{
			var method = GetMethod ("SomeStaticActionBoolParam");
			var pa = ParameterizedActionFactory.CreateAction (method);
			
			pa (this, new object [] { new ManosAppStub (), new ManosContextStub (), true });
			
			Assert.IsTrue (action_invoked_properly);
		}
		
		[Test]
		public void Invoke_StaticMethodTwoStringsShouldMatch_MatchingStringsMatch ()
		{
			var method = GetMethod ("SomeStaticActionTwoStringsShouldMatch");
			var pa = ParameterizedActionFactory.CreateAction (method);
			
			pa (this, new object [] { new ManosAppStub (), new ManosContextStub (), "foo", "foo" });
			
			Assert.IsTrue (action_invoked_properly);
		}
		
		[Test]
		public void Invoke_StaticMethodTwoStringsShouldMatch_DifferentStringsDoNotMatch ()
		{
			var method = GetMethod ("SomeStaticActionTwoStringsShouldMatch");
			var pa = ParameterizedActionFactory.CreateAction (method);
			
			action_invoked_properly = true;
			pa (this, new object [] { new ManosAppStub (), new ManosContextStub (), "foo", "bar" });
			
			Assert.IsFalse (action_invoked_properly);
		}
	}
}

