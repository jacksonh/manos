using System;
using NUnit.Framework;


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
			
			Assert.Throws<ArgumentNullException> (() => cache.Get (null));
		}
		
		[Test]
		public void Get_KeyThatHasntBeenAdded_ReturnsNull ()
		{
			var cache = new ManosInProcCache ();
			
			var item = cache.Get ("foobar");
			Assert.IsNull (item);
		}
		
		[Test]
		public void Set_NullKey_Throws ()
		{
			var cache = new ManosInProcCache ();
			
			var item = "foo";
			Assert.Throws<ArgumentNullException> (() => cache.Set (null, item));
		}
		
		[Test]
		public void Set_NullItem_DoesNotThrow ()
		{
			var cache = new ManosInProcCache ();
			
			Assert.DoesNotThrow(() => cache.Set ("foo", null));
		}
		
		[Test]
		public void Set_NewKey_AddsItem ()
		{
			var cache = new ManosInProcCache ();
			var bar = new object ();
			
			cache.Set ("foo", bar);
			
			var retrieved = cache.Get ("foo");
			Assert.AreSame (bar, retrieved);
		}
		
		[Test]
		public void Set_KeyAlreadyExists_ReplacesItem ()
		{
			var cache = new ManosInProcCache ();
			var existing = new object ();
			var new_item = new object ();
			
			cache.Set ("foo", existing);
			cache.Set ("foo", new_item);
			
			var retrieved = cache.Get ("foo");
			Assert.AreSame (new_item, retrieved);
		}
		
		[Test]
		public void Set_NullItem_RemovesItem ()
		{
			var cache = new ManosInProcCache ();
			var existing = new object ();
			
			cache.Set ("foo", existing);
			cache.Set ("foo", null);
			
			var retrieved = cache.Get ("foo");
			Assert.IsNull (retrieved);
		}
		
		[Test]
		public void Clear_RemovesRegisteredItem ()
		{
			var cache = new ManosInProcCache ();
			var existing = new object ();
			
			cache.Set ("foo", existing);
			cache.Clear ();
			
			var retrieved = cache.Get ("foo");
			Assert.IsNull (retrieved);
		}
		
		[Test]
		public void HandleExpires_RegisteredItem_RemovesItem ()
		{
			var cache = new ManosInProcCacheStub ();
			var existing = new object ();
			
			ManosInProcCache.CacheItem item = cache.DoSetInternal ("foo", existing);
			cache.ForceHandleExpires (item);
			
			var retrieved = cache.Get ("foo");
			Assert.IsNull (retrieved);
		}
		
		[Test]
		public void HandleExpires_ItemAlreadyRemoved_DoesNotThrow ()
		{
			var cache = new ManosInProcCacheStub ();
			var existing = new object ();
			
			ManosInProcCache.CacheItem item = cache.DoSetInternal ("foo", existing);
			cache.Remove ("foo");
			
			Assert.DoesNotThrow (() => cache.ForceHandleExpires (item));
		}
	}
}

