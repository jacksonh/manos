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

namespace Manos.Collections {

	public class ByteBuffer {

		int length;
		int position;

		public byte [] Bytes;

		public ByteBuffer (byte [] bytes, int position, int length)
		{
			Bytes = bytes;
			this.position = position;
			this.length = length;
		}

		public byte CurrentByte {
			get { return Bytes [position]; }
		}

		public int Length {
			get { return length; }
			set {
				if (value > Bytes.Length)
					throw new ArgumentOutOfRangeException ("value", "Can not increase the size of a byte buffer.");
				if (value < 0)
					throw new ArgumentOutOfRangeException ("value", "Length must be zero or greater.");
				length = value;
			}
		}

		public int Position {
			get { return position; }
			set {
				if (value > length)
					throw new ArgumentOutOfRangeException ("value", "Position must be less than the array length.");
				if (value < 0)
					throw new ArgumentOutOfRangeException ("value", "Position must be zero or greater.");

				Console.WriteLine ("updating position to: '{0}'", value);
				position = value;
			}
		}

		public byte ReadByte ()
		{
			if (position >= length)
				throw new InvalidOperationException ("Read past end of ByteBuffer.");
			return Bytes [position++];
		}
	}
}

