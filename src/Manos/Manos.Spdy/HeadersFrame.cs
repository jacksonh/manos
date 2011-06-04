using System;

namespace Manos.Spdy
{
	public class HeadersFrame : ControlFrame
	{
		public int StreamID { get; set; }
		public NameValueHeaderBlock Headers { get; set; }
		public HeadersFrame ()
		{
		}
		public HeadersFrame(byte[] data, int offset, int length)
		{
			this.Type = ControlFrameType.HEADERS;
			base.Parse(data, offset, length);
			this.StreamID = Util.BuildInt(data, offset + 8, 4);
			this.Headers = NameValueHeaderBlock.Parse(data, offset + 12, length - 12);
		}
	}
}

