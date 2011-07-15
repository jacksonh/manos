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

		public void Get (string key, CacheItemCallback callback)
		{
			CacheItem item;
			object res = null;

			if (key == null)
				throw new ArgumentNullException ("key");
			if (callback == null)
				throw new ArgumentNullException ("callback");

			if (items.TryGetValue (key, out item))
				res = item.Item;

			callback (key, res);
		}

		public void Set (string key, object value)
		{
			SetInternal (key, value);
		}

		public void Set (string key, object value, TimeSpan expires)
		{
			Set (key, value, expires, delegate {
			});
		}

		public void Set (string key, object value, CacheItemCallback callback)
		{
			SetInternal (key, value);

			if (callback != null)
				callback (key, value);
		}

		public void Set (string key, object value, TimeSpan expires, CacheItemCallback callback)
		{
			CacheItem item = SetInternal (key, value);
			
			AppHost.AddTimeout (expires, RepeatBehavior.Single, item, HandleExpires);

			if (callback != null)
				callback (key, value);
		}

		public void Remove (string key)
		{
			Remove (key, null);
		}

		public void Remove (string key, CacheItemCallback callback)
		{
			CacheItem item;
			object value = null;
				
			if (items.TryGetValue (key, out item)) {
				item.IsRemoved = true;
				items.Remove (key);
				value = item.Item;
			}

			if (callback != null)
				callback (key, value);
		}
		
		public void Clear ()
		{
			items.Clear ();	
		}

		public void Clear (CacheOpCallback callback)
		{
			items.Clear ();

			if (callback != null)
				callback ();
		}
		
		protected CacheItem SetInternal (string key, object value)
		{
			CacheItem item = null;
			if (items.TryGetValue (key, out item))
				item.IsRemoved = true;
			
			item = new CacheItem (key, value);
			items [key] = item;

			return item;
		}
		
		protected virtual void HandleExpires (ManosApp app, object obj_item)
		{
			CacheItem item = (CacheItem) obj_item;
			if (item.IsRemoved)
				return;
			
			item.IsRemoved = true;
			items.Remove (item.Key);
		}
	}
}

