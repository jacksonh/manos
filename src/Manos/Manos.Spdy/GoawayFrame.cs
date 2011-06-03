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
		}
	}
}

