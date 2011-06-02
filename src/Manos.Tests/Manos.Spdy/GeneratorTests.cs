using System;
using NUnit.Framework;
using Manos.Spdy;

namespace Manos.Spdy.Tests
{
	[TestFixture]
	public class GeneratorTests
	{
		[SetUp]
		public void SetUp()
		{
		}
		[Test]
		public void GenerateSynStream ()
		{
			SynStreamFrame frame = new SynStreamFrame();
			frame.Version = 2;
			//frame.Length auto generated
			frame.StreamID = 27;
			frame.AssociatedToStreamID = 1;
			frame.Priority = 1;
			NameValueHeaderBlock headers = new NameValueHeaderBlock();
			headers.Add("header1", "value1");
			headers.Add("header2", "value2");
			frame.Headers = headers;
			byte[] fromclass = frame.Serialize();
			Assert.AreEqual(fromclass.Length, frame.Length + 8, "Lengths"); //8 is for control frame header
			Assert.AreEqual(0x80, fromclass[0], "Control Bit");
			Assert.AreEqual(0x02, fromclass[1], "Version");
			Assert.AreEqual(0x01, fromclass[3], "Frame Type"); //skip 2 because type is two bits
			Assert.AreEqual(0x00, fromclass[4], "Flags");
			// 5,6,7 are Length, Will add test once I actually implement packet generation
			Assert.AreEqual(0x1B, fromclass[11], "Stream ID"); //8, 9, 10, 11 -> Stream ID
			Assert.AreEqual(0x01, fromclass[15], "Associated To Stream ID"); // 12, 13, 14, 15 -> Assoc Stream ID
			Assert.AreEqual(0x20, fromclass[16], "Priority"); // 17 is unused
			byte[] output = new byte[0];
			int declength = Compression.Inflate(fromclass, 18, frame.Length - 10, out output);
			byte[] nv = headers.UncompressedSerialize();
			for (int i = 0; i < declength; i++)
			{
				Assert.AreEqual(nv[i], output[i], "Name Value Block Byte #" + i);
			}
		}
		[Test]
		public void GenerateSynReply()
		{
			SynReplyFrame frame = new SynReplyFrame();
			frame.Version = 2;
			frame.Flags = 0x00;
			frame.StreamID = 2;
			NameValueHeaderBlock headers = new NameValueHeaderBlock();
			headers.Add("header1", "value1");
			headers.Add("header2", "value2");
			frame.Headers = headers;
			byte[] fromclass = frame.Serialize();
			Assert.AreEqual(fromclass.Length, frame.Length + 8, "Lengths"); //8 is for control frame header
			Assert.AreEqual(0x80, fromclass[0], "Control Bit");
			Assert.AreEqual(0x02, fromclass[1], "Version");
			Assert.AreEqual(0x02, fromclass[3], "Frame Type"); //skip 2 because type is two bits
			Assert.AreEqual(0x00, fromclass[4], "Flags");
			// 5,6,7 are Length, Will add test once I actually implement packet generation
			Assert.AreEqual(0x02, fromclass[11], "Stream ID"); //8, 9, 10, 11 -> Stream ID
			byte[] output = new byte[0];
			int declength = Compression.Inflate(fromclass, 12, frame.Length - 4, out output);
			byte[] nv = headers.UncompressedSerialize();
			for (int i = 0; i < declength; i++)
			{
				Assert.AreEqual(nv[i], output[i], "Name Value Block Byte #" + i);
			}
		}
		[Test]
		public void GenerateRstStream()
		{
			RstStreamFrame frame = new RstStreamFrame();
			frame.Version = 2;
			frame.Flags = 0x00;
			frame.StreamID = 341;
			frame.StatusCode = RstStreamStatusCode.REFUSED_STREAM;
			byte fromclass = frame.Serialize();
			Assert.AreEqual(fromclass.Length, 8 + 8, "Lengths"); //first 8 is rst_stream length always, 2nd 8 is for control frame header
			Assert.AreEqual(0x80, fromclass[0], "Control Bit");
			Assert.AreEqual(0x02, fromclass[1], "Version");
			Assert.AreEqual(0x03, fromclass[3], "Frame Type"); //skip 2 because type is two bits
			Assert.AreEqual(0x00, fromclass[4], "Flags");
			// 5,6,7 are Length, Will add test once I actually implement packet generation
			Assert.AreEqual(0x01, fromclass[10], "Stream ID");
			Assert.AreEqual(0x55, fromclass[11], "Stream ID"); //8, 9, 10, 11 -> Stream ID
			Assert.AreEqual(0x03, fromclass[15], "Status Code"); //12, 13, 14, 15
		}
	}
}

