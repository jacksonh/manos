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
using Manos.ShouldExt;


namespace Manos.Caching.Tests
{
	[TestFixture()]
	public class ManosInProcCacheTest
	{
		
		public class ManosInProcCacheStub : ManosInProcCache {
		
			public void ForceHandleExpires (object item)
			{
				base.HandleExpires (null, item);
			}
			
			public ManosInProcCache.CacheItem DoSetInternal (string key, object value)
			{
				return base.SetInternal (key, value);	
			}
		}
		

		[Test]
		public void Get_NullKey_Throws ()
		{
			var cache = new ManosInProcCache ();
			
			Should.Throw<ArgumentNullException> (() => cache.Get (null, null));
		}
		
		[Test]
		public void Get_KeyThatHasntBeenAdded_ReturnsNull ()
		{
			var cache = new ManosInProcCache ();
			
			cache.Get ("foobar", (name, value) => {
				Assert.IsNull (value);
			});
		}
		
		[Test]
		public void Set_NullKey_Throws ()
		{
			var cache = new ManosInProcCache ();
			
			var item = "foo";
			Should.Throw<ArgumentNullException> (() => cache.Set (null, item));
		}
		
		[Test]
		public void Set_NullItem_DoesNotThrow ()
		{
			var cache = new ManosInProcCache ();
			
			Should.NotThrow(() => cache.Set ("foo", null));
		}
		
		[Test]
		public void Set_NewKey_AddsItem ()
		{
			var cache = new ManosInProcCache ();
			var bar = new object ();
			
			cache.Set ("foo", bar);
			
			cache.Get ("foo", (name, item) => {
				Assert.AreSame (bar, item);
			});
		}
		
		[Test]
		public void Set_KeyAlreadyExists_ReplacesItem ()
		{
			var cache = new ManosInProcCache ();
			var existing = new object ();
			var new_item = new object ();
			
			cache.Set ("foo", existing);
			cache.Set ("foo", new_item);
			
			cache.Get ("foo", (name, item) => {
				Assert.AreSame (new_item, item);
			});
		}
		
		[Test]
		public void Set_NullItem_RemovesItem ()
		{
			var cache = new ManosInProcCache ();
			var existing = new object ();
			
			cache.Set ("foo", existing);
			cache.Set ("foo", null);
			
			cache.Get ("foo", (name, item) => {
				Assert.IsNull (item);
			});
		}
		
		[Test]
		public void Clear_RemovesRegisteredItem ()
		{
			var cache = new ManosInProcCache ();
			var existing = new object ();
			
			cache.Set ("foo", existing);
			cache.Clear ();
			
			cache.Get ("foo", (name, item) => {
				Assert.IsNull (item);
			});
		}
		
		[Test]
		public void HandleExpires_RegisteredItem_RemovesItem ()
		{
			var cache = new ManosInProcCacheStub ();
			var existing = new object ();
			
			ManosInProcCache.CacheItem item = cache.DoSetInternal ("foo", existing);
			cache.ForceHandleExpires (item);
			
			cache.Get ("foo", (name, get_item) => {
				Assert.IsNull (get_item);
			});
		}
		
		[Test]
		public void HandleExpires_ItemAlreadyRemoved_DoesNotThrow ()
		{
			var cache = new ManosInProcCacheStub ();
			var existing = new object ();
			
			ManosInProcCache.CacheItem item = cache.DoSetInternal ("foo", existing);
			cache.Remove ("foo");
			
			Should.NotThrow (() => cache.ForceHandleExpires (item));
		}
	}
}

