
Contributing
============

Patches are welcome.  The easiest way to contribute to Manos is to
fork the project on github and open a pull request once your patch is
ready.

Its highly appreciated if all new features include unit
tests. Preferably nunit tests that are added to the Manos.Tests.dll
assembly.

Before starting a new task its best to email the mailing list to
ensure that no one else is working on the same feature.


Unit Testing Practices
----------------------

Its very important that the unit tests are clear and as singular as
possible in their purpose. Tests methods should obey the following
structure:

	public void <Name of method being tested>_<conditions of test>_<expected result>
	{
		....
	}
	
For example:

	[Test]
	public void Ctor_NullName_Throws ()
	{
		Should.Throw<ArgumentNullException> (() => new HttpCookie (null, "value"));
	}

Test methods should also only have a single assert condition.  Don't
test for multiple things in a single test.  Something like this would
be considered bad:

	[Test]
	public void Ctor_ValidValues_AddsPair ()
	{
		var name = "foobar";
		var value = "the value";
			
		var cookie = new HttpCookie (name, value);
		Assert.AreEqual (1, cookie.Values.Count);
		Assert.AreEqual ("the value", cookie.Values ["foobar"]);
	}

That should be split up into two different tests.
