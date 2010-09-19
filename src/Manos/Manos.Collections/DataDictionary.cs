

using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace Manos.Collections
{
	public class DataDictionary
	{
		private Dictionary<string,UnsafeString> dictionary;
		private List<DataDictionary> children;
		
		public DataDictionary ()
		{
			dictionary = new Dictionary<string, UnsafeString> ();
		}
		
		public string this [string key] {
			get { return GetString (key); }
			set { Set (key, value); }
		}
		
		public int Count {
			get {
				int sum = 0;
				if (children != null)
					children.Sum (c => c.Count);
				return sum + dictionary.Count;
			}
		}
		
		public IList<DataDictionary> Children {
			get {
				if (children == null)
					children = new List<DataDictionary> ();
				return children;
			}
		}
		
		public UnsafeString Get (string key)
		{
			UnsafeString value = null;
			
			if (dictionary.TryGetValue (key, out value))
				return value;
			
			if (children != null)
				children.Where (d => (value = d.Get (key)) != null).FirstOrDefault ();
			
			return value;
		}
		
		public string GetString (string key)
		{
			UnsafeString str = Get (key);
			
			if (str == null)
				return null;
			
			return str.SafeValue;
		}
		
		public void Set (string key, string value)
		{
			dictionary [key] = new UnsafeString (value);
		}
		
		public void Set (string key, UnsafeString value)
		{
			dictionary [key] = value;
		}
	}
}

