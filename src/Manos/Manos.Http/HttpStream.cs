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
using Manos.Collections;

namespace Manos.Http
{
	public class HttpStream : System.IO.Stream, IDisposable
	{
		private long length;
		private bool chunk_encode = true;
		private bool metadata_written;
		private bool final_chunk_sent;
		private Queue<object> write_ops;

		public HttpStream (HttpEntity entity, Manos.IO.Stream stream)
		{
			HttpEntity = entity;
			SocketStream = stream;
			AddHeaders = true;
		}

		public HttpEntity HttpEntity {
			get;
			private set;
		}

		public Manos.IO.Stream SocketStream {
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

		public bool AddHeaders {
			get;
			set;
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
			get { return length; }
		}

		public override long Position {
			get { return length; }
			set { Seek (value, SeekOrigin.Begin); }
		}

		public override void Flush ()
		{
		}

		public override int Read (byte[] buffer, int offset, int count)
		{
			throw new NotSupportedException ("Can not Read from an HttpStream.");
		}

		public override long Seek (long offset, SeekOrigin origin)
		{
			throw new NotSupportedException ("Can not seek on an HttpStream.");
		}

		public override void SetLength (long value)
		{
			throw new NotSupportedException ("Can not set the length of an HttpStream.");
		}

		public override void Write (byte[] buffer, int offset, int count)
		{
			Write (buffer, offset, count, chunk_encode);
		}

		public void SendFile (string file_name)
		{
			EnsureMetadata ();
			
			var len = Manos.IO.Libev.FileStream.GetLength (file_name);
			length += len;
			
			QueueFile (file_name);
		}

		IEnumerable<ByteBuffer> SendCallback (Action callback)
		{
			callback ();
			yield break;
		}

		void SendFileData (string fileName)
		{
			if (SocketStream is ISendfileCapable) {
				((ISendfileCapable) SocketStream).SendFile (fileName);
			} else {
				SocketStream.PauseWriting ();
				var fs = Manos.IO.Libev.FileStream.OpenRead (fileName, 64 * 1024);
				SocketStream.Write (new StreamCopySequencer (fs, SocketStream, true));
			}
			SocketStream.Write (SendCallback (SendBufferedOps));
		}

		void SendFileImpl (string fileName)
		{
			var len = Manos.IO.Libev.FileStream.GetLength (fileName);
			if (chunk_encode) {
				SendChunk (len, false);
				SendFileData (fileName);
				SendChunk (-1, false);
			} else {
				SendFileData (fileName);
			}
		}

		private void Write (byte [] buffer, int offset, int count, bool chunked)
		{
			EnsureMetadata ();

			if (chunked)
				SendChunk (count, false);

			length += (count - offset);
			
			QueueBuffer (new ByteBuffer (buffer, offset, count));
			
			if (chunked)
				SendChunk (-1, false);
		}

		public void End ()
		{
			End (null);
		}

		public void End (Action callback)
		{
			if (chunk_encode) {
				SendFinalChunk (callback);
				return;
			}
			
			if (callback != null) {
				if (write_ops == null)
					write_ops = new Queue<object> ();
				write_ops.Enqueue (callback);
			}

			WriteMetadata ();
			SendBufferedOps ();
		}

		public void SendFinalChunk (Action callback)
		{
			EnsureMetadata ();

			if (!chunk_encode || final_chunk_sent)
				return;

			final_chunk_sent = true;

			SendChunk (0, true);
			SocketStream.Write (SendCallback (callback));
		}

		public void SendBufferedOps ()
		{
			if (write_ops != null) {
				while (write_ops.Count > 0) {
					var op = write_ops.Dequeue ();
					if (op is ByteBuffer) {
						SocketStream.Write ((ByteBuffer) op);
					} else if (op is string) {
						SendFileImpl ((string) op);
						return;
					} else if (op is Action) {
						SocketStream.Write (SendCallback ((Action) op));
					} else {
						throw new InvalidOperationException ();
					}
				}
			}
		}

		void WriteMetadata ()
		{
			if (AddHeaders) {
				if (chunk_encode) {
					HttpEntity.Headers.SetNormalizedHeader ("Transfer-Encoding", "chunked");
				} else {
					HttpEntity.Headers.ContentLength = Length;
				}
			}
			
			StringBuilder builder = new StringBuilder ();
			HttpEntity.WriteMetadata (builder);

			byte [] data = Encoding.ASCII.GetBytes (builder.ToString ());

			metadata_written = true;
			
			SocketStream.Write (data);
		}

		void EnsureMetadata ()
		{
			if (!chunk_encode || metadata_written)
				return;

			WriteMetadata ();
		}

		private void QueueBuffer (ByteBuffer buffer)
		{
			if (chunk_encode) {
				SocketStream.Write (buffer);
				return;
			}

			if (write_ops == null)
				write_ops = new Queue<object> ();

			write_ops.Enqueue (buffer);
		}

		private void QueueFile (string file)
		{
			if (chunk_encode) {
				SendFileImpl (file);
				return;
			}

			if (write_ops == null)
				write_ops = new Queue<object> ();

			write_ops.Enqueue (file);
		}

		private void SendChunk (long l, bool last)
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
			
			QueueBuffer (new ByteBuffer (chunk_buffer, 0, i));
		}
	}
}

