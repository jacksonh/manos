

using System;
using System.Reflection;


namespace Mango {

	[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
	public class GetAttribute : Attribute {

		public GetAttribute (string url)
		{
		}
	}
}


