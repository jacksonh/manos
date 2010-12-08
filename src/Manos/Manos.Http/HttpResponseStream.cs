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
		private long length;
		private bool chunk_encode = true;
		private bool final_chunk_sent;

		private Queue<IWriteOperation> write_ops;

		public HttpResponseStream (HttpResponse response, SocketStream stream)
		{
			Response = response;
			SocketStream = stream;
		}

		public HttpResponse Response {
			get;
			private set;
		}

		public SocketStream SocketStream {
			get;
			private set;
		}

		public bool Chunked {
			get { return chunk_encode; }
			set {
				if (length > 0 && chunk_encode != value)
					throw new InvalidOperationException ("Chunked can not be changed after a write has been performed.");
				chunk_encode = value;
			}
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
			Write (buffer, offset, count, chunk_encode);
		}

		public void SendFile (string file_name)
		{
			EnsureMetadata ();

			var file_stream = new FileStream (file_name, FileMode.Open, FileAccess.Read);
			var write_file = new SendFileOperation (file_stream, null);

			length += file_stream.Length;

			if (chunk_encode)
				SendChunk ((int) file_stream.Length, false);
			QueueWriteOperation (write_file);

			if (chunk_encode)
				SendChunk (-1, false);
		}

		private void Write (byte [] buffer, int offset, int count, bool chunked)
		{
			EnsureMetadata ();

			var bytes = new List<ArraySegment<byte>> ();

			if (chunked)
				WriteChunk (bytes, count, false);

			length += (count - offset);
			
			bytes.Add (new ArraySegment<byte> (buffer, offset, count));
			if (chunked)
				WriteChunk (bytes, -1, false);

			var write_bytes = new SendBytesOperation (bytes, null);
			QueueWriteOperation (write_bytes);
		}

		public void End (WriteCallback callback)
		{
			if (chunk_encode) {
				SendFinalChunk (callback);
				return;
			}

			SendBufferedOps (callback);
		}

		public void SendFinalChunk (WriteCallback callback)
		{
			EnsureMetadata ();

			if (!chunk_encode || final_chunk_sent)
				return;
			final_chunk_sent = true;

			var bytes = new List<ArraySegment<byte>> ();

			WriteChunk (bytes, 0, true);

			var write_bytes = new SendBytesOperation (bytes, callback);
			QueueWriteOperation (write_bytes);
		}

		public void SendBufferedOps (WriteCallback callback)
		{
			if (write_ops != null) {
				IWriteOperation [] ops = write_ops.ToArray ();

				for (int i = 0; i < ops.Length; i++) {
					SocketStream.QueueWriteOperation (ops [i]);
				}
			}

			SocketStream.QueueWriteOperation (new NopWriteOperation (callback));
		}

		private void EnsureMetadata ()
		{
			if (!chunk_encode || Response.metadata_written)
				return;
			Response.WriteMetadata ();
		}

		private void QueueWriteOperation (IWriteOperation op)
		{
			if (chunk_encode) {
				SocketStream.QueueWriteOperation (op);
				return;
			}

			if (write_ops == null)
				write_ops = new Queue<IWriteOperation> ();

			write_ops.Enqueue (op);
		}

		private void SendChunk (int l, bool last)
		{
			var bytes = new List<ArraySegment<byte>> ();

			WriteChunk (bytes, l, last);

			var write_bytes = new SendBytesOperation (bytes, null);
			QueueWriteOperation (write_bytes);
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

			length += i;
			
			bytes.Add (new ArraySegment<byte> (chunk_buffer, 0, i));
		}
	}
}

