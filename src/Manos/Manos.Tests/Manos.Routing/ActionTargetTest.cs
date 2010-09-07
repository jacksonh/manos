using System;
using NUnit.Framework;

using Manos.Testing;


namespace Manos.Routing.Tests
{
	[TestFixture]
	public class ManosActionTargetTest
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
			Assert.Throws<ArgumentNullException> (() => new ManosActionTarget (null));
		}
		
		[Test]
		public void Ctor_ValidActon_SetsAction ()
		{
			var mat = new ManosActionTarget (ValidAction);
			
			Assert.AreEqual (new ManosAction (ValidAction), mat.Action);
		}
		
		[Test]
		public void ActionSetter_NullAction_Throws ()
		{
			var mat = new ManosActionTarget (ValidAction);
			
			Assert.Throws<ArgumentNullException> (() => mat.Action = null);
		}
		
		[Test]
		public void ActionSetter_InvalidDelegateType_Throws ()
		{
			var mat = new ManosActionTarget (new ManosAction (ValidAction));
			
			Assert.Throws<InvalidOperationException> (() => mat.Action = new InvalidDelegate (InvalidAction));
		}
		
		[Test]
		public void Invoke_ActionIsInvoked ()
		{
			bool action_set = false;
			var mat = new ManosActionTarget (ctx => action_set = true);
			
			mat.Invoke (new ManosAppStub (), new ManosContextStub ());
			
			Assert.IsTrue (action_set);
		}
	}
}

