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
	}
}

