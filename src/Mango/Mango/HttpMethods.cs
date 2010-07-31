
using System;


namespace Mango {

	public static class HttpMethods {

		public static readonly string [] GetMethods = new string [] { "GET" };
		public static readonly string [] HeadMethods = new string [] { "HEAD" };
		public static readonly string [] PostMethods = new string [] { "POST" };
		public static readonly string [] PutMethods = new string [] { "PUT" };
		public static readonly string [] DeleteMethods = new string [] { "DELETE" };
		public static readonly string [] TraceMethods = new string [] { "TRACE" };
		public static readonly string [] OptionsMethods = new string [] { "OPTIONS" };
		
		public static readonly string [] RouteMethods = new string [] { "GET", "PUT", "POST", "HEAD", "DELETE", "TRACE", "OPTIONS" };

	}
}

