
using System;
using System.IO;
using System.Text;
using System.Reflection;
using System.Collections.Generic;
using Mango.Server;


namespace Mango.Templates {

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

		public static string __filename (string input)
		{
			return Path.GetFileName (input);
		}

		public static string __filename_noextension (string input)
		{
			return Path.GetFileName (input);
		}

		public static string __remove_extension (string input)
		{
			return Path.GetFileNameWithoutExtension (input);
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

	public interface IMingePage {

		void Render (IMangoContext context, object the_arg);
		void RenderToResponse (IHttpResponse response, object the_arg);
	}

	
	[Serializable]
	public class MingePage : IMingePage {

		public void Render (IMangoContext context, object the_arg)
		{
			RenderToResponse (context.Response, the_arg);
		}

		public virtual void RenderToResponse (IHttpResponse response, object the_arg)
		{
		}
	}

}

