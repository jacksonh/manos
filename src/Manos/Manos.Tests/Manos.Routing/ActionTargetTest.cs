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

