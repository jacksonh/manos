

using System;
using NUnit.Framework;

using Manos.Templates.Testing;

namespace Manos.Templates.Tests
{
	[TestFixture]
	public class TemplateFactoryTest
	{
		[SetUp]
		public void Setup ()
		{
			//
			// Otherwise template names will be registered across tests
			//
			TemplateFactory.Clear ();
		}
		
		[Test]
		public void RegisterTemplate_NullName_Throws ()
		{
			IManosTemplate template = new ManosTemplateStub ();
			
			Assert.Throws<ArgumentNullException> (() => TemplateFactory.Register (null, template));
		}
		
		[Test]
		public void RegisterTemplate_NullTemplate_Throws ()
		{
			string name = "foobar";
			
			Assert.Throws<ArgumentNullException> (() => TemplateFactory.Register (name, null));
		}
		
		[Test]
		public void RegisterTemplate_AlreadyRegistered_Throws ()
		{
			var name = "foobar";
			var template = new ManosTemplateStub ();
			
			TemplateFactory.Register (name, template);
			
			Assert.Throws<InvalidOperationException> (() => TemplateFactory.Register (name, template));
		}
		
		[Test]
		public void RegisterTemplate_RegisterAndRetrieve_ItemIsRegistered ()
		{
			var name = "foobar";
			var expected = new ManosTemplateStub ();
			
			TemplateFactory.Register (name, expected);
			
			var retrieved = TemplateFactory.Get (name);
			
			Assert.AreEqual (expected, retrieved); 
		}
		
		[Test]
		public void TryGet_NullName_Throws ()
		{
			IManosTemplate template;
			
			Assert.Throws<ArgumentNullException> (() => TemplateFactory.TryGet (null, out template));
		}
		
		[Test]
		public void Get_NullName_Throws ()
		{
			Assert.Throws<ArgumentNullException> (() => TemplateFactory.Get (null));
		}
		
		[Test]
		public void Get_NonExistent_ReturnsNull ()
		{
			var name = "foo";
			var template = TemplateFactory.Get (name);
			
			Assert.IsNull (template);
		}
		
		[Test]
		public void Clear_RegisteredItems_UnregistersItems ()
		{
			var name = "blah";
			IManosTemplate template = new ManosTemplateStub ();
			
			TemplateFactory.Register (name, template);
			
			TemplateFactory.Clear ();
			
			template = TemplateFactory.Get (name);
			Assert.IsNull (template);
		}
		
		[Test]
		public void TryGet_NonExistant_ReturnsFalse ()
		{
			var name = "wolfbear";
			IManosTemplate template = null;
			
			Assert.IsFalse (TemplateFactory.TryGet (name, out template));
		}
		
		[Test]
		public void TryGet_NonExistant_SetsTemplateNull ()
		{
			var name = "wolfbear";
			IManosTemplate template = new ManosTemplateStub ();
			
			TemplateFactory.TryGet (name, out template);
			
			Assert.IsNull (template);
		}
		
		[Test]
		public void TryGet_RegisteredTemplate_ReturnsTrue ()
		{
			var name = "barkingpossum";
			IManosTemplate template = new ManosTemplateStub ();
			
			TemplateFactory.Register (name, template);
			
			bool found = TemplateFactory.TryGet (name, out template);
			
			Assert.IsTrue (found);
		}
		
		[Test]
		public void TryGet_RegisteredTemplate_SetsTemplate ()
		{
			var name = "manbearpig";
			IManosTemplate expected = new ManosTemplateStub ();
			
			TemplateFactory.Register (name, expected);
			
			IManosTemplate actual = null;
			TemplateFactory.TryGet (name, out actual);
			
			Assert.AreSame (expected, actual);
		}
	}
}

