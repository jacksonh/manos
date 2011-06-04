using System;

namespace Manos.Spdy
{
	public class GoawayFrame : ControlFrame
	{
		public int LastGoodStreamID { get; set; }
		public int StatusCode { get; set; }
		public GoawayFrame ()
		{
		}
		public GoawayFrame(byte[] data, int offset, int length)
		{
			this.Type = ControlFrameType.GOAWAY;
			base.Parse(data, offset, length);
			this.LastGoodStreamID = Util.BuildInt(data, offset + 8, 4);
			this.StatusCode = Util.BuildInt(data, offset + 12, 4);
		}
	}
}

