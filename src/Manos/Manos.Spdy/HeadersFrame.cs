using System;

namespace Manos.Spdy
{
	public class HeadersFrame : ControlFrame
	{
		public int StreamID { get; set; }
		public NameValueHeaderBlock Headers { get; set; }
		public HeadersFrame ()
		{
		}
		public HeadersFrame(byte[] data, int offset, int length)
		{
		}
	}
}

