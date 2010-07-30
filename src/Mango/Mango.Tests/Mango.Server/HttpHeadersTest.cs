
using System;
using System.IO;

using NUnit.Framework;



namespace Mango.Server.Tests
{
	[TestFixture()]
	public class HttpHeadersTest
	{

		[Test()]
		public void TestMultilineParse ()
		{
			//
			// multiline values are acceptable if the next 
			// line starts with spaces
			//
			string header = @"HeaderName: Some multiline
  								value";
		
			HttpHeaders headers = new HttpHeaders ();
			
			headers.Parse (new StringReader (header));
			
			Assert.AreEqual ("some multiline value", headers ["HeaderName"], "a1");
		}
	}
}
