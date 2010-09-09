using System;
using System.Reflection;

using NUnit.Framework;
using Manos.ShouldExt;

namespace Manos.Routing.Tests
{
	
	[TestFixture()]
	public class ParameterizedActionTargetTest
	{
		public void MethodWithNoArgs ()
		{
		}
		
		private MethodInfo GetMethodWithNoArgs ()
		{
			return GetType ().GetMethod ("MethodWithNoArgs");
		}
		
		[Test]
		public void Ctor_NullTarget_DoesNotThrow ()
		{
			var method = GetMethodWithNoArgs ();
			var pa = ParameterizedActionFactory.CreateAction (method);
			
			Should.NotThrow (() => new ParameterizedActionTarget (null, method, pa));
		}
		
		[Test]
		public void Ctor_NullMethod_Throws ()
		{
			var method = GetMethodWithNoArgs ();
			var pa = ParameterizedActionFactory.CreateAction (method);
			
			Should.Throw<ArgumentNullException> (() => new ParameterizedActionTarget ("foobar", null, pa));
		}
		
		[Test]
		public void Ctor_NullAction_Throws ()
		{
			var method = GetMethodWithNoArgs ();
			
			Should.Throw<ArgumentNullException> (() => new ParameterizedActionTarget ("foobar", method, null));
		}
	}
}

