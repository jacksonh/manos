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

namespace Manos.IO
{
	/// <summary>
	/// Byte buffers are used avoid expensive array copy operations when
	/// only parts of an array are valid.
	/// </summary>
	public sealed class ByteBuffer
	{
		int position;
		int length;
		byte [] bytes;
		
		/// <summary>
		/// Initializes a new instance of the <see cref="Manos.IO.ByteBuffer"/> class.
		/// </summary>
		public ByteBuffer (byte [] bytes, int position, int length)
		{
			this.bytes = bytes;
			this.position = position;
			this.length = length;
		}
		
		/// <summary>
		/// Gets the byte at position <see cref="Position"/> in
		/// <see cref="Bytes"/>.
		/// </summary>
		public byte CurrentByte {
			get { return bytes [position]; }
		}
		
		/// <summary>
		/// Gets the byte array wrapped by this instance.
		/// </summary>
		public byte [] Bytes {
			get { return bytes; }
		}
		
		/// <summary>
		/// Gets the length of the segment of valid data within <see cref="Bytes"/>.
		/// This length may be <c>0</c>.
		/// </summary>
		public int Length {
			get { return length; }
		}
		
		/// <summary>
		/// Gets the position at which the segment of valid data within <see cref="Bytes"/>
		/// starts, i.e. the first valid byte.
		/// </summary>
		public int Position {
			get { return position; }
		}
		
		/// <summary>
		/// Reads and consumes one byte from the buffer. This method is mostly equivalent to
		/// copying <see cref="CurrentByte"/> and skipping one byte with <see cref="Skip"/>.
		/// </summary>
		/// <returns>
		/// The byte.
		/// </returns>
		/// <exception cref='InvalidOperationException'>
		/// Is thrown when no more bytes are left to read.
		/// </exception>
		public byte ReadByte ()
		{
			if (length == 0)
				throw new InvalidOperationException ("Read past end of ByteBuffer.");
			length--;
			return bytes [position++];
		}
		
		/// <summary>
		/// Consumes <see cref="bytes"/> bytes of valid data by advancing <see cref="Position"/>
		/// and decreasing <see cref="Length"/>.
		/// </summary>
		/// <param name='bytes'>
		/// Number of bytes to skip.
		/// </param>
		/// <exception cref='ArgumentException'>
		/// When <see cref="bytes"/> is less than <c>0</c>, or greater than the remaining amount
		/// of data in the valid segment.
		/// </exception>
		public void Skip (int bytes)
		{
			if (bytes < 0)
				throw new ArgumentException ("Can't move backwards in buffer.");
			if (bytes > length)
				throw new ArgumentException ("Can't move past end of buffer.");
			position += bytes;
			length -= bytes;
		}
	}
}

