using System;
using NUnit.Framework;


namespace Manos.Collections.Tests
{
	[TestFixture]
	public class DataDictionaryTest
	{
		[Test]
		public void Get_NoItemAdded_ReturnsNull ()
		{
			var dd = new DataDictionary ();
			
			string res = dd.Get ("foobar");
			Assert.IsNull (res);
		}
		
		[Test]
		public void Get_EmptyChildrenAdded_ReturnsNull ()
		{
			var dd = new DataDictionary ();
			
			dd.Children.Add (new DataDictionary ());
			dd.Children.Add (new DataDictionary ());
			
			string res = dd.Get ("foobar");
			Assert.IsNull (res);
		}
		
		[Test]
		public void Get_DifferentKeyAdded_ReturnsNull ()
		{
			var dd = new DataDictionary ();
			
			dd.Set ("blizbar", "baz");
			
			string res = dd.Get ("foobar");
			Assert.IsNull (res);
		}
		
		[Test]
		public void Get_NameSet_ReturnsItem ()
		{
			var dd = new DataDictionary ();
			
			dd.Set ("foobar", "baz");
			
			string res = dd.Get ("foobar");
			Assert.AreEqual ("baz", res);
		}
		
		[Test]
		public void Get_NameSetTwice_ReturnsSecondItem ()
		{
			var dd = new DataDictionary ();
			
			dd.Set ("foobar", "blah");
			dd.Set ("foobar", "baz");
			
			string res = dd.Get ("foobar");
			Assert.AreEqual ("baz", res);
		}
		
		[Test]
		public void Get_NameSetInChild_ReturnsItem ()
		{
			var dd = new DataDictionary ();
			var child = new DataDictionary ();
			
			dd.Children.Add (child);
			child.Set ("foobar", "baz");
			
			string res = dd.Get ("foobar");
			Assert.AreEqual ("baz", res);
		}
		
		[Test]
		public void Get_NameInDictionaryAndChild_ReturnsItemFromDictionary ()
		{
			var dd = new DataDictionary ();
			var child = new DataDictionary ();
			
			dd.Children.Add (child);
			child.Set ("foobar", "nooo");
			dd.Set ("foobar", "baz");
			
			string res = dd.Get ("foobar");
			Assert.AreEqual ("baz", res);
		}
		
		[Test]
		public void Get_MultipleChildrenWithKey_ReturnsFirstChildAdded ()
		{
			var dd = new DataDictionary ();
			var child = new DataDictionary ();
			
			dd.Children.Add (child);
			child.Set ("foobar", "baz");
			
			child = new DataDictionary ();
			dd.Children.Add (child);
			child.Set ("foobar", "blahh");
			
			child = new DataDictionary ();
			dd.Children.Add (child);
			child.Set ("foobar", "bork");
			
			string res = dd.Get ("foobar");
			Assert.AreEqual ("baz", res);
		}
	}
}

