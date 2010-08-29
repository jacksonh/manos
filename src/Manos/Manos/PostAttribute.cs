
using System;
using System.Reflection;


namespace Manos {

	[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
	public class PostAttribute : HttpMethodAttribute {

		public PostAttribute ()
		{
		}

		public PostAttribute (params string [] patterns) : base (patterns)
		{
			Methods = new string [] { "POST" };
		}
	}
}


