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
	
		public void Clear ()
		{
			dictionary.Clear ();
			children = null;
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

