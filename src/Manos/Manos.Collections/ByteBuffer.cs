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
using System.Runtime.InteropServices;


namespace Manos.Collections {

	[StructLayout (LayoutKind.Sequential)]
	public struct ByteBufferS {
		public int Position;
		public int Length;
		public byte [] Bytes;

		public ByteBufferS (byte [] data, int position, int length)
		{
			this.Position = position;
			this.Length = length;
			this.Bytes = data;
		}
	}

	public class ByteBuffer {

		internal ByteBufferS buffer;

		public ByteBuffer (byte [] bytes, int position, int length)
		{
			buffer = new ByteBufferS (bytes, position, length);
		}

		public byte CurrentByte {
			get { return buffer.Bytes [buffer.Position]; }
		}

		public byte [] Bytes {
			get { return buffer.Bytes; }
		}

		public int Length {
			get { return buffer.Length; }
			set {
				if (value > buffer.Bytes.Length)
					throw new ArgumentOutOfRangeException ("value", "Can not increase the size of a byte buffer.");
				if (value < 0)
					throw new ArgumentOutOfRangeException ("value", "Length must be zero or greater.");
				buffer.Length = value;
			}
		}

		public int Position {
			get { return buffer.Position; }
			set {
				if (value > buffer.Length)
					throw new ArgumentOutOfRangeException ("value", "Position must be less than the array length.");
				if (value < 0)
					throw new ArgumentOutOfRangeException ("value", "Position must be zero or greater.");
				buffer.Position = value;
			}
		}

		public byte ReadByte ()
		{
			if (buffer.Position >= buffer.Length)
				throw new InvalidOperationException ("Read past end of ByteBuffer.");
			return buffer.Bytes [buffer.Position++];
		}
	}
}

