using System;
using System.Collections;
using System.Collections.Specialized;

using NUnit.Framework;
using Manos.ShouldExt;

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
			NameValueCollection data = new NameValueCollection ();
			bool is_match = simple.IsMatch ("/foo/manos/baz/", 0, data, out end);
			
			Assert.IsTrue (is_match);
		}
		
		[Test]
		public void IsMatch_SinglePatternInMiddle_FindsData ()
		{
			var simple = new SimpleMatchOperation ("/foo/{bar}/baz/");
			
			int end;
			NameValueCollection data = new NameValueCollection ();
			simple.IsMatch ("/foo/manos/baz/", 0, data, out end);
			
			string bar_value = data ["bar"];
			Assert.AreEqual ("manos", bar_value);
		}
		
		[Test]
		public void IsMatch_SinglePatternInMiddle_UpdatesEnd ()
		{
			var simple = new SimpleMatchOperation ("/foo/{bar}/baz/");
			
			int end;
			NameValueCollection data = new NameValueCollection ();
			simple.IsMatch ("/foo/manos/baz/", 0, data, out end);
			
			Assert.AreEqual (15, end);
		}
		
		[Test]
		public void IsMatch_MultiplePatterns_ReturnsTrue ()
		{
			var simple = new SimpleMatchOperation ("{foo}/{bar}");
			
			int end;
			NameValueCollection data = new NameValueCollection ();
			
			bool is_match = simple.IsMatch ("match1/match2", 0, data, out end);
			Assert.IsTrue (is_match);
		}
		
		[Test]
		public void IsMatch_MultiplePatterns_FindsData ()
		{
			var simple = new SimpleMatchOperation ("{foo}/{bar}");
			
			int end;
			NameValueCollection data = new NameValueCollection ();
			
			simple.IsMatch ("match1/match2", 0, data, out end);
			
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
			NameValueCollection data = new NameValueCollection ();
			Console.WriteLine ("about to do match");
			simple.IsMatch ("manos/mono", 0, data, out end);
			Console.WriteLine ("done with match");
			
			Assert.AreEqual (10, end);
		}
		
		[Test]
		public void IsMatch_SingleMatchNonMatchingString_ReturnsFalse ()
		{
			var simple = new SimpleMatchOperation ("/foo/{bar}/");
			
			int end;
			NameValueCollection data = new NameValueCollection ();
			
			bool is_match = simple.IsMatch ("nope/no/", 0, data, out end);
			Assert.IsFalse (is_match);
		}
		
		[Test]
		public void IsMatch_SingleMatchNonMatchingString_DoesntAddData ()
		{
			var simple = new SimpleMatchOperation ("/foo/{bar}/");
			
			int end;
			NameValueCollection data = new NameValueCollection ();
			
			simple.IsMatch ("nope/no/", 0, data, out end);
			Assert.AreEqual (0, data.Count);
		}
		
		[Test]
		public void IsMatch_MatchBeforeEnd_SetsEndCorrectly ()
		{
			var simple = new SimpleMatchOperation ("/{bar}/");
			
			int end;
			NameValueCollection data = new NameValueCollection ();
			
			simple.IsMatch ("/yup/more/data/foo/", 0, data, out end);
			Assert.AreEqual (5, end);
		}
	}
}

