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
using NUnit.Framework;

using Manos.Testing;
using Manos.ShouldExt;


namespace Manos.Routing.Tests
{
	[TestFixture]
	public class ActionTargetTest
	{
		private void ValidAction (IManosContext ctx)
		{
		}
		
		private void InvalidAction ()
		{
		}
		
		private delegate void InvalidDelegate ();
		
		[Test]
		public void Ctor_NullArgument_Throws ()
		{
			Should.Throw<ArgumentNullException> (() => new ActionTarget (null));
		}
		
		[Test]
		public void Ctor_ValidActon_SetsAction ()
		{
			var mat = new ActionTarget (ValidAction);
			
			Assert.AreEqual (new ManosAction (ValidAction), mat.Action);
		}
		
		[Test]
		public void ActionSetter_NullAction_Throws ()
		{
			var mat = new ActionTarget (ValidAction);
			
			Should.Throw<ArgumentNullException> (() => mat.Action = null);
		}
		
		[Test]
		public void ActionSetter_InvalidDelegateType_Throws ()
		{
			var mat = new ActionTarget (new ManosAction (ValidAction));
			
			Should.Throw<InvalidOperationException> (() => mat.Action = new InvalidDelegate (InvalidAction));
		}
		
		[Test]
		public void Invoke_ActionIsInvoked ()
		{
			bool action_set = false;
			var mat = new ActionTarget (ctx => action_set = true);
			
			mat.Invoke (new ManosAppStub (), new ManosContextStub ());
			
			Assert.IsTrue (action_set);
		}
	}
}

