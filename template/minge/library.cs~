
using System;
using System.IO;
using System.Text;
using System.Reflection;
using System.Collections.Generic;


namespace Mango.Templates.Minge {

	public delegate string MingeFilter (string input, object [] args);

	public static class BuiltinFilters {

		public static string __upper (string input)
		{
			return input.ToUpper ();
		}

		public static string __lower (string input)
		{
			return input.ToLower ();
		}

		public static string __default (string input, string default_value)
		{
			if (String.IsNullOrEmpty (input))
				return default_value;

			return input;
		}
	}

	public static class MingeFilterManager {

		public static MethodInfo GetFilter (string filter)
		{
			Type bin = typeof (BuiltinFilters);

			MethodInfo res = bin.GetMethod (String.Concat ("__", filter), BindingFlags.Static | BindingFlags.Public);

			return res;
		}
	}

	public class MingePage {

		public virtual void RenderToStream (TextWriter stream, Dictionary<string,object> args)
		{
		}
	}

}

