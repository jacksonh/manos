
using System;
using NUnit.Framework;

using Manos.Routing;
using Manos.ShouldExt;


namespace Manos.Routing.Tests
{

	[TestFixture()]
	public class MatchOperationFactoryTest
	{

		[Test ()]
		public void TestCreateNull ()
		{
			Should.Throw<ArgumentNullException> (() => MatchOperationFactory.Create (null));
		}
		
		[Test ()]
		public void TestIsNop ()
		{
			IMatchOperation op;
			
			op = MatchOperationFactory.Create (String.Empty);
			Should.BeInstanceOf<NopMatchOperation> (op, "a1");
		}
		
		[Test()]
		public void TestIsRegex ()
		{
			IMatchOperation op;
			
			op = MatchOperationFactory.Create ("dog.");
			Should.BeInstanceOf<RegexMatchOperation> (op, "a1");
			
			op = MatchOperationFactory.Create (".dog");
			Should.BeInstanceOf<RegexMatchOperation> (op, "a2");
			
			op = MatchOperationFactory.Create ("d.og");
			Should.BeInstanceOf<RegexMatchOperation> (op, "a3");
			
			op = MatchOperationFactory.Create (".");
			Should.BeInstanceOf<RegexMatchOperation> (op, "a4");
			
			op = MatchOperationFactory.Create ("[dog]");
			Should.BeInstanceOf<RegexMatchOperation> (op, "a6");
			
			op = MatchOperationFactory.Create ("(dog)");
			Should.BeInstanceOf<RegexMatchOperation> (op, "a7");
			
			op = MatchOperationFactory.Create ("^dog");
			Should.BeInstanceOf<RegexMatchOperation> (op, "a8");
			
			op = MatchOperationFactory.Create ("dog*");
			Should.BeInstanceOf<RegexMatchOperation> (op, "a9");
			
			op = MatchOperationFactory.Create (".*dog");
			Should.BeInstanceOf<RegexMatchOperation> (op, "a10");
			
			op = MatchOperationFactory.Create ("$dog");
			Should.BeInstanceOf<RegexMatchOperation> (op, "a11");
			
			op = MatchOperationFactory.Create ("dog$");
			Should.BeInstanceOf<RegexMatchOperation> (op, "a12");
		}
		
		[Test]
		public void Create_SimpleMatchInMiddle_CreatesSimpleMatch ()
		{
			IMatchOperation op = MatchOperationFactory.Create ("/Foo/{bar}/");
			
			Should.BeInstanceOf<SimpleMatchOperation> (op);
		}
		
		[Test]
		public void Create_SimpleMatchAtBeginning_CreatesSimpleMatch ()
		{
			IMatchOperation op = MatchOperationFactory.Create ("{bar}/Foo");
			
			Should.BeInstanceOf<SimpleMatchOperation> (op);
		}
		
		[Test]
		public void Create_SimpleMatchAtEnd_CreatesSimpleMatch ()
		{
			IMatchOperation op = MatchOperationFactory.Create ("/Foo/{bar}");
			
			Should.BeInstanceOf<SimpleMatchOperation> (op);
		}
		
		[Test]
		public void Create_SimpleMatchIsWholePattern_CreatesSimpleMatch ()
		{
			IMatchOperation op = MatchOperationFactory.Create ("{bar}");
			
			Should.BeInstanceOf<SimpleMatchOperation> (op);
		}
		
		[Test]
		public void Create_EscapedOpenSimpleMatch_CreatesStringMatch ()
		{
			IMatchOperation op = MatchOperationFactory.Create ("{{bar}");
			
			Should.BeInstanceOf<StringMatchOperation> (op);
		}
		
		[Test]
		public void Create_EscapedCloseSimpleMatch_CreatesStringMatch ()
		{
			IMatchOperation op = MatchOperationFactory.Create ("{bar}}");
			
			Should.BeInstanceOf<StringMatchOperation> (op);
		}
		
		[Test]
		public void TestIsString ()
		{
			IMatchOperation op;
			
			op = MatchOperationFactory.Create ("foobar");
			Should.BeInstanceOf<StringMatchOperation> (op, "a1");
			
			op = MatchOperationFactory.Create ("1");
			Should.BeInstanceOf<StringMatchOperation> (op, "a2");
			
			op = MatchOperationFactory.Create ("i am the walrus");
			Should.BeInstanceOf<StringMatchOperation> (op, "a3");
		}
	}
}
