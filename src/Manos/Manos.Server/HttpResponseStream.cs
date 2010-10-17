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
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace Manos.Server
{
	//
	// TODO: As a first pass in an attempt to keep this class as simple as 
	// possible we'll allow a little ineffeciency.  In the case of seeking
	// back into the middle of a segment and then doing a write, we'll just 
	// truncate the segment and insert the next segment, instead of trying
	// to conserve space.
	//
	// This isn't neccasarily bad, I just need to research the buffer sizes
	// a little more and figure out which is the most effecient way of
	// doing things.
	//
	
	// One fear here is StreamWriters that write single bytes at a time. This 
	// would create a long list of single byte arrays. An easy fix for this
	// would be to start using the unused space at the end of segments and
	// start making a minimum segment size, so in the case of a single byte
	// write, we would create a 24 byte buffer and set the count to 1, the
	// next write would use the remaining 23 bytes.
	//
	public class HttpResponseStream : Stream
	{
		private long segment_offset;
		private int current_segment;

		public static readonly int MIN_BUFFER_SIZE = 24;

		private List<ArraySegment<byte>> segments = new List<ArraySegment<byte>> (10);

		public HttpResponseStream ()
		{
		}
		
		public override bool CanRead {
			get { return true; }
		}
		
		
		public override bool CanSeek {
			get { return true; }
		}
		
		
		public override bool CanWrite {
			get { return true; }
		}
		
		
		public override long Length {
			get {
				if (segments.Count == 0)
					return 0;
				
				long total = segments.Sum (s => s.Count);
				return total;
			}
		}
		
		private ArraySegment<byte> CurrentSegment {
			get { return segments [current_segment]; }	
		}
		
		private bool AtEnd {
			get {
				if (segments.Count == 0)
					return true;
				
				return current_segment == segments.Count - 1 && segment_offset == CurrentSegment.Count;
			}
		}
		
		public override long Position {
			get {
				long pos = 0;
				for (int i = 0; i < current_segment; i++) {
					pos += segments [i].Count;
				}
				
				pos += segment_offset;
				return pos;
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
			if (buffer == null)
				throw new ArgumentNullException ("buffer");
			if (offset < 0)
				throw new ArgumentOutOfRangeException ("offset");
			if (count < 0)
				throw new ArgumentOutOfRangeException ("count");
			if (offset + count > buffer.Length)
				throw new ArgumentException ("The sum of offset and count is larger than the buffer length.");
			
			long read_count = 0;
			while (current_segment < segments.Count) {

				if (segment_offset == CurrentSegment.Count) {
					segment_offset = 0;
					++current_segment;
					continue;
				}
				
				long amount = Math.Min (count - read_count, CurrentSegment.Count - segment_offset);
				Array.Copy (CurrentSegment.Array, CurrentSegment.Offset + segment_offset, buffer, offset + read_count, amount);
				read_count += amount;
				segment_offset += amount;
				
				if (read_count == count)
					break;
			}
			
			return (int) read_count;
		}
		
		
		public override long Seek (long offset, SeekOrigin origin)
		{
			switch (origin) {
			case SeekOrigin.Begin:
				return SeekFromBegin (offset);
			case SeekOrigin.Current:
				return SeekFromCurrent (offset);
			case SeekOrigin.End:
				return SeekFromEnd (offset);
			}
			
			throw new ArgumentException ("Invalid SeekOrigin.");
		}
		
		private long SeekFromBegin (long offset)
		{
			if (offset < 0)
				throw new ArgumentException ("Can not seek past beginning of stream.");
			
			if (offset == 0) {
			   current_segment = 0;
			   segment_offset = 0;
			   return 0;
			}
			   
			if (segments.Count < 1)
				throw new ArgumentException ("Can not seek on empty stream.");
			
			return SeekForward (0, 0, offset);
		}
		
		private long SeekForward (int segment, long pos, long offset)
		{
			bool found = false;
			for ( ; segment < segments.Count; segment++) {
				
				if (pos + segments [segment].Count > offset) {
					found = true;
					break;
				}
				
				pos += segments [segment].Count;
			}
			
			if (!found)
				throw new ArgumentException ("Can not seek past the end of stream.");
			
			current_segment = segment;
			segment_offset = offset - pos;
			
			return Position;
		}
		
		private long SeekBackwards (int segment, long pos, long offset)
		{			
			long offset_amount = Math.Abs (offset);
			
			bool found = false;
			for ( ; segment >= 0; segment--) {

				if (pos > offset_amount){
					found = true;
					break;
				}
				
				pos += segments [segment].Count;
			}
			
			if (!found)
				throw new ArgumentException ("Can not seek past beginning of stream.");
			
			current_segment = segment;
			segment_offset = pos - offset_amount;
			
			return Position;
		}
		
		private long SeekFromEnd (long offset)
		{
			return SeekBackwards (current_segment, segment_offset, offset);
		}
		
		private long SeekFromCurrent (long offset)
		{
			if (offset == 0)
				return Position;
			
			if (offset > 0)
				return SeekForward (current_segment, segment_offset, offset);
			
			return SeekBackwards (current_segment, segment_offset, offset);
		}
		
		public override void SetLength (long value)
		{
			long len = 0;
			int segment = 0;
			
			for ( ; segment < segments.Count; segment++) {
				if (len + segments [segment].Count < value) {
					len += segments [segment].Count;
					continue;
				}
				len += segments [segment].Count;
				break;
			}
			
			if (len == value)
				return;
			
			if (len > value) {
				current_segment = segment;
				segment_offset = len - value;
				ArraySegment<byte> old = segments [segment];
				segments [segment] = new ArraySegment<byte> (old.Array, old.Offset, (int) segment_offset);
				
				while (segments.Count > segment + 1)
					segments.RemoveAt (segment + 1);
				return;
			}
			
			byte [] filler = new byte [value - len];
			segments.Add (new ArraySegment<byte> (filler, 0, filler.Length));
		}
		
		public void Insert (byte [] buffer, int offset, int count)
		{
			if (AtEnd) {
			   Write (buffer, offset, count);
			   return;
			}

			var segment = new ArraySegment<byte> (buffer, offset, count);
			segments.Insert (current_segment, segment);
			segment_offset = count;
		}

		public List<ArraySegment<byte>> GetBuffers ()
		{
			return segments;
		}

		public override void Write (byte[] buffer, int offset, int count)
		{
			if (AtEnd) {
				if (buffer.Length < MIN_BUFFER_SIZE) {
					byte [] bigger = new byte [MIN_BUFFER_SIZE];
					Array.Copy (buffer, offset, bigger, 0, count);
					buffer = bigger;
				}
				segments.Add (new ArraySegment<byte> (buffer, offset, count));
				current_segment = segments.Count - 1;
				segment_offset = count;
				return;
			}

			if (CurrentSegment.Count - segment_offset > count) {
				// There is room to stick this in the current segment.
				Array.Copy (buffer, offset, CurrentSegment.Array, segment_offset, count);
				return;
			}

			int index = current_segment;
			RemoveBytes (count);

			segments.Insert (index + 1, new ArraySegment<byte> (buffer, offset, count));
			current_segment = index + 1;
			segment_offset = count;
		}

		private void CutSegment (int segment, long position)
		{
			var s = segments [segment];

			if (position == s.Count) {
			   // At the end of the segment so nothing needs to be done.
			   return;
			}
			
			var data = new byte [s.Count - position];
			Array.Copy (s.Array, s.Offset + position, data, 0, s.Count - position);

			var after = new ArraySegment<byte> (data, 0, data.Length);
			segments.Insert (segment + 1, after);

			segments [segment] = new ArraySegment<byte> (s.Array, s.Offset, (int) position);
		}

		private void RemoveBytes (int amount)
		{
			if (CurrentSegment.Offset != segment_offset) {
			   CutSegment (current_segment, segment_offset);
			   ++current_segment;
			   segment_offset = 0;
			}

			int segment = current_segment;
			int offset = (int) segment_offset;
			int removed = 0;

			int start_segment = current_segment;
			while (segment <= segments.Count - 1) {

			      int segment_len = segments [segment].Count - offset;
			      
			      if (removed + segment_len == amount) {
				 break;
			      }

			      if (removed + segment_len > amount) {
			      	 // we need to end in this segment, truncate it by the proper amount
				 int len = segment_len - (amount - removed);
				 segments [segment] = new ArraySegment<byte> (segments [segment].Array, segments [segment].Offset, len);
				 break;
			      }

			      removed += segment_len;
			      ++segment;
			      offset = 0;
			}
			
			int num_segments = segment - start_segment;
			for (int i = 0; i < num_segments; i++) {
			    segments.RemoveAt (start_segment);
			}
		}
	}
}

