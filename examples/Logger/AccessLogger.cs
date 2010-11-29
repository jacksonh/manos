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
using System.IO;
using System.Text;

using Manos;
using Manos.Http;

namespace Manos.Util {

	public class AccessLogger : ManosPipe {

		private string path;
		private FileStream stream;

		public AccessLogger (string path)
		{
			if (path == null)
				throw new ArgumentNullException ("path");

			this.path = path;
			CreateStream ();
		}

		public override void OnPostProcessRequest (ManosApp app, IHttpTransaction transaction)
		{
			// LogFormat "%h %l %u %t "%r" %>s %b "%{Referer}i" "%{User-Agent}i"" combined
			// %h - Remote host       -- DONT HAVE
			// %l - Remote log name   -- DONT HAVE
			// %u - Remote user       -- DONT HAVE
			// %t - Date+Time
			// %r - Request path
			// %s - Status Code
			// %b - Bytes sent
			// %Referer -
			// %User Agent -

			string line = String.Format ("- - - [{0}] \"{1}\" {2} {3} - -\n",
					DateTime.Now.ToString ("dd/MMM/yyyy:HH:mm:ss K"),
					transaction.Request.LocalPath,
					transaction.Response.StatusCode,
					transaction.Response.Headers.ContentLength);

			byte [] data = Encoding.Default.GetBytes (line);
			stream.BeginWrite (data, 0, data.Length, null, null);
		}

		private void CreateStream ()
		{
			string dir = Path.GetDirectoryName (path);
			// If it exists this does nothing.
			if (!String.IsNullOrEmpty (dir))
				Directory.CreateDirectory (dir);

			stream = new FileStream (path, FileMode.Append, FileAccess.Write, FileShare.Read, 8, true);
		}
	}
}

