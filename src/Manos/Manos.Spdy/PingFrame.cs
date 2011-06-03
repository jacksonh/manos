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
		}
	}
}

