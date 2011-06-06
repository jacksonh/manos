using System;

namespace Manos.Spdy
{
	public class SynReplyFrame : ControlFrame
	{
		public int StreamID { get; set; }
		public NameValueHeaderBlock Headers { get; set; }
		public SynReplyFrame ()
		{
			this.Type = ControlFrameType.SYN_REPLY;
		}
		public SynReplyFrame(byte[] data, int offset, int length)
		{
			this.Type = ControlFrameType.SYN_REPLY;
			base.Parse(data, offset, length);
			this.StreamID = Util.BuildInt(data, offset + 8, 4);
			this.Headers = NameValueHeaderBlock.Parse(data, offset + 12, length - 12);
		}
		public new byte[] Serialize()
		{
			byte[] nvblock = this.Headers.Serialize();
			this.Length = nvblock.Length + 4;
			var header = base.Serialize();
			byte[] middle = new byte[4];
			Util.IntToBytes(this.StreamID, ref middle, 0, 4);
			return Util.Combine(header, middle, nvblock);
		}
	}
}

