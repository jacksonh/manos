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
	/// <summary>
	/// A Heirarcical dictionary. Data can live at the "root" level, or in child dictionaries.
	/// </summary>
	public class DataDictionary
	{
		private Dictionary<string,UnsafeString> dictionary;
		private List<DataDictionary> children;
		
		public DataDictionary ()
		{
			dictionary = new Dictionary<string, UnsafeString> ();
		}
		
		/// <summary>
		/// Get or set the value at the specified key.
		/// </summary>
		/// <param name="key">
		/// A <see cref="System.String"/>
		/// </param>
		public string this [string key] {
			get { return GetString (key); }
			set { Set (key, value); }
		}
		
		/// <summary>
		/// The number of child dictionaries + the number or keys in the this dictionary.
		/// </summary>
		public int Count {
			get {
				int sum = 0;
				if (children != null)
					children.Sum (c => c.Count);
				return sum + dictionary.Count;
			}
		}
		
		/// <summary>
		/// The child dictionaries.
		/// </summary>
		public IList<DataDictionary> Children {
			get {
				if (children == null)
					children = new List<DataDictionary> ();
				return children;
			}
		}
		
		/// <summary>
		/// The "unsafe" version of the value that is stored in this dictionary, or "null" if no value is stored for the specified key.
		/// </summary>
		/// <param name="key">
		/// A <see cref="System.String"/>
		/// </param>
		/// <returns>
		/// A <see cref="UnsafeString"/>
		/// </returns>
		public UnsafeString Get (string key)
		{
			UnsafeString value = null;
			
			if (dictionary.TryGetValue (key, out value))
				return value;
			
			if (children != null)
				children.Where (d => (value = d.Get (key)) != null).FirstOrDefault ();
			
			return value;
		}
		
		/// <summary>
		/// Get a "safe" string from the dictionary, or, if the key doesn't exist in the dictionary, null.
		/// </summary>
		/// <param name="key">
		/// </param>
		/// <returns>
		/// The "safe" version of the value that is stored in the dictionary.
		/// </returns>
		public string GetString (string key)
		{
			UnsafeString str = Get (key);
			
			if (str == null)
				return null;
			
			return str.SafeValue;
		}
	
		/// <summary>
		/// Remove all elements from this dictionary, and remove all references to child dictionaries.
		/// </summary>
		public void Clear ()
		{
			dictionary.Clear ();
			children = null;
		}	
		
		/// <summary>
		/// Assign a value into this dictionary with the specified key.
		/// </summary>
		/// <param name="key">
		/// A <see cref="System.String"/>
		/// </param>
		/// <param name="value">
		/// A <see cref="System.String"/>
		/// </param>
		public void Set (string key, string value)
		{
			dictionary [key] = new UnsafeString (value);
		}
		
		/// <summary>
		/// Assign a value into this dictionary with the specified key.
		/// </summary>
		/// <param name="key">
		/// A <see cref="System.String"/>
		/// </param>
		/// <param name="value">
		/// A <see cref="UnsafeString"/>
		/// </param>
		public void Set (string key, UnsafeString value)
		{
			dictionary [key] = value;
		}
	}
}

