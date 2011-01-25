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
using System.Collections;
using System.Collections.Generic;

using Manos.IO;

namespace Manos.Http
{
	public class HttpStreamWriterWrapper : Stream
	{
		private HttpStream stream;

		public HttpStreamWriterWrapper (HttpStream stream)
		{
			this.stream = stream;
		}

		
		public override bool CanRead {
			get { return stream.CanRead; }
		}

		public override bool CanSeek {
			get { return stream.CanSeek; }
		}

		public override bool CanWrite {
			get { return stream.CanWrite; }
		}

		public override long Length {
			get {
				return stream.Length;
			}
		}
		
		public override long Position {
			get {
				return stream.Position;
			}
			set {
				stream.Position = value;
			}
		}
		
		public override void Flush ()
		{
			stream.Flush ();
		}

		public override int Read (byte [] buffer, int offset, int count)
		{
			return stream.Read (buffer, offset, count);
		}
		
		public override long Seek (long offset, SeekOrigin origin)
		{
			return stream.Seek (offset, origin);
		}

		public override void SetLength (long value)
		{
			stream.SetLength (value);
		}

		public override void Write (byte[] buffer, int offset, int count)
		{
			stream.Write ((byte []) buffer.Clone (), offset, count);
		}
	}

}



