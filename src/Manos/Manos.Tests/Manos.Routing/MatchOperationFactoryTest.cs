
using System;
using NUnit.Framework;

using Mango.Routing;


namespace Mango.Routing.Tests
{

	[TestFixture()]
	public class MatchOperationFactoryTest
	{

		[Test ()]
		public void TestCreateNull ()
		{
			Assert.Throws<ArgumentNullException> (() => MatchOperationFactory.Create (null));
		}
		
		[Test ()]
		public void TestIsNop ()
		{
			IMatchOperation op;
			
			op = MatchOperationFactory.Create (String.Empty);
			Assert.IsInstanceOf<NopMatchOperation> (op, "a1");
		}
		
		[Test()]
		public void TestIsRegex ()
		{
			IMatchOperation op;
			
			op = MatchOperationFactory.Create ("dog.");
			Assert.IsInstanceOf<RegexMatchOperation> (op, "a1");
			
			op = MatchOperationFactory.Create (".dog");
			Assert.IsInstanceOf<RegexMatchOperation> (op, "a2");
			
			op = MatchOperationFactory.Create ("d.og");
			Assert.IsInstanceOf<RegexMatchOperation> (op, "a3");
			
			op = MatchOperationFactory.Create (".");
			Assert.IsInstanceOf<RegexMatchOperation> (op, "a4");
			
			op = MatchOperationFactory.Create ("[dog]");
			Assert.IsInstanceOf<RegexMatchOperation> (op, "a6");
			
			op = MatchOperationFactory.Create ("(dog)");
			Assert.IsInstanceOf<RegexMatchOperation> (op, "a7");
			
			op = MatchOperationFactory.Create ("^dog");
			Assert.IsInstanceOf<RegexMatchOperation> (op, "a8");
			
			op = MatchOperationFactory.Create ("dog*");
			Assert.IsInstanceOf<RegexMatchOperation> (op, "a9");
			
			op = MatchOperationFactory.Create (".*dog");
			Assert.IsInstanceOf<RegexMatchOperation> (op, "a10");
			
			op = MatchOperationFactory.Create ("$dog");
			Assert.IsInstanceOf<RegexMatchOperation> (op, "a11");
			
			op = MatchOperationFactory.Create ("dog$");
			Assert.IsInstanceOf<RegexMatchOperation> (op, "a12");
		}
		
		[Test]
		public void TestIsString ()
		{
			IMatchOperation op;
			
			op = MatchOperationFactory.Create ("foobar");
			Assert.IsInstanceOf<StringMatchOperation> (op, "a1");
			
			op = MatchOperationFactory.Create ("1");
			Assert.IsInstanceOf<StringMatchOperation> (op, "a2");
			
			op = MatchOperationFactory.Create ("i am the walrus");
			Assert.IsInstanceOf<StringMatchOperation> (op, "a3");
		}
	}
}
