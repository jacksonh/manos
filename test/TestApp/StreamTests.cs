

using System;


namespace Manos.Tests {

	  public class StreamTests {

	  	  public StreamTests (TestRunner tr)
	  	  {
			TestRunner = tr;
	  	  }

	  	  public TestRunner TestRunner {
	  	  	  get;
		  	  private set;
	          }

	  	  public void Run ()
	  	  {
			TestRunner.RunTest ("/StreamTests/EchoLocalPath", "/StreamTests/EchoLocalPath");

			TestRunner.RunTest ("/StreamTests/EchoInt?the_int=45", "/StreamTests/EchoInt?the_int=52", "45");
			TestRunner.RunTest ("/StreamTests/EchoString?the_string=foobar", 
					   	"/StreamTests/EchoString?the_string=iamtheloaduri", "foobar");

			TestRunner.RunTest ("/StreamTests/SendFile?name=TestRunner.exe", null);
	  	  }
	}
}

