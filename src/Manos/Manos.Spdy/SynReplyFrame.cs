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

		public SynReplyFrame (byte [] data,int offset,int length,InflatingZlibContext inflate)
		{
			this.Type = ControlFrameType.SYN_REPLY;
			base.Parse (data, offset, length);
			this.StreamID = Util.BuildInt (data, offset + 8, 4);
			this.Headers = NameValueHeaderBlock.Parse (data, offset + 14, length - 12, inflate); //14 because of 2 unused bytes
		}

		public byte [] Serialize (DeflatingZlibContext deflate)
		{
			byte[] nvblock = this.Headers.Serialize (deflate);
			this.Length = nvblock.Length + 6;
			var header = base.Serialize ();
			byte[] middle = new byte[6];
			Util.IntToBytes (this.StreamID, ref middle, 0, 4);
			return Util.Combine (header, middle, nvblock);
		}
	}
}

