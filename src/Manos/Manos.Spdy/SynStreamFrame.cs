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
	}
}

