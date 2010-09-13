using System;

namespace Manos {
	
	
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
	public class RouteAttribute : HttpMethodAttribute {

		public RouteAttribute ()
		{
		}

		public RouteAttribute (params string [] patterns) : base (patterns)
		{
			Methods = HttpMethods.RouteMethods;
		}
	}
}

