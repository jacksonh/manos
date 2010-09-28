using System;
using NUnit.Framework;
using System.IO;
using Manos.ShouldExt;


namespace Manos.Server.Tests
{
	[TestFixture()]
	public class HttpResponseStreamTest
	{
		[Test]
		public void Write_SingleSegment_SetsLength ()
		{
			var stream = new HttpResponseStream ();
			var buffer = new byte [10];
			
			stream.Write (buffer, 0, 10);
			
			long len = stream.Length;
			Assert.AreEqual (10, len);
		}
		
		[Test]
		public void Write_OffsetSegment_SetsLength ()
		{
			var stream = new HttpResponseStream ();
			var buffer = new byte [10];
			
			stream.Write (buffer, 5, 5);
			
			long len = stream.Length;
			Assert.AreEqual (5, len);
		}
		
		[Test]
		public void Write_TruncatedSegment_SetsLength ()
		{
			var stream = new HttpResponseStream ();
			var buffer = new byte [10];
			
			stream.Write (buffer, 0, 5);
			
			long len = stream.Length;
			Assert.AreEqual (5, len);
		}
		
		[Test]
		public void Write_TwoSegments_SetsLength ()
		{
			var stream = new HttpResponseStream ();
			var buffer = new byte [10];
			
			stream.Write (buffer, 0, 10);
			stream.Write (buffer, 0, 10);
			
			long len = stream.Length;
			Assert.AreEqual (20, len);
		}
		
		[Test]
		public void Write_TwoSegments_SetsPosition ()
		{
			var stream = new HttpResponseStream ();
			var buffer = new byte [10];
			
			stream.Write (buffer, 0, 10);
			stream.Write (buffer, 0, 10);
			
			long pos = stream.Position;
			Assert.AreEqual (20, pos);
		}
		
		[Test]
		public void SeekOrigin_NegativePastBeginning_Throws ()
		{
			var stream = new HttpResponseStream ();
			var buffer = new byte [10];
			
			stream.Write (buffer, 0, 10);
			
			Should.Throw<ArgumentException> (() => stream.Seek (-1, SeekOrigin.Begin));
		}
		
		[Test]
		public void SeekOrigin_PositivePastEnd_Throws ()
		{
			var stream = new HttpResponseStream ();
			var buffer = new byte [10];
			
			stream.Write (buffer, 0, 10);
			
			Should.Throw<ArgumentException> (() => stream.Seek (11, SeekOrigin.Begin));
		}
		
		[Test]
		public void Seek_FromBeginning_SetsPosition ()
		{
			var stream = new HttpResponseStream ();
			var buffer = new byte [10];
			
			stream.Write (buffer, 0, 10);
			stream.Seek (5, SeekOrigin.Begin);
			
			long pos = stream.Position;
			Assert.AreEqual (5, pos);
		}
		
		[Test]
		public void Seek_FromBeginningMultipleSegments_SetsPosition ()
		{
			var stream = new HttpResponseStream ();
			var buffer = new byte [10];
			
			stream.Write (buffer, 0, 10);
			stream.Write (buffer, 0, 10);
			stream.Write (buffer, 0, 10);
			
			stream.Seek (25, SeekOrigin.Begin);
			
			long pos = stream.Position;
			Assert.AreEqual (25, pos);
		}
		
		[Test]
		public void Seek_FromBeginningLastIndexOfSegment_SetsPosition ()
		{
			var stream = new HttpResponseStream ();
			var buffer = new byte [10];
			
			stream.Write (buffer, 0, 10);
			stream.Write (buffer, 0, 10);
			
			stream.Seek (10, SeekOrigin.Begin);
			
			long pos = stream.Position;
			Assert.AreEqual (10, pos);
		}
		
		[Test]
		public void Seek_FromEnd_SetsPosition ()
		{
			var stream = new HttpResponseStream ();
			var buffer = new byte [10];
			
			stream.Write (buffer, 0, 10);
			stream.Seek (-3, SeekOrigin.End);
			
			long pos = stream.Position;
			Assert.AreEqual (7, pos);
		}
		
		[Test]
		public void Seek_FromEndMultipleBuffersAcrossBoundries_SetsPosition ()
		{
			var stream = new HttpResponseStream ();
			var buffer = new byte [10];
			
			stream.Write (buffer, 0, 10);
			stream.Write (buffer, 0, 10);
			stream.Write (buffer, 0, 10);
			stream.Seek (-25, SeekOrigin.End);
			
			long pos = stream.Position;
			Assert.AreEqual (5, pos);
		}
		
		[Test]
		public void Seek_FromEndMultipleBuffers_SetsPosition ()
		{
			var stream = new HttpResponseStream ();
			var buffer = new byte [10];
			
			stream.Write (buffer, 0, 10);
			stream.Write (buffer, 0, 10);
			stream.Write (buffer, 0, 10);
			stream.Seek (-5, SeekOrigin.End);
			
			long pos = stream.Position;
			Assert.AreEqual (25, pos);
		}
		
		[Test]
		public void Seek_FromEndLastIndexOfBuffer_SetsPosition ()
		{
			var stream = new HttpResponseStream ();
			var buffer = new byte [10];
			
			stream.Write (buffer, 0, 10);
			stream.Write (buffer, 0, 10);

			stream.Seek (-10, SeekOrigin.End);
			
			long pos = stream.Position;
			Assert.AreEqual (10, pos);
		}
		
		[Test]
		public void Seek_FromCurrentBackwards_SetsPosition ()
		{
			var stream = new HttpResponseStream ();
			var buffer = new byte [10];
			
			stream.Write (buffer, 0, 10);
			stream.Write (buffer, 0, 10);

			stream.Seek (-5, SeekOrigin.Current);
			
			long pos = stream.Position;
			Assert.AreEqual (15, pos);
		}
		
		[Test]
		public void Write_SeekedBackInLastSegment_SetsPosition ()
		{
			var stream = new HttpResponseStream ();
			var buffer = new byte [10];
			
			stream.Write (buffer, 0, 10);
			stream.Write (buffer, 0, 10);

			stream.Seek (-5, SeekOrigin.Current);
			stream.Write (buffer, 0, 10);
			
			long pos = stream.Position;
			Assert.AreEqual (25, pos);
		}
		
		[Test]
		public void Write_SeekedBackPastLastSegmentWriteOverAll_SetsPosition ()
		{
			var stream = new HttpResponseStream ();
			var buffer = new byte [10];
			var buffer_big = new byte [25];
			
			stream.Write (buffer, 0, 10);
			stream.Write (buffer, 0, 10);

			stream.Seek (-15, SeekOrigin.Current);
						Assert.AreEqual (5, stream.Position);

			stream.Write (buffer_big, 0, 25);
			
			long pos = stream.Position;
			Assert.AreEqual (30, pos);
		}
		
		[Test]
		public void SetLength_LessThanSingleBuffer_Truncates ()
		{
			var stream = new HttpResponseStream ();
			var buffer = new byte [10];
			
			stream.Write (buffer, 0, 10);
			stream.SetLength (5);
			
			long length = stream.Length;
			Assert.AreEqual (5, length);
		}
		
		[Test]
		public void SetLength_LessThanMultiBuffer_Truncates ()
		{
			var stream = new HttpResponseStream ();
			var buffer = new byte [10];
			
			stream.Write (buffer, 0, 10);
			stream.Write (buffer, 0, 10);
			stream.Write (buffer, 0, 10);
			
			stream.SetLength (5);
			
			long length = stream.Length;
			Assert.AreEqual (5, length);
		}
		
		[Test]
		public void SetLength_LongerThanSingleBuffer_AddsFiller ()
		{
			var stream = new HttpResponseStream ();
			var buffer = new byte [10];
			
			stream.Write (buffer, 0, 10);
			
			stream.SetLength (25);
			
			long length = stream.Length;
			Assert.AreEqual (25, length);
		}
		
		[Test]
		public void SetLength_MultiBuffer_AddsFiller ()
		{
			var stream = new HttpResponseStream ();
			var buffer = new byte [10];
			
			stream.Write (buffer, 0, 10);
			stream.Write (buffer, 0, 10);
			
			stream.SetLength (25);
			
			long length = stream.Length;
			Assert.AreEqual (25, length);
		}
		
		[Test]
		public void SetLength_EqualToCurrentLength_LengthStaysTheSame ()
		{
			var stream = new HttpResponseStream ();
			var buffer = new byte [10];
			
			stream.Write (buffer, 0, 10);
			stream.Write (buffer, 0, 10);
			
			stream.SetLength (20);
			
			long length = stream.Length;
			Assert.AreEqual (20, length);
		}
		
		[Test]
		public void Read_SingleBuffer_UpdatesPosition ()
		{
			var stream = new HttpResponseStream ();
			var buffer = new byte [10];
			
			stream.Write (buffer, 0, 10);
			stream.Seek (0, SeekOrigin.Begin);
			stream.Read (buffer, 0, 5);
			
			long position = stream.Position;
			Assert.AreEqual (5, position);
		}
		
		[Test]
		public void Read_MultipleBuffers_UpdatesPosition ()
		{
			var stream = new HttpResponseStream ();
			var buffer = new byte [10];
			
			stream.Write (buffer, 0, 10);
			stream.Write (buffer, 0, 10);
			stream.Write (buffer, 0, 10);
			
			stream.Seek (15, SeekOrigin.Begin);
			stream.Read (buffer, 0, 5);
			
			long position = stream.Position;
			Assert.AreEqual (20, position);
		}
		
		[Test]
		public void Read_ReadLastItem_UpdatesPosition ()
		{
			var stream = new HttpResponseStream ();
			var buffer = new byte [10];
			
			stream.Write (buffer, 0, 10);
			
			stream.Seek (-1, SeekOrigin.End);
			stream.Read (buffer, 0, 1);
			
			long position = stream.Position;
			Assert.AreEqual (10, position);
		}
		
		[Test]
		public void Read_PastEndOfStream_ReturnsAmountRead ()
		{
			var stream = new HttpResponseStream ();
			var buffer = new byte [10];
			
			stream.Write (buffer, 0, 10);
			
			stream.Seek (-1, SeekOrigin.End);
			int amount_read = stream.Read (buffer, 0, 2);
			
			Assert.AreEqual (1, amount_read);
		}
		
		[Test]
		public void Read_SingleBuffer_CorrectData ()
		{
			var stream = new HttpResponseStream ();
			var write_buffer = new byte [3];
			var read_buffer = new byte [1];
			
			write_buffer [2] = 0xFA;
			
			stream.Write (write_buffer, 0, 3);
			
			stream.Seek (-1, SeekOrigin.End);
			stream.Read (read_buffer, 0, 1);
			
			byte read_byte = read_buffer [0];
			Assert.AreEqual (0xFA, read_byte);
		}
		
		[Test]
		public void Read_MultipleBuffers_CorrectData ()
		{
			var stream = new HttpResponseStream ();
			var write_buffer1 = new byte [3];
			var write_buffer2 = new byte [3];
			var read_buffer = new byte [6];
			
			stream.Write (write_buffer1, 0, 3);
			
			write_buffer2 [0] = 0xFA;
			stream.Write (write_buffer2, 0, 3);
			
			stream.Seek (0, SeekOrigin.Begin);
			stream.Read (read_buffer, 0, 6);
			
			byte read_byte = read_buffer [3];
			Assert.AreEqual (0xFA, read_byte);
		}
	}
}

