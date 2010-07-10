

using System;
using System.Reflection;


namespace Mango {

	[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
	public class PutAttribute : HttpMethodAttribute {

		public PutAttribute (params string [] patterns) : base (patterns)
		{
			Methods = new string [] { "PUT" };
		}
	}
}


