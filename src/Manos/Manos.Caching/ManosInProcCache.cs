

using System;
using System.Collections.Generic;

namespace Manos.Caching
{
	public class ManosInProcCache : IManosCache
	{
		private class CacheItem {
			public bool IsRemoved;
			public string Key;
			public object Item;
			
			public CacheItem (string key, object item)
			{
				Key = key;
				Item = item;
			}
		}
		
		private Dictionary<string,CacheItem> items = new Dictionary<string, CacheItem> ();
		
		private object lock_obj = new object ();
		
		public object Get (string key)
		{
			CacheItem item;
			
			lock (lock_obj) {
				if (!items.TryGetValue (key, out item))
					return null;
			}
			
			return item.Item;
		}

		public void Set (string key, object value)
		{
			SetInternal (key, value);
		}

		public void Set (string key, object value, TimeSpan expires)
		{
			CacheItem item = SetInternal (key, value);
			
			AppHost.AddTimeout (expires, RepeatBehavior.Single, item, HandleExpires);               
		}

		private CacheItem SetInternal (string key, object value)
		{
			CacheItem item = null;
			lock (lock_obj) {
				if (items.TryGetValue (key, out item))
					item.IsRemoved = true;
			
				item = new CacheItem (key, value);
				items [key] = item;
			}
			
			return item;
		}
		
		public void Remove (string key)
		{
			lock (lock_obj) {
				CacheItem item;
				
				if (!items.TryGetValue (key, out item))
					return;
				
				item.IsRemoved = true;
				items.Remove (key);
			}
		}
		
		public object this[string key] {
			get {
				return Get (key);
			}
			set {
				Set (key, value);
			}
		}
		
		public void Clear ()
		{
			lock (lock_obj) {
				items.Clear ();	
			}
		}
		
		private void HandleExpires (ManosApp app, object obj_item)
		{
			CacheItem item = (CacheItem) obj_item;
			if (item.IsRemoved)
				return;
			
			item.IsRemoved = true;
			lock (lock_obj) {
				items.Remove (item.Key);
			}
		}
	}
}

