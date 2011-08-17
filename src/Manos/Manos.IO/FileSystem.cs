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
using System.Linq;
using System.Text;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using Libev;
using System.IO;


namespace Manos.IO {

	public static class FileSystem {
#if WINDOWS
        public static void GetFileLength(string path, Action<long, int> cb)
        {
            long l = 0;
            int e = 0;
            try
            {
                l = new System.IO.FileInfo(path).Length;
            }
            catch (FileNotFoundException)
            {
                e = 1;
            }
            catch (IOException)
            {
                e = 2;
            }
            cb(l, e);
        }

#else
		public static void GetFileLength (string path, Action<long,int> cb)
		{
			GCHandle handle = GCHandle.Alloc (cb);
			manos_file_get_length (path, LengthCallbackHandler, GCHandle.ToIntPtr (handle));

		}

		public static void LengthCallbackHandler (IntPtr gchandle, IntPtr length, int error)
		{
			GCHandle handle = GCHandle.FromIntPtr (gchandle);
			Action<long,int> cb = (Action<long,int>) handle.Target;

			handle.Free ();

			cb (length.ToInt64 (), error);
		}
		
		[DllImport ("libmanos", CallingConvention = CallingConvention.Cdecl)]
		private static extern void manos_file_get_length (string path, Action<IntPtr,IntPtr,int> cb, IntPtr gchandle);
#endif
	}
}

