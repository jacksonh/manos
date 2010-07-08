

using System;
using System.Reflection;


namespace Mango {

	[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
	public class GetAttribute : Attribute {

		public GetAttribute (string routes)
		{
		}

		public GetAttribute (params string [] routes)
		{
		}
	}
}


