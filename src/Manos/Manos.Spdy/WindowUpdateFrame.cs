using System;

namespace Manos.Spdy
{
	public class WindowUpdateFrame : ControlFrame
	{
		public int StreamID { get; set; }
		public int DeltaWindowSize { get; set; }
		public WindowUpdateFrame ()
		{
		}
		public WindowUpdateFrame(byte[] data, int offset, int length)
		{
			this.Type = ControlFrameType.WINDOW_UPDATE;
			base.Parse(data, offset, length);
			this.StreamID = Util.BuildInt(data, offset + 8, 4);
			this.DeltaWindowSize = Util.BuildInt(data, offset + 12, 4);
		}
	}
}

