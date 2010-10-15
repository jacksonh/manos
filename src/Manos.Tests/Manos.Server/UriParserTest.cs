

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

