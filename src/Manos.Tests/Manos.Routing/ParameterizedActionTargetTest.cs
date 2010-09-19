using System;
using System.Reflection;

using NUnit.Framework;
using Manos.ShouldExt;

using Manos.Testing;
using Manos.Routing.Testing;

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
		
		[Test]
		public void TryConvertType_StringValue_ReturnsTrue ()
		{
			IManosContext ctx = new ManosContextStub ();
			
			object data = null;
			bool converted = ParameterizedActionTarget.TryConvertType (ctx, typeof (string), new UnsafeString ("foobar"), out data);
			Assert.IsTrue (converted);
		}
		
		[Test]
		public void TryConvertType_StringValue_SetsData ()
		{
			IManosContext ctx = new ManosContextStub ();
			
			object data = null;
			
			ParameterizedActionTarget.TryConvertType (ctx, typeof (string), new UnsafeString ("foobar"), out data);
			Assert.AreEqual ("foobar", data);
		}
		
		[Test]
		public void TryConvertType_IntValue_ReturnsTrue ()
		{
			IManosContext ctx = new ManosContextStub ();
			
			object data = null;
			bool converted = ParameterizedActionTarget.TryConvertType (ctx, typeof (int), new UnsafeString ("42"), out data);
			Assert.IsTrue (converted);
		}
		
		[Test]
		public void TryConvertType_IntValue_SetsData ()
		{
			IManosContext ctx = new ManosContextStub ();
			
			object data = null;
			
			ParameterizedActionTarget.TryConvertType (ctx, typeof (int), new UnsafeString ("42"), out data);
			Assert.AreEqual (42, data);
		}
		
		[Test]
		public void TryConvertType_BadValue_ReturnsFalse ()
		{
			IManosContext ctx = new ManosContextStub ();
			
			object data = null;
			bool converted = ParameterizedActionTarget.TryConvertType (ctx, typeof (int), new UnsafeString ("foobar"), out data);
			Assert.IsFalse (converted);
		}
		
		[Test]
		public void TryConvertType_IntValue_SetsDataNull ()
		{
			IManosContext ctx = new ManosContextStub ();
			
			object data = null;
			
			ParameterizedActionTarget.TryConvertType (ctx, typeof (int), new UnsafeString ("foobar"), out data);
			Assert.IsNull (data);
		}
	}
}

