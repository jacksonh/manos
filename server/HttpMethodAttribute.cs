
using System;
using System.Reflection;


namespace Mango {

	[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
	public abstract class HttpMethodAttribute : Attribute {

		public HttpMethodAttribute ()
		{
		}

		public HttpMethodAttribute (string [] patterns)
		{
			Patterns = patterns;
		}

		public string Name {
			get;
			set;
		}

		public string [] Methods {
			get;
			protected set;
		}

		public string [] Patterns {
			get;
			private set;
		}
	}
}


