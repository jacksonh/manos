
using System;
using NUnit.Framework;

using Mango.Server;


namespace Mango.Server.Tests
{


	[TestFixture()]
	public class HttpHeadersTest
	{

		[Test()]
		public void TestSetHeadersNull ()
		{
			HttpHeaders headers = new HttpHeaders ();
			
			Assert.Throws<ArgumentNullException> (() => headers.SetHeader (null, String.Empty), "a1");
			Assert.Throws<ArgumentNullException> (() => headers.SetHeader (String.Empty, null), "a2");
		}
		
		[Test()]
		public void TestSetHeadersInvalid ()
		{
			HttpHeaders headers = new HttpHeaders ();
			
			Assert.Throws<ArgumentException> (() => headers.SetHeader (String.Empty, "foobar"), "a1");
		}
		
		[Test]
		public void TestSetInvalidContentLength ()
		{
			HttpHeaders headers = new HttpHeaders ();
			
			Assert.Throws<ArgumentException> (() => headers.SetHeader ("Content-Length", "-1"), "a1");
			Assert.Throws<ArgumentException> (() => headers.SetHeader ("Content-Length", "foobar"), "a2");
			Assert.Throws<ArgumentException> (() => headers.SetHeader ("Content-Length", "49.5"), "a3");
		}
		
		[Test]
		public void EnsureSet ()
		{
			HttpHeaders headers = new HttpHeaders ();
			
			headers.SetHeader ("foobar", "baz");
			
			Assert.AreEqual ("baz", headers ["foobar"]);
		}
	}
}
