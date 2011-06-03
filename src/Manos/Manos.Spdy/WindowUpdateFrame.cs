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
		}
	}
}

