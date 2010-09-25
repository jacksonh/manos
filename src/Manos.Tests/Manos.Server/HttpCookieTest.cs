using System;
using NUnit.Framework;

using Manos.ShouldExt;

namespace Manos.Server.Tests
{
	[TestFixture()]
	public class HttpCookieTest
	{
		[Test]
		public void Ctor_NullName_Throws ()
		{
			Should.Throw<ArgumentNullException> (() => new HttpCookie (null, "value"));
		}
		
		[Test]
		public void Ctor_NullValue_Throws ()
		{
			Should.Throw<ArgumentNullException> (() => new HttpCookie ("name", null));
		}
		
		[Test]
		public void Ctor_ValidValues_AddsPair ()
		{
			var name = "foobar";
			var value = "the value";
			
			var cookie = new HttpCookie (name, value);
			Assert.AreEqual (1, cookie.Values.Count);
		}
		
		[Test]
		public void Ctor_ValidValues_AddsName ()
		{
			var name = "foobar";
			var value = "the value";
			
			var cookie = new HttpCookie (name, value);
			
			bool contains = cookie.Values.ContainsKey ("foobar");
			Assert.IsTrue (contains);
		}
		
		[Test]
		public void Ctor_ValidValues_SetsValue ()
		{
			var name = "foobar";
			var value = "the value";
			
			var cookie = new HttpCookie (name, value);
			Assert.AreEqual ("the value", cookie.Values [name]);
		}
		
		[Test]
		public void ToHeaderString_SingleValueNoWhiteSpace_Formatting()
		{
			var name = "foobar";
			var value = "value";
			var cookie = new HttpCookie (name, value);
			
			var header = cookie.ToHeaderString ();
			Assert.AreEqual ("Set-Cookie: foobar=value", header);
		}
		
		[Test]
		public void ToHeaderString_SingleValueWithWhiteSpace_Formatting()
		{
			var name = "foobar";
			var value = "the value";
			var cookie = new HttpCookie (name, value);
			
			var header = cookie.ToHeaderString ();
			Assert.AreEqual ("Set-Cookie: foobar=\"the value\"", header);
		}
		
		[Test]
		public void ToHeaderString_SingleValueWithSemiColon_Formatting()
		{
			var name = "foobar";
			var value = "value;";
			var cookie = new HttpCookie (name, value);
			
			var header = cookie.ToHeaderString ();
			Assert.AreEqual ("Set-Cookie: foobar=\"value;\"", header);
		}
		
		[Test]
		public void ToHeaderString_SingleValueWithComma_Formatting()
		{
			var name = "foobar";
			var value = "val,ue";
			var cookie = new HttpCookie (name, value);
			
			var header = cookie.ToHeaderString ();
			Assert.AreEqual ("Set-Cookie: foobar=\"val,ue\"", header);
		}
		
		[Test]
		public void ToHeaderString_NameWithComma_Formatting()
		{
			var name = "foo,bar";
			var value = "value";
			var cookie = new HttpCookie (name, value);
			
			var header = cookie.ToHeaderString ();
			Assert.AreEqual ("Set-Cookie: \"foo,bar\"=value", header);
		}
		
		[Test]
		public void ToHeaderString_NameValueAndDomain_Formatting ()
		{
			var name = "foobar";
			var value = "value";
			var cookie = new HttpCookie (name, value);
			
			cookie.Domain = "http://manos-de-mono.com";
			
			var header = cookie.ToHeaderString ();
			Assert.AreEqual ("Set-Cookie: foobar=value; domain=http://manos-de-mono.com", header);
		}
		
		[Test]
		public void ToHeaderString_NameValueAndPath_Formatting ()
		{
			var name = "foobar";
			var value = "value";
			var cookie = new HttpCookie (name, value);
			
			cookie.Path = "/foobar/";
			
			var header = cookie.ToHeaderString ();
			Assert.AreEqual ("Set-Cookie: foobar=value; path=/foobar/", header);
		}
		
		[Test]
		public void ToHeaderString_NameValueAndExpires_Formatting ()
		{
			var name = "foobar";
			var value = "value";
			var cookie = new HttpCookie (name, value);
			
			cookie.Expires = new DateTime (2010, 8, 4, 10, 11, 12, DateTimeKind.Utc);
			
			var header = cookie.ToHeaderString ();
			Assert.AreEqual ("Set-Cookie: foobar=value; expires=Wed, 04 Aug 2010 10:11:12 GMT", header);
		}
		
		[Test]
		public void ToHeaderString_NameValueAndSecure_Formatting ()
		{
			var name = "foobar";
			var value = "value";
			var cookie = new HttpCookie (name, value);
			
			cookie.Secure = true;
			
			var header = cookie.ToHeaderString ();
			Assert.AreEqual ("Set-Cookie: foobar=value; secure", header);
		}
		
		[Test]
		public void ToHeaderString_NameValueAndHttpOnly_Formatting ()
		{
			var name = "foobar";
			var value = "value";
			var cookie = new HttpCookie (name, value);
			
			cookie.HttpOnly = true;
			
			var header = cookie.ToHeaderString ();
			Assert.AreEqual ("Set-Cookie: foobar=value; HttpOnly", header);
		}
	}
}

