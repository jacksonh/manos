

using System;
using System.Reflection;


namespace Mango {

	[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
	public class GetAttribute : HttpMethodAttribute {

		public GetAttribute ()
		{
		}

		public GetAttribute (params string [] patterns) : base (patterns)
		{
			Methods = new string [] { "GET" };
		}
	}
}


