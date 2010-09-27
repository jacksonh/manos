

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
	
	public class HttpResponseStream : Stream
	{
		private long segment_offset;
		private int current_segment;

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
			throw new System.NotImplementedException();
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
		
		
		public override void Write (byte[] buffer, int offset, int count)
		{
			if (AtEnd) {
				segments.Add (new ArraySegment<byte> (buffer, offset, count));
				current_segment = segments.Count - 1;
				segment_offset = count;
				return;
			}
			
			// Snip the current segment off and insert a new one.
			segments [current_segment] = new ArraySegment<byte> (CurrentSegment.Array, CurrentSegment.Offset, (int) segment_offset);
			
			if (current_segment == segments.Count - 1) {
				segments.Add (new ArraySegment<byte> (buffer, offset, count));
				current_segment = segments.Count - 1;
				segment_offset = count;
			} else {
				segments.Insert (current_segment + 1, new ArraySegment<byte> (buffer, offset, count));
				current_segment = current_segment + 1;
				segment_offset = count;
			}     
		}
	}
}

