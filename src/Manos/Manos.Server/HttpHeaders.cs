
using System;
using System.IO;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Collections;
using System.Collections.Generic;

using Mono.Unix.Native;


namespace Mango.Server {

	public class HttpHeaders {

		public static readonly string CONTENT_LENGTH_KEY = "Content-Length";
		
		private long? content_length;
		private Dictionary<string,string> items = new Dictionary<string,string> ();

		public HttpHeaders ()
		{
		}

		public long? ContentLength {
			get { return content_length; }
			set {
				if (value < 0)
					throw new ArgumentException ("value");
				
				content_length = value;
				if (value == null) {
					items.Remove (CONTENT_LENGTH_KEY);
					return;
				}
				items [CONTENT_LENGTH_KEY] = value.ToString ();
			}
		}

		public string this [string name] {
			get {
				if (name == null)
					throw new ArgumentNullException ("name");
				return items [NormalizeName (name)];
			}
		}

		public int Count {
			get { return items.Count; }	
		}
		
		public Dictionary<string,string>.KeyCollection Keys {
			get {
				return items.Keys;
			}
		}

		public bool TryGetValue (string key, out string value)
		{
			return items.TryGetValue (NormalizeName (key), out value);
		}

		public void Parse (TextReader reader)
		{
			string line = reader.ReadLine ();
			while (line != null) {
				int line_end = line.Length - 1;
				
				if (String.IsNullOrEmpty (line))
					return;
				
				if (Char.IsWhiteSpace (line [0]))
					throw new HttpException ("Malformed HTTP header. Found whitespace before data.");
				
				while (Char.IsWhiteSpace (line [line_end])) {
					line_end--;
					if (line_end == 0)
						throw new HttpException ("Malformed HTTP header. No data found.");
				}

				int colon = line.IndexOf (':');
				if (colon <= 0) 
					throw new HttpException ("Malformed HTTP header. No colon found.");
				if (colon >= line_end)
					throw new HttpException ("Malformed HTTP header. No value found.");
				string value = line.Substring (colon + 1, line_end - colon).TrimStart ();
				if (value.Length == 0)
					throw new HttpException ("Malformed HTTP header. No Value found.");
				
				string key = line.Substring (0, colon);
				
				//
				// If the next line starts with whitespace its part of the current value
				//
				line = reader.ReadLine ();
				while (line != null && line.Length > 0 && Char.IsWhiteSpace (line [0])) {
					value += " " + line.Trim ();
					line = reader.ReadLine ();
				}
				
				SetHeader (key, value);
			}
		}

		public byte [] Write (Encoding encoding)
		{
			StringBuilder builder = new StringBuilder ();
			foreach (var header in items) {
				builder.Append (header.Key);
				builder.Append (": ");
				builder.Append (header.Value);
				builder.Append ("\r\n");
			}
			builder.Append ("\r\n");

			return encoding.GetBytes (builder.ToString ());
		}

		public void SetHeader (string name, string value)
		{
			if (name == null)
				throw new ArgumentNullException ("name");
			
			name = NormalizeName (name);

			if (!IsValidHeaderName (name))
				throw new ArgumentException (String.Format ("Invalid header '{0}'.", name));
			
			if (name == CONTENT_LENGTH_KEY) {
				SetContentLength (value);
			}

			if (value == null) {
				items.Remove (name);
				return;
			}
				
			items [name] = value;
		}

		public static string NormalizeName (string name)
		{
			if (String.IsNullOrEmpty (name))
				throw new ArgumentException ("name", "name must be a non-null non-empty string");
			
			StringBuilder res = new StringBuilder (name);

			res [0] = Char.ToUpper (name [0]);
			
			char p = name [0];
			for (int i = 1; i < res.Length; i++) {
				char c = res [i];
				if (p == '-' && Char.IsLower (c))
					res [i] = Char.ToUpper (c);
				if (p != '-' && Char.IsUpper (c))
					res [i] = Char.ToLower (c);
				p = c;
			}

			return res.ToString ();
		}

		public bool IsValidHeaderName (string name)
		{
			// TODO: What more can I do here?
			if (name.Length == 0)
				return false;
			return true;
		}
		
		public void SetContentLength (string value)
		{
			if (value == null) {
				items.Remove (CONTENT_LENGTH_KEY);
				content_length = null;
				return;
			}
			
			int cl;
			if (!Int32.TryParse (value, out cl))
				throw new ArgumentException ("Malformed HTTP Header, invalid Content-Length value.", "value");
			if (cl < 0)
				throw new ArgumentException ("Content-Length must be a positive integer.", "value");
			ContentLength = cl;
		}

		/// from mono's System.Web.Util/HttpEncoder.cs
		private static string EncodeHeaderString (string input)
		{
			StringBuilder sb = null;
			char ch;
			
			for (int i = 0; i < input.Length; i++) {
				ch = input [i];

				if ((ch < 32 && ch != 9) || ch == 127)
					StringBuilderAppend (String.Format ("%{0:x2}", (int)ch), ref sb);
			}

			if (sb != null)
				return sb.ToString ();

			return input;
		}

		private static void StringBuilderAppend (string s, ref StringBuilder sb)
		{
			if (sb == null)
				sb = new StringBuilder (s);
			else
				sb.Append (s);
		}
	}
}



