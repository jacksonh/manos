
using System;
using System.Reflection;


namespace Manos {

	[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
	public abstract class IgnoreAttribute : Attribute {


	}

}
