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
	
	public static class TemplateFactory {
		
		private static Dictionary<string,IManosTemplate> templates = new Dictionary<string, IManosTemplate> ();
		
		public static IManosTemplate Get (string name)
		{
			IManosTemplate res = null;
			
			if (!TryGet (name, out res))
				return null;
			
			return res;
		}
		
		public static bool TryGet (string name, out IManosTemplate template)
		{
			if (name == null)
				throw new ArgumentNullException ("name");
			
			return templates.TryGetValue (name, out template);
		}
		
		public static void Register (string name, IManosTemplate template)
		{
			if (name == null)
				throw new ArgumentNullException ("name");
			if (template == null)
				throw new ArgumentNullException ("template");
			
			if (templates.ContainsKey (name))
				throw new InvalidOperationException (String.Format ("A template named {0} has already been registered.", name));
			
			templates.Add (name, template);
		}
		
		public static void Clear ()
		{
			templates.Clear ();
		}
	}
}

