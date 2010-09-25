using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Manos.Server
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
	}
}

