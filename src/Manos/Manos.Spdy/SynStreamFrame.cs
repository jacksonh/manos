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
			this.Type = ControlFrameType.SYN_STREAM;
		}

		public SynStreamFrame (byte [] data,int offset,int length,InflatingZlibContext inflate)
		{
			this.Type = ControlFrameType.SYN_STREAM;
			base.Parse (data, offset, length);
			this.StreamID = Util.BuildInt (data, offset + 8, 4);
			this.AssociatedToStreamID = Util.BuildInt (data, offset + 12, 4);
			this.Priority = data [16] >> 5;
			this.Headers = NameValueHeaderBlock.Parse (data, 18, this.Length - 10, inflate);
		}

		public byte [] Serialize (DeflatingZlibContext deflate)
		{
			byte[] nvblock = this.Headers.Serialize (deflate);
			this.Length = nvblock.Length + 10;
			var header = base.Serialize ();
			byte[] middle = new byte[10];
			Util.IntToBytes (this.StreamID, ref middle, 0, 4);
			Util.IntToBytes (this.AssociatedToStreamID, ref middle, 4, 4);
			middle [8] = (byte) (this.Priority << 5);
			return Util.Combine (header, middle, nvblock);
		}
	}
}

