using System;

namespace Manos.Spdy
{
	public class SynReplyFrame : ControlFrame
	{
		public int StreamID { get; set; }
		public NameValueHeaderBlock Headers { get; set; }
		public SynReplyFrame ()
		{
		}
		public new void Parse(byte[] data, int offset, int length)
		{
			base.Parse(data, offset, length);
		}
	}
}

