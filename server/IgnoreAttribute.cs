
using System;
using System.Reflection;


namespace Mango {

	[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
	public abstract class IgnoreAttribute : Attribute {


	}

}
