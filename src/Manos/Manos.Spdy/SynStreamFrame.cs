using System;

namespace Manos.Spdy
{
	public class SynStreamFrame : ControlFrame
	{
		public int StreamID { get; set; }
		public int AssociatedToStreamID { get; set; }
		public int Priority { get; set; }
		public NameValueHeaderBlock Headers { get; set; }
		public SynStreamFrame ()
		{
		}
		public SynStreamFrame(byte[] data, int offset, int length)
		{
			this.Type = ControlFrameType.SYN_STREAM;
			base.Parse(data, offset, length);
			this.StreamID = Util.BuildInt(data, offset + 8, 4);
			this.AssociatedToStreamID = Util.BuildInt(data, offset + 12, 4);
			this.Priority = data[16] >> 5;
			this.Headers = NameValueHeaderBlock.Parse(data, 18, this.Length - 10);
		}
	}
}

