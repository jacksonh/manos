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

			byte [] data = File.ReadAllBytes ("TestRunner.exe");
			TestRunner.RunTest ("/StreamTests/SendFile?name=TestRunner.exe", data);

			TestRunner.RunUploadTest ("/StreamTests/UploadFile?name=TestRunner.exe", "TestRunner.exe", data);
		}
	}
}

