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
using NUnit.Framework;

namespace Manos.Server.Tests
{
	[TestFixture()]
	public class RepeatBehaviorTest
	{
	
		//
		// Sticking this here so I can easily remember the order of the params
		// 
		// public static bool TryParse (string uri, out string scheme, out string path, out string query)
		//

		
		[Test]
		public void TryParse_GoodUri_ReturnsTrue ()
		{
			string dummy;
			string good_uri = "http://manos-de-mono.com/";
			
			bool res = UriParser.TryParse (good_uri, out dummy, out dummy, out dummy, out dummy);
			Assert.IsTrue (res);
		}

		[Test]
		public void TryParse_GoodUriWithPort_ReturnsTrue ()
		{
			string dummy;
			string good_uri = "http://manos-de-mono.com:8080/";
			
			bool res = UriParser.TryParse (good_uri, out dummy, out dummy, out dummy, out dummy);
			Assert.IsTrue (res);
		}

		[Test]
		public void TryParse_GoodUriWithNoTrailingSlash_ReturnsTrue ()
		{
			string dummy;
			string good_uri = "http://manos-de-mono.com:8080";
			
			bool res = UriParser.TryParse (good_uri, out dummy, out dummy, out dummy, out dummy);
			Assert.IsTrue (res);
		}

		[Test]
		public void TryParse_GoodUriWithNoTrailingSlash_SetsPathToSlash ()
		{
			string dummy;
			string path;
			string good_uri = "http://www.manos-de-mono.com:8080/";
			
			UriParser.TryParse (good_uri, out dummy, out dummy, out path, out dummy);
			Assert.AreEqual ("/", path);
		}
	}
}

