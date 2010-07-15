
using System;
using System.Reflection;


namespace Mango {

	[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
	public class DeleteAttribute : HttpMethodAttribute {

		public DeleteAttribute (params string [] patterns) : base (patterns)
		{
			Methods = new string [] { "DELETE" };
		}
	}
}


