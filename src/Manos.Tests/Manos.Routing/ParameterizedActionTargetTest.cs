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
		public void MethodWithIntArg(int i)
		{
		}
		public void MethodWithStringArg(string s)
		{
		}
		private MethodInfo GetMethodWithNoArgs ()
		{
			return GetType ().GetMethod ("MethodWithNoArgs");
		}
		private MethodInfo GetMethodWithIntArg()
		{
			return GetType().GetMethod("MethodWithIntArg");
		}
		private MethodInfo GetMethodWithStringArg()
		{
			return GetType().GetMethod("MethodWithStringArg");
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
			bool converted = ParameterizedActionTarget.TryConvertUnsafeString (ctx, typeof (string), GetMethodWithStringArg().GetParameters()[0], new UnsafeString ("foobar"), out data);
			Assert.IsTrue (converted);
		}
		
		[Test]
		public void TryConvertType_StringValue_SetsData ()
		{
			IManosContext ctx = new ManosContextStub ();
			
			object data = null;
			
			ParameterizedActionTarget.TryConvertUnsafeString (ctx, typeof (string), GetMethodWithStringArg().GetParameters()[0], new UnsafeString ("foobar"), out data);
			Assert.AreEqual ("foobar", data);
		}
		
		[Test]
		public void TryConvertType_IntValue_ReturnsTrue ()
		{
			IManosContext ctx = new ManosContextStub ();
			
			object data = null;
			bool converted = ParameterizedActionTarget.TryConvertUnsafeString (ctx, typeof (int), GetMethodWithIntArg().GetParameters()[0], new UnsafeString ("42"), out data);
			Assert.IsTrue (converted);
		}
		
		[Test]
		public void TryConvertType_IntValue_SetsData ()
		{
			IManosContext ctx = new ManosContextStub ();
			
			object data = null;
			
			ParameterizedActionTarget.TryConvertUnsafeString (ctx, typeof (int), GetMethodWithIntArg().GetParameters()[0], new UnsafeString ("42"), out data);
			Assert.AreEqual (42, data);
		}
		
		[Test]
		public void TryConvertType_BadValue_ReturnsFalse ()
		{
			IManosContext ctx = new ManosContextStub ();
			
			object data = null;
			bool converted = ParameterizedActionTarget.TryConvertUnsafeString (ctx, typeof (int), GetMethodWithIntArg().GetParameters()[0], new UnsafeString ("foobar"), out data);
			Assert.IsFalse (converted);
		}
		
		[Test]
		public void TryConvertType_IntValue_SetsDataNull ()
		{
			IManosContext ctx = new ManosContextStub ();
			
			object data = null;
			
			ParameterizedActionTarget.TryConvertUnsafeString (ctx, typeof (int), GetMethodWithIntArg().GetParameters()[0], new UnsafeString ("foobar"), out data);
			Assert.IsNull (data);
		}
	}
}

