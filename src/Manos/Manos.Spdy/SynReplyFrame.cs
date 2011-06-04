using System;

namespace Manos.Spdy
{
	public class SynReplyFrame : ControlFrame
	{
		public int StreamID { get; set; }
		public NameValueHeaderBlock Headers { get; set; }
		public SynReplyFrame ()
		{
		}
		public SynReplyFrame(byte[] data, int offset, int length)
		{
			this.Type = ControlFrameType.SYN_REPLY;
			base.Parse(data, offset, length);
			this.StreamID = Util.BuildInt(data, offset + 8, 4);
			this.Headers = NameValueHeaderBlock.Parse(data, offset + 12, length - 12);
		}
	}
}

