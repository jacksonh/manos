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

namespace Manos.Templates.Tests
{
	[TestFixture()]
	public class CodegenTest
	{

		[Test()]
		public void TestCase ()
		{
		}
		
		[Test]
		public void TestFullTypeNameForPath ()
		{
			string app_name = "FooBar";
			string path;
			string name;
			
			path = "Tests.html";
			name = Page.FullTypeNameForPath (app_name, path);
			Assert.AreEqual ("FooBar.Templates.TestsHtml", name, "a1");
			
			path = "Manos.Tests.Tests.html";
			name = Page.FullTypeNameForPath (app_name, path);
			Assert.AreEqual ("FooBar.Templates.Manos.Tests.TestsHtml", name, "a2");
			
			path = "manos.tests.html";
			name = Page.FullTypeNameForPath (app_name, path);
			Assert.AreEqual ("FooBar.Templates.Manos.TestsHtml", name, "a3");
			
			path = "Manos/Tests.html";
			name = Page.FullTypeNameForPath (app_name, path);
			Assert.AreEqual ("FooBar.Templates.Manos.TestsHtml", name, "a4");
			
			path = "Manos.Tests/Tests.HTML";
			name = Page.FullTypeNameForPath (app_name, path);
			Assert.AreEqual ("FooBar.Templates.Manos.Tests.TestsHtml", name, "a5");
			
			path = "manos/tests.hTMl";
			name = Page.FullTypeNameForPath (app_name, path);
			Assert.AreEqual ("FooBar.Templates.Manos.TestsHtml", name, "a6");
		}
		
		[Test]
		public void TestTypeNameForPathWithDoubleDots ()
		{
			string app_name = "FooBar";
			string path;
			
			path = "Manos/tests..html";
			Assert.Throws<ArgumentException> (() => Page.FullTypeNameForPath (app_name, path));
		}
	}
}
