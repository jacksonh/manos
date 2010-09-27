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
	}
}

