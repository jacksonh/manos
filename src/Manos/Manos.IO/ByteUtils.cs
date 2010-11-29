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

namespace Manos.IO {

	  public static class ByteUtils {

	  	 public static int FindDelimiter (byte [] delimiter, byte [] data, int start, int end)
		 {
			start = Array.IndexOf (data, delimiter [0], start, end - start);

			while (start >= 0) {
				bool match = true;
				for (int i = 1; i < delimiter.Length; i++) {
					if (data [start + i] == delimiter [i])
						continue;
					match = false;
					break;
				}
				if (match)
					return start + delimiter.Length;
				start = Array.IndexOf (data, delimiter [0], start + 1);
			}

			return -1;
		 }
	  }
}

