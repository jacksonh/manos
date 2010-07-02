
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

		private long? content_length;
		private Dictionary<string,string> items = new Dictionary<string,string> ();

		public HttpHeaders ()
		{
		}

		public long? ContentLength {
			get { return content_length; }
			set {
				content_length = value;
				items ["Content-Length"] = value.ToString ();
			}
		}

		public string this [string name] {
			get {
				if (name == null)
					throw new ArgumentNullException ("name");
				return items [name];
			}
		}

		public void Parse (TextReader reader)
		{
			string line;
			while ((line = reader.ReadLine ()) != null) {
				int colon = line.IndexOf (':');
				if (colon <= 0) {
					Console.Error.WriteLine ("Invalid HTTP header: {0}", line);
					continue;
				}
				SetHeader (line.Substring (0, colon), line.Substring (colon + 1, line.Length - colon - 1).Trim ());
			}
		}

		public void Write (Stream stream, Encoding encoding)
		{
			// fuck me, so this is going to encode to a buffer,
			// then write to a buffer, which will byte convert
			// the string to a buffer, write to the stream buffer,
			// which then will write to the IO buffer, and
			// get sent across the socket.  I would like to
			// eliminate the first 36 buffers from this process
			
			StringBuilder builder = new StringBuilder ();
			foreach (var header in items) {
				builder.AppendFormat ("{0}: {1}", header.Key, header.Value);
			}

			Console.WriteLine ("sent headers:  {0}", builder.ToString ());

			byte [] data = encoding.GetBytes (builder.ToString ());
			stream.Write (data, 0, data.Length);
		}

		public void SetHeader (string name, string value)
		{
			Console.WriteLine ("setting header: {0} '{1}'", name, value);

			if (name == null)
				throw new ArgumentNullException ("name");
			if (value == null)
				throw new ArgumentNullException ("value");
			
			name = NormalizeName (name);

			if (!IsValidHeaderName (name))
				throw new ArgumentException (String.Format ("Invalid header '{0}'.", name));
			
			switch (name) {
			case "Content-Length":
				SetContentLength (value);				
				break;
			}

			items [name] = value;
		}

		internal string NormalizeName (string name)
		{
			StringBuilder res = new StringBuilder (name);

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

		internal bool IsValidHeaderName (string name)
		{
			// TODO: What more can I do here?
			if (name.Length == 0)
				return false;
			return true;
		}
		
		private void SetContentLength (string value)
		{
			if (value == null)
				throw new ArgumentNullException ("value");
			
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



