
using System;
using System.IO;
using System.Text;
using System.Reflection;
using System.Collections.Generic;
using Manos.Server;


namespace Manos.Templates {

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

	public static class TemplateFilterManager {

		public static MethodInfo GetFilter (string filter)
		{
			Type bin = typeof (BuiltinFilters);

			MethodInfo res = bin.GetMethod (String.Concat ("__", filter), BindingFlags.Static | BindingFlags.Public);

			return res;
		}
	}

	public interface ITemplatePage {

		void Render (IManosContext context, Stream stream, object the_arg);
	}
}


