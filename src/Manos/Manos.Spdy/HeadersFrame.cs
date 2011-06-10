using System;

namespace Manos.Spdy
{
	public class HeadersFrame : ControlFrame
	{
		public int StreamID { get; set; }
		public NameValueHeaderBlock Headers { get; set; }
		public HeadersFrame ()
		{
			this.Type = ControlFrameType.HEADERS;
		}
		public HeadersFrame(byte[] data, int offset, int length, InflatingZlibContext inflate)
		{
			this.Type = ControlFrameType.HEADERS;
			base.Parse(data, offset, length);
			this.StreamID = Util.BuildInt(data, offset + 8, 4);
			this.Headers = NameValueHeaderBlock.Parse(data, offset + 12, length - 12, inflate);
		}
		public byte[] Serialize(DeflatingZlibContext deflate)
		{
			byte[] nvblock = this.Headers.Serialize(deflate);
			this.Length = nvblock.Length + 4;
			var header = base.Serialize();
			byte[] middle = new byte[4];
			Util.IntToBytes(this.StreamID, ref middle, 0, 4);
			return Util.Combine(header, middle, nvblock);
		}
	}
}

