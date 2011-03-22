//
// Copyright (C) 2010 Jackson Harper (jackson@manosdemono.com)
// Copyright (C) 2011 Andrius Bentkus (andrius.bentkus@gmail.com)
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
using System.Linq;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;

using Manos;

namespace Manos.Tool
{
	public class Loader
	{
		public static T LoadLibrary<T> (string library, IList<string> arguments)
		{
			T app = default(T);
			Assembly a = Assembly.LoadFrom (library);
			
			foreach (Type t in a.GetTypes ()) {
				if (typeof(T).IsAssignableFrom(t)) {
					app = CreateAppInstance<T> (t, arguments);
				}
			}

			return app;
		}
		
		public static T CreateAppInstance<T> (Type t, IList<string> arguments)
		{
			int arg_count = arguments.Count;
			ConstructorInfo [] constructors = t.GetConstructors ();
			
			foreach (ConstructorInfo ci in constructors.Where (c => c.GetParameters ().Count () == arg_count)) {
				object [] args = ArgsForParams (ci.GetParameters (), arguments);
				if (args == null)
					continue;
				try {
					return (T) Activator.CreateInstance (t, args);
				} catch (Exception e) {
					Console.Error.WriteLine ("Exception creating App Type: '{0}'.", t);
					Console.Error.WriteLine (e);
				}
			}
			
			return default(T);
		}
		
		public static object [] ArgsForParams (ParameterInfo [] prms, IList<string> arguments)
		{
			object [] res = new object [prms.Length];
			
			for (int i = 0; i < prms.Count (); i++) {
				try {
					res [i] = Convert.ChangeType (arguments [i], prms [i].ParameterType);
				} catch (Exception e) {
					Console.Error.WriteLine ("Exception converting type: '{0}'.", prms [i].ParameterType);
					Console.Error.WriteLine (e);
					
					return null;
				}
			}
			
			return res;
		}

	}
}

