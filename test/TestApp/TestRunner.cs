

using System;
using System.IO;
using System.Net;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;


namespace Manos.Tests {

	public class TestRunner {

		private bool LoadTest = true;
		private Process LoadProcess = null;

		private string MANOS_SERVER = "http://localhost:8080";

		public static int Main (string [] args)
		{
			var r = new TestRunner ();

			return r.Run ();
		}

		 
		public TestRunner ()
		{
		}

		public int Run ()
		{
			try {
				RunStreamTests ();
			} catch (Exception e) {
				Console.WriteLine (e);
				return -1;
			}

			return 0;
		}		 

		public void RunStreamTests ()
		{
			var st = new StreamTests (this);

			st.Run ();
		}

		public void RunTest (string uri, object expected)
		{
			RunTestInternal (uri, uri, "GET", null, expected);
		}

		public void RunTest (string uri, string load_uri, object expected)
		{
			RunTestInternal (uri, load_uri, "GET", null, expected);
		}

		public void RunUploadTest (string uri, string file, object expected)
		{
			Console.WriteLine ("RUNNING UPLOAD {0}.....", uri);

			WebClient client = new WebClient ();
			byte [] result = client.UploadFile (new Uri (MANOS_SERVER + uri), file);

			byte [] data_expected = expected as byte [];
			if (data_expected != null) {
				if (result.Length != data_expected.Length)
					throw new Exception (String.Format ("Upload test failed. Data lengths differed ({0} vs {1})", result.Length, data_expected.Length));
			}

			Console.WriteLine ("PASSED.");
		}

		public void RunTestInternal (string uri, string load_uri, string method, Dictionary<string,string> data, object expected, bool upload=false)
		{
			Console.Write ("RUNNING {0}...", uri);

			uri = MANOS_SERVER + uri;

			if (LoadTest) {
				BeginLoad (uri);
			}

			var request = (HttpWebRequest) WebRequest.Create (uri);

			request.Method = method;

			HttpWebResponse response = (HttpWebResponse) request.GetResponse ();
		        if (response.StatusCode != HttpStatusCode.OK)
				throw new Exception ("Bad status code for uri " + uri + " " + response.StatusCode + ".");

			var stream = response.GetResponseStream ();

			string expected_str = expected as string;
			if (expected_str != null) {
				var reader = new StreamReader (stream);
				string result = reader.ReadToEnd ();

				if (result != expected_str)
					throw new Exception (String.Format ("Expected '{0}' for uri {1} got '{2}'", expected, uri, result));
			}

			byte [] expected_data = expected as byte [];
			if (expected_data != null) {
				for (int i = 0; i < expected_data.Length; i++) {
					byte b = (byte) stream.ReadByte ();
					if (b != expected_data [i])
						throw new Exception (String.Format ("Data does not match at index {0}.", i));
				}
				if (stream.ReadByte () != -1)
					throw new Exception ("Data does not match, extra data at end of stream.");
			}

			if (LoadTest) {
				WaitForLoad ();
			}

			Console.WriteLine ("PASSED");
		}

		private void BeginLoad (string uri)
		{
			LoadProcess = Process.Start ("ab", String.Format ("-c 50 -n 5000 -d -k {0}", uri));	
		}

		private void WaitForLoad ()
		{
			try {
				LoadProcess.WaitForExit ();
				LoadProcess = null;
			} catch (Exception e) {
				Console.Error.WriteLine (e);
			}
		}
	}
	  
}

