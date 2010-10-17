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
using System.Collections;
using System.Collections.Specialized;

using NUnit.Framework;
using Manos.ShouldExt;
using Manos.Collections;

namespace Manos.Routing.Tests
{
	[TestFixture()]
	public class SimpleMatchOperationTest
	{
		[Test]
		public void Ctor_NullPattern_Throws ()
		{
			Should.Throw<ArgumentNullException> (() => new SimpleMatchOperation (null));
		}
		
		[Test]
		public void Ctor_ValidPattern_SetsPatternProperty ()
		{
			var simple = new SimpleMatchOperation ("{foobar}");
			
			Assert.AreEqual ("{foobar}", simple.Pattern);
		}
		
		[Test]
		public void PatternSetter_NullValue_Throws ()
		{
			var simple = new SimpleMatchOperation ("{foobar}");
			
			Should.Throw<ArgumentNullException> (() => simple.Pattern = null);
		}
		
		[Test]
		public void PatternSetter_ValidValue_SetsPatternProperty ()
		{
			var simple = new SimpleMatchOperation ("{foobar}");
			
			simple.Pattern = "{baz}";
			Assert.AreEqual ("{baz}", simple.Pattern);
		}
		
		[Test]
		public void IsMatch_SinglePatternInMiddle_ReturnsTrue ()
		{
			var simple = new SimpleMatchOperation ("/foo/{bar}/baz/");
			
			int end;
			DataDictionary data = new DataDictionary ();
			bool is_match = simple.IsMatch ("/foo/manos/baz/", 0, out data, out end);
			
			Assert.IsTrue (is_match);
		}
		
		[Test]
		public void IsMatch_SinglePatternInMiddle_FindsData ()
		{
			var simple = new SimpleMatchOperation ("/foo/{bar}/baz/");
			
			int end;
			DataDictionary data = new DataDictionary ();
			simple.IsMatch ("/foo/manos/baz/", 0, out data, out end);
			
			string bar_value = data ["bar"];
			Assert.AreEqual ("manos", bar_value);
		}
		
		[Test]
		public void IsMatch_SinglePatternInMiddle_UpdatesEnd ()
		{
			var simple = new SimpleMatchOperation ("/foo/{bar}/baz/");
			
			int end;
			DataDictionary data = new DataDictionary ();
			simple.IsMatch ("/foo/manos/baz/", 0, out data, out end);
			
			Assert.AreEqual (15, end);
		}
		
		[Test]
		public void IsMatch_MultiplePatterns_ReturnsTrue ()
		{
			var simple = new SimpleMatchOperation ("{foo}/{bar}");
			
			int end;
			DataDictionary data;
			
			bool is_match = simple.IsMatch ("match1/match2", 0, out data, out end);
			Assert.IsTrue (is_match);
		}
		
		[Test]
		public void IsMatch_MultiplePatterns_FindsData ()
		{
			var simple = new SimpleMatchOperation ("{foo}/{bar}");
			
			int end;
			DataDictionary data;
			
			simple.IsMatch ("match1/match2", 0, out data, out end);
			
			string match1 = data ["foo"];
			string match2 = data ["bar"];
			
			Assert.AreEqual ("match1", match1, "match 1");
			Assert.AreEqual ("match2", match2, "match 2");
		}
		
		[Test]
		public void IsMatch_MultiplePatterns_UpdatesEnd ()
		{
			var simple = new SimpleMatchOperation ("{foo}/{bar}");
			
			int end;
			DataDictionary data;
			simple.IsMatch ("manos/mono", 0, out data, out end);
			
			Assert.AreEqual (10, end);
		}
		
		[Test]
		public void IsMatch_SingleMatchNonMatchingString_ReturnsFalse ()
		{
			var simple = new SimpleMatchOperation ("/foo/{bar}/");
			
			int end;
			DataDictionary data;
			
			bool is_match = simple.IsMatch ("nope/no/", 0, out data, out end);
			Assert.IsFalse (is_match);
		}
		
		[Test]
		public void IsMatch_SingleMatchNonMatchingString_DoesntAddData ()
		{
			var simple = new SimpleMatchOperation ("/foo/{bar}/");
			
			int end;
			DataDictionary data;
			
			simple.IsMatch ("nope/no/", 0, out data, out end);
			Assert.IsNull (data);
		}
		
		[Test]
		public void IsMatch_MatchBeforeEnd_SetsEndCorrectly ()
		{
			var simple = new SimpleMatchOperation ("/{bar}/");
			
			int end;
			DataDictionary data;
			
			simple.IsMatch ("/yup/more/data/foo/", 0, out data, out end);
			Assert.AreEqual (5, end);
		}
		
		public void IsMatch_MatchSecondLastChar_ReturnsTrue ()
		{
			var simple = new SimpleMatchOperation ("/foo/{bar}~");
			
			int end;
			DataDictionary data;
			
			bool is_match = simple.IsMatch ("/foo/barbaz~", 0, out data, out end);
			Assert.IsTrue (is_match);
		}
	}
}

