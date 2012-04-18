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
			byte[] output = (new InflatingZlibContext()).Inflate(fromclass, 18, frame.Length - 10);
			int declength = output.Length;
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
			byte[] output = (new InflatingZlibContext()).Inflate(fromclass, 12, frame.Length - 4);
			int declength = output.Length;
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
			byte[] fromclass = frame.Serialize();
			Assert.AreEqual(16, fromclass.Length, "Lengths"); //first 8 is rst_stream length always, 2nd 8 is for control frame header
			Assert.AreEqual(0x80, fromclass[0], "Control Bit");
			Assert.AreEqual(0x02, fromclass[1], "Version");
			Assert.AreEqual(0x03, fromclass[3], "Frame Type"); //skip 2 because type is two bits
			Assert.AreEqual(0x00, fromclass[4], "Flags");
			// 5,6,7 are Length, Will add test once I actually implement packet generation
			Assert.AreEqual(0x01, fromclass[10], "Stream ID");
			Assert.AreEqual(0x55, fromclass[11], "Stream ID"); //8, 9, 10, 11 -> Stream ID
			Assert.AreEqual(0x03, fromclass[15], "Status Code"); //12, 13, 14, 15
		}
		[Test]
		public void GenerateSettings()
		{
			SettingsFrame frame = new SettingsFrame();
			frame.Version = 2;
			frame.Flags = 0x00;
			frame.UploadBandwidth = 255;
			frame.MaxConcurrentStreams = 240;
			byte[] fromclass = frame.Serialize();
			Assert.AreEqual(fromclass.Length, 8 + frame.Length, "Frame Length");
			Assert.AreEqual(0x80, fromclass[0], "Control Bit");
			Assert.AreEqual(0x02, fromclass[1], "Version");
			Assert.AreEqual(0x04, fromclass[3], "Frame Type"); //skip 2 because type is two bits
			Assert.AreEqual(0x00, fromclass[4], "Flags");
			// 5,6,7 are Length, Will add test once I actually implement packet generation
			Assert.AreEqual(0x02, fromclass[11], "Number of Entries"); // 8, 9, 10, 11
			Assert.AreEqual(0x01, fromclass[12], "Entry 1 persist flag"); //First byte is flags (12)
			Assert.AreEqual(0x01, fromclass[15], "Entry 1 ID"); //last 3 bytes are ID (13, 14, 15)
			Assert.AreEqual(0xFF, fromclass[19], "Entry 1 Value"); //16, 17, 18, 19
			Assert.AreEqual(0x01, fromclass[20], "Entry 2 persist flag"); //First byte is flags (20)
			Assert.AreEqual(0x04, fromclass[23], "Entry 2 ID"); //last 3 bytes are ID (21, 22, 23)
			Assert.AreEqual(0xF0, fromclass[27], "Entry 2 Value"); //24, 25, 26, 27
		}
		[Test]
		public void GeneratePing()
		{
			PingFrame frame = new PingFrame();
			frame.Version = 2;
			frame.Flags = 0x00;
			frame.ID = 17;
			byte[] fromclass = frame.Serialize();
			Assert.AreEqual(0x80, fromclass[0], "Control Bit");
			Assert.AreEqual(0x02, fromclass[1], "Version");
			Assert.AreEqual(0x06, fromclass[3], "Frame Type"); //skip 2 because type is two bits
			Assert.AreEqual(0x00, fromclass[4], "Flags");
			Assert.AreEqual(0x04, fromclass[7], "Length"); // 5, 6, 7
			Assert.AreEqual(0x11, fromclass[11], "ID"); // 8, 9, 10, 11
		}
		[Test]
		public void GenerateGoAway()
		{
			GoawayFrame frame = new GoawayFrame();
			frame.Version = 2;
			frame.Flags = 0x00;
			frame.LastGoodStreamID = 7;
			frame.StatusCode = 17;
			byte[] fromclass = frame.Serialize();
			Assert.AreEqual(0x80, fromclass[0], "Control Bit");
			Assert.AreEqual(0x02, fromclass[1], "Version");
			Assert.AreEqual(0x07, fromclass[3], "Frame Type"); //skip 2 because type is two bits
			Assert.AreEqual(0x00, fromclass[4], "Flags");
			Assert.AreEqual(0x08, fromclass[7], "Length"); //5, 6, 7
			Assert.AreEqual(0x07, fromclass[11], "Last Good Stream ID"); // 8, 9, 10, 11
			Assert.AreEqual(0x11, fromclass[15], "Status Code"); //12, 13, 14, 15
		}
		[Test]
		public void GenerateHeaders()
		{
			HeadersFrame frame = new HeadersFrame();
			frame.Version = 2;
			frame.Flags = 0x00;
			frame.StreamID = 27;
			NameValueHeaderBlock headers = new NameValueHeaderBlock();
			headers.Add("Header1", "Value1");
			headers.Add("Header2", "Value2");
			frame.Headers = headers;
			byte[] fromclass = frame.Serialize();
			Assert.AreEqual(fromclass.Length, frame.Length + 8, "Lengths"); //8 is for control frame header
			Assert.AreEqual(0x80, fromclass[0], "Control Bit");
			Assert.AreEqual(0x02, fromclass[1], "Version");
			Assert.AreEqual(0x08, fromclass[3], "Frame Type"); //skip 2 because type is two bits
			Assert.AreEqual(0x00, fromclass[4], "Flags");
			// 5,6,7 are Length, Will add test once I actually implement packet generation
			Assert.AreEqual(0x1B, fromclass[11], "Stream ID"); //8, 9, 10, 11 -> Stream ID
			byte[] output = (new InflatingZlibContext()).Inflate(fromclass, 12 , frame.Length - 4);
			int declength = output.Length;
			byte[] nv = headers.UncompressedSerialize();
			for (int i = 0; i < declength; i++)
			{
				Assert.AreEqual(nv[i], output[i], "Name Value Block Byte #" + i);
			}
		}
		[Test]
		public void GenerateWindowUpdate()
		{
			WindowUpdateFrame frame = new WindowUpdateFrame();
			frame.Version = 2;
			frame.Flags = 0x00;
			frame.StreamID = 30;
			frame.DeltaWindowSize = 82;
			byte[] fromclass = frame.Serialize();
			Assert.AreEqual(0x80, fromclass[0], "Control Bit");
			Assert.AreEqual(0x02, fromclass[1], "Version");
			Assert.AreEqual(0x09, fromclass[3], "Frame Type"); //skip 2 because type is two bits
			Assert.AreEqual(0x00, fromclass[4], "Flags");
			Assert.AreEqual(0x08, fromclass[7], "Length"); // 5, 6, 7
			Assert.AreEqual(0x1E, fromclass[11], "Stream ID"); // 8, 9, 10, 11
			Assert.AreEqual(0x52, fromclass[15], "Delta Window Size"); // 12, 13, 14, 15
		}
		[Test]
		public void GenerateVersion()
		{
			VersionFrame frame = new VersionFrame();
			frame.Version = 1;
			frame.Flags = 0x00;
			frame.SupportedVersions = new int[] { 2, 3 };
			byte[] fromclass = frame.Serialize();
			Assert.AreEqual(0x80, fromclass[0], "Control Bit");
			Assert.AreEqual(0x01, fromclass[1], "Version");
			Assert.AreEqual(0x0A, fromclass[3], "Frame Type"); //skip 2 because type is two bits
			Assert.AreEqual(0x00, fromclass[4], "Flags");
			Assert.AreEqual(0x08, fromclass[7], "Length"); // 5, 6, 7
			Assert.AreEqual(0x02, fromclass[11], "Number of Supported Versions"); //8, 9, 10, 11
			Assert.AreEqual(0x02, fromclass[13], "Supported Version 1 (2)"); // 12, 13
			Assert.AreEqual(0x03, fromclass[15], "Supported Version 2 (3)"); // 14, 15
		}
		[Test]
		public void GenerateData()
		{
			DataFrame frame = new DataFrame();
			frame.Flags = 0x00;
			frame.StreamID = 27;
			frame.Data = new byte[] { 0x43, 0x22, 0x99 };
			byte[] fromclass = frame.Serialize();
			Assert.AreEqual(0x00, fromclass[0], "Not a Control Frame");
			Assert.AreEqual(0x1B, fromclass[3], "Stream ID");
			Assert.AreEqual(0x00, fromclass[4], "Flags");
			Assert.AreEqual(0x03, fromclass[7], "Length");
			Assert.AreEqual(0x43, fromclass[8], "Data 1");
			Assert.AreEqual(0x22, fromclass[9], "Data 2");
			Assert.AreEqual(0x99, fromclass[10], "Data 3");
		}
	}
}

