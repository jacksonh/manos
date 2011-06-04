using System;

namespace Manos.Spdy
{
	public class PingFrame : ControlFrame
	{
		public int ID { get; set; }
		public PingFrame()
		{
		}
		public PingFrame(byte[] data, int offset, int length)
		{
			this.Type = ControlFrameType.PING;
			base.Parse(data, offset, length);
			this.ID = Util.BuildInt(data, offset + 8, 4);
		}
	}
}

