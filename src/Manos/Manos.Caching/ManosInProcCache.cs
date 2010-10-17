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
using System.Collections.Generic;

namespace Manos.Caching
{
	public class ManosInProcCache : IManosCache
	{
		public class CacheItem {
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
		
		protected CacheItem SetInternal (string key, object value)
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
		
		protected virtual void HandleExpires (ManosApp app, object obj_item)
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

