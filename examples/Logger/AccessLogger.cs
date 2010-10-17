
using System;
using System.IO;
using System.Text;

using Manos;
using Manos.Server;

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

