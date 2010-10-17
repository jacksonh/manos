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

