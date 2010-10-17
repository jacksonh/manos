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


