
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

		private Dictionary<string,string> items = new Dictionary<string,string> ();

		public HttpHeaders ()
		{

		}

		public int? ContentLength {
			get;
			private set;
		}

		public void Parse (TextReader reader)
		{
			string line;
			while ((line = reader.ReadLine ()) != null) {
				int colon = line.IndexOf (':');
				if (colon <= 0)
					continue;
				SetHeader (line.Substring (0, colon), line.Substring (colon + 1, line.Length - colon - 1).Trim ());
			}
		}

		public void SetHeader (string name, string value)
		{
			Console.WriteLine ("setting header: {0} '{1}'", name, value);

			name = NormalizeName (name);

			switch (name) {
			case "Content-Length":
				SetContentLength (value);				
				break;
			}

			items [name] = value;
		}

		private string NormalizeName (string name)
		{
			return name;
		}

		private void SetContentLength (string value)
		{
			if (value == null) {
				ContentLength = null;
				return;
			}

			int cl;
			if (!Int32.TryParse (value, out cl)) {
				throw new Exception ("Malformed HTTP Header, invalid Content-Length value.");
			}
			ContentLength = cl;
		}
	}
}



