

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

		 public void RunTest (string uri, string expected)
		 {
			RunTestInternal (uri, uri, "GET", null, expected);
		 }

		 public void RunTest (string uri, string load_uri, string expected)
		 {
			RunTestInternal (uri, load_uri, "GET", null, expected);
		 }

		 public void RunTestInternal (string uri, string load_uri, string method, Dictionary<string,string> data, string expected)
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
    			 var reader = new StreamReader (stream);

			 string result = reader.ReadToEnd ();

			 if (result != expected)
			    throw new Exception (String.Format ("Expected '{0}' for uri {1} got '{2}'", expected, uri, result));

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

