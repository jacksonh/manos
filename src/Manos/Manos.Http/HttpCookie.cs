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
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Manos.Collections;

namespace Manos.Http
{
	public class HttpCookie
	{
		private Dictionary<string,string> values;
		
		public HttpCookie (string name, string value)
		{
			if (name == null)
				throw new ArgumentNullException ("name");
			if (value == null)
				throw new ArgumentNullException ("value");
			
			values = new Dictionary<string, string> ();
			values.Add (name, value);
		}
		
		public Dictionary<string,string> Values {
			get { return values; }
		}
		
		public string Domain {
			get;
			set;
		}
		
		public string Path { 
			get; 
			set; 
		}
		
		public DateTime Expires { 
			get; 
			set; 
		}
		
		public bool Secure { 
			get; 
			set; 
		}
		
		public bool HttpOnly {
			get;
			set;
		}
		
		public void WriteHeader (IHttpResponse response)
		{
			// TODO: Eventually I'd like to make this write as it goes instead of
			// buffering into a stream, but I need to create the ResponseStream
			// type first.
			response.Write (ToHeaderString ());
		}
		
		public string ToHeaderString ()
		{
			StringBuilder builder = new StringBuilder ();
			
			builder.Append ("Set-Cookie: ");
			
			bool first = true;
			foreach (KeyValuePair<string,string> kv in Values) {
				if (!first)
					builder.Append ("; ");
				first = false;
				builder.Append (EscapeStr (kv.Key));
				builder.Append ("=");
				builder.Append (EscapeStr (kv.Value));
			}
			
			if (Domain != null) {
				builder.Append ("; domain=");
				builder.Append (EscapeStr (Domain));
			}
			
			if (Path != null) {
				builder.Append ("; path=");
				builder.Append (EscapeStr (Path));
			}
			
			if (Expires != DateTime.MinValue) {
				builder.Append ("; expires=");
				builder.Append (Expires.ToUniversalTime ().ToString ("r"));
			}
			
			if (Secure) {
				builder.Append ("; secure");	
			}
			
			if (HttpOnly) {
				builder.Append ("; HttpOnly");	
			}
			
			builder.Append ("\r\n");
			
			return builder.ToString ();
		}
		
		public string ToJsString ()
		{
			return null;	
		}
		
		private static string escape_chars = "';,=";
		
		public static string EscapeStr (string key)
		{
			// TODO: UrlEncode
			
			bool do_escape = false;
			
			for (int i = 0; i < key.Length; i++) {
				if (escape_chars.IndexOf (key [i]) > 0) {
					do_escape = true;
					break;
				}
				if (Char.IsWhiteSpace (key, i)) {
					do_escape = true;
					break;
				}	
			}
			
			if (do_escape)
				return String.Concat ('\"', key, '\"');
			return key;
		}
		
		public static DataDictionary FromHeader (string header)
		{
			int eq_idx = -1;
			int key_idx = 0;
			DataDictionary dict = new DataDictionary ();

			for (int i = 0; i < header.Length; i++) {
				if (header [i] == ';') {
					if (eq_idx == -1)
						continue;
					string key = header.Substring (key_idx, eq_idx - key_idx);
					string value = header.Substring (eq_idx + 1, i - eq_idx - 1);
					
					dict.Set (key.Trim (), value.Trim ());
					
					key_idx = i + 1;
					eq_idx = -1;
					continue;
				}
				
				if (header [i] == '=')
					eq_idx = i;
			}
			
			if (eq_idx != -1) {
				string key = header.Substring (key_idx, eq_idx - key_idx);
				string value = header.Substring (eq_idx + 1);
					
				dict.Set (key.Trim (), value.Trim ());
			}
			
			return dict;
		}	
	}
}

