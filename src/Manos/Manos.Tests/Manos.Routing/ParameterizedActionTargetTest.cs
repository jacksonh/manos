using System;
using System.Reflection;

using NUnit.Framework;

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
			
			Assert.DoesNotThrow (() => new ParameterizedActionTarget (null, method, pa));
		}
		
		[Test]
		public void Ctor_NullMethod_Throws ()
		{
			var method = GetMethodWithNoArgs ();
			var pa = ParameterizedActionFactory.CreateAction (method);
			
			Assert.Throws<ArgumentNullException> (() => new ParameterizedActionTarget ("foobar", null, pa));
		}
		
		[Test]
		public void Ctor_NullAction_Throws ()
		{
			var method = GetMethodWithNoArgs ();
			
			Assert.Throws<ArgumentNullException> (() => new ParameterizedActionTarget ("foobar", method, null));
		}
	}
}

