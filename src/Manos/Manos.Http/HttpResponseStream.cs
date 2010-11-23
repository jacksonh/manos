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
	public class HttpResponseStream : Stream
	{
		private int length;
		private bool final_chunk_sent;

		public HttpResponseStream (HttpResponse response, IOStream stream)
		{
			Response = response;
			IOStream = stream;
		}

		public HttpResponse Response {
			get;
			private set;
		}

		public IOStream IOStream {
			get;
			private set;
		}

		public override bool CanRead {
			get { return false; }
		}

		public override bool CanSeek {
			get { return false; }
		}

		public override bool CanWrite {
			get { return true; }
		}

		public override long Length {
			get {
				return length;
			}
		}
		
		public override long Position {
			get {
				return length;
			}
			set {
				Seek (value, SeekOrigin.Begin);
			}
		}
		
		public override void Flush ()
		{
			// NOOP
		}

		public override int Read (byte[] buffer, int offset, int count)
		{
			throw new NotSupportedException ("Can not Read from an HttpResponseStream.");
		}
		
		public override long Seek (long offset, SeekOrigin origin)
		{
			throw new NotSupportedException ("Can not seek on an HttpResponseStream.");
		}

		public override void SetLength (long value)
		{
			throw new NotSupportedException ("Can not set the length of an HttpResponseStream.");
		}

		public override void Write (byte[] buffer, int offset, int count)
		{
			Write (buffer, offset, count, true);
		}

		public void WriteNoChunk (byte [] buffer, int offset, int count)
		{
			Write (buffer, offset, count, false);
		}

		public void SendFile (string file_name)
		{
			EnsureMetadata ();

			FileStream file_stream = new FileStream (file_name, FileMode.Open, FileAccess.Read);
			WriteFileOperation write_file = new WriteFileOperation (file_stream, null);

			SendChunk ((int) file_stream.Length, false);
			IOStream.QueueWriteOperation (write_file);
			SendChunk (-1, false);
		}

		private void Write (byte [] buffer, int offset, int count, bool chunked)
		{
			EnsureMetadata ();

			var bytes = new List<ArraySegment<byte>> ();

			if (chunked)
				WriteChunk (bytes, count, false);
			bytes.Add (new ArraySegment<byte> (buffer, offset, count));
			if (chunked)
				WriteChunk (bytes, -1, false);

			WriteBytesOperation write_bytes = new WriteBytesOperation (bytes, null);

			IOStream.QueueWriteOperation (write_bytes);
		}

		public void SendFinalChunk (WriteCallback callback)
		{
			EnsureMetadata ();

			if (final_chunk_sent)
				return;
			final_chunk_sent = true;

			var bytes = new List<ArraySegment<byte>> ();

			WriteChunk (bytes, 0, true);

			WriteBytesOperation write_bytes = new WriteBytesOperation (bytes, callback);
			IOStream.QueueWriteOperation (write_bytes);
		}

		private void EnsureMetadata ()
		{
			if (Response.metadata_written)
				return;
			Response.WriteMetadata ();
		}

		private void SendChunk (int l, bool last)
		{
			var bytes = new List<ArraySegment<byte>> ();

			WriteChunk (bytes, l, last);

			WriteBytesOperation write_bytes = new WriteBytesOperation (bytes, null);
			IOStream.QueueWriteOperation (write_bytes);
		}

		private void WriteChunk (List<ArraySegment<byte>> bytes, int l, bool last)
		{
			if (l == 0 && !last)
				return;

			int i = 0;
			byte [] chunk_buffer = new byte [24];

			if (l >= 0) {
				string s = l.ToString ("x");
				for (; i < s.Length; i++)
					chunk_buffer [i] = (byte) s [i];
			}

			chunk_buffer [i++] = 13;
			chunk_buffer [i++] = 10;
			if (last) {
				chunk_buffer [i++] = 13;
				chunk_buffer [i++] = 10;
			}

			bytes.Add (new ArraySegment<byte> (chunk_buffer, 0, i));
		}
	}
}

