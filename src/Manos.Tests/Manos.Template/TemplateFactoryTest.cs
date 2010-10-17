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

