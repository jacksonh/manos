//
// Copyright (C) 2010 Jackson Harper (jackson@manosdemono.com)
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//
//


using System;
using System.IO;
using System.Text;
using System.Reflection;
using System.Collections.Generic;


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


