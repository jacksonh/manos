

using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;


namespace Manos.Collections
{
	public class DataDictionary
	{
		private Dictionary<string,string> dictionary;
		private List<DataDictionary> children;
		
		public DataDictionary ()
		{
			dictionary = new Dictionary<string, string> ();
		}
		
		public string this [string key] {
			get { return Get (key); }
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
		
		public string Get (string key)
		{
			string value = null;
			
			if (dictionary.TryGetValue (key, out value))
				return value;
			
			if (children != null)
				children.Where (d => (value = d.Get (key)) != null).FirstOrDefault ();
			
			return value;
		}
			
		public void Set (string key, string value)
		{
			dictionary [key] = value;
		}
	}
}

