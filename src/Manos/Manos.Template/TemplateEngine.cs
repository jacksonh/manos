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
using System.Linq;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;


namespace Manos.Templates {

	public static class TemplateEngine {

		private static Dictionary<string,Type> loaded_templates = new Dictionary<string,Type> ();
		private static Assembly template_assembly = null;

		public static string RenderToSting (string path)
		{
			return RenderToString (path, new object ());
		}

		public static string RenderToString (string path, object the_arg)
		{
			MemoryStream stream = new MemoryStream ();
			StreamWriter writer = new StreamWriter (stream);

			RenderToStream (path, writer, the_arg);
			writer.Flush ();

			stream.Seek (0, SeekOrigin.Begin);
			StreamReader reader = new StreamReader (stream);
			return reader.ReadToEnd ();
		}

		public static void RenderToStream (string path, TextWriter writer, object the_arg)
		{
			/*
			string name = Page.FullTypeNameForPath (ApplicationName, path);
			Type t = GetType (name);

			IMingePage page = (IMingePage) Activator.CreateInstance (t);
			page.RenderToStream (writer, the_arg);
			*/
		}

		private static Type GetType (string name)
		{
			Type res = null;

			if (loaded_templates.TryGetValue (name, out res))
				return res;

			if (template_assembly == null) {
				foreach (Assembly a in AppDomain.CurrentDomain.GetAssemblies ()) {
					res = a.GetType (name);
					if (res != null) {
						template_assembly = a;
						break;
					}
				}
			}

			loaded_templates.Add (name, res);
			return res;
		}
	}
}

