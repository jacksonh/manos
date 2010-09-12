
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;



namespace Manos.Server {

	public class RequestDataDictionary : IDictionary<string,string> {

		IDictionary<string,string> dictionary = new Dictionary<string,string> ();
		List<NameValueCollection> data_collections = new List<NameValueCollection> ();
		
		public int Count {
			get {
				int count = data_collections.Sum (c => c.Count);
				
				return count + dictionary.Count;
			}
		}
		
		public bool IsReadOnly {
			get { return false; }
		}
		
		public void AddCollection (NameValueCollection col)
		{
			data_collections.Add (col);
		}
		
		public void Add (KeyValuePair<string,string> item)
		{
			dictionary.Add (item);	
		}
		
		public void Add (string key, string value)
		{
			dictionary.Add (key, value);
		}

		public void Clear ()
		{
			dictionary.Clear ();
			data_collections.ForEach (c => c.Clear ());
		}
		
		public bool Contains (KeyValuePair<string,string> item)
		{
			if (dictionary.Contains (item))
				return true;
			
			foreach (NameValueCollection nvc in data_collections) {
				string value = nvc.Get (item.Key);
				if (value == null)
					continue;
				if (ItemStringContainsValue (value, item.Value))
				    return true;
			}
			
			return false;
		}
		
		private bool ItemStringContainsValue (string item, string value)
		{
			if (item == value)
				return true;
			
			
			int start = 0;
			int idx = item.IndexOf (value);
			
			while (idx >= 0) {
				if (idx != 0 && item [idx - 1] != ',') {
					start = idx + 1;
					idx = item.IndexOf (value, start);
					continue;
				}
				
				int end = idx + value.Length;
				if (end > item.Length)
					return false;
				
				if (end == item.Length || item [end] == ',')
					return true;
				
				start = idx + 1;
				idx = item.IndexOf (value, start);
			}
			
			return false;
		}
		
		public bool ContainsKey (string key)
		{
			if (dictionary.ContainsKey (key))
				return true;

			return data_collections.Where (c => c.Get (key) != null).FirstOrDefault () != null;
		}

		public bool Remove (string key)
		{
			bool res = false;
			
			bool r = dictionary.Remove (key);
			if (r)
				res = r;
			
			foreach (NameValueCollection nvc in data_collections) {
				r = nvc.Get (key) != null;
				nvc.Remove (key);
				
				if (r)
					res = true;
			}
			
			return res;
		}

		public bool Remove (KeyValuePair<string,string> item)
		{
			throw new NotImplementedException ();	
		}
		
		public bool TryGetValue (string key, out string value)
		{
			if (dictionary.TryGetValue (key, out value))
				return true;
			
			return false;
		}

		public string this[string key] {
			get {
				throw new NotImplementedException ();
			}
			set {
				throw new NotImplementedException ();
			}
		}

		public ICollection<string> Keys {
			get {
				throw new NotImplementedException ();
			}
		}

		public ICollection<string> Values {
			get {
				throw new NotImplementedException ();
			}
		}
		
		IEnumerator IEnumerable.GetEnumerator ()
		{
			throw new NotImplementedException ();
		}
		
		public IEnumerator<KeyValuePair<string,string>> GetEnumerator ()
		{
			throw new NotImplementedException ();	
		}
		
		public void CopyTo (KeyValuePair<string,string> [] array, int index)
		{
			throw new NotImplementedException ();
		}
		
	}
}


