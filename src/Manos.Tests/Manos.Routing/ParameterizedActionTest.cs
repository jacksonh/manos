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

