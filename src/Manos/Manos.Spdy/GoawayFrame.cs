using System;

namespace Manos.Spdy
{
	public class GoawayFrame : ControlFrame
	{
		public int LastGoodStreamID { get; set; }

		public int StatusCode { get; set; }

		public GoawayFrame ()
		{
			this.Type = ControlFrameType.GOAWAY;
		}

		public GoawayFrame (byte [] data,int offset,int length)
		{
			this.Type = ControlFrameType.GOAWAY;
			base.Parse (data, offset, length);
			this.LastGoodStreamID = Util.BuildInt (data, offset + 8, 4);
			this.StatusCode = Util.BuildInt (data, offset + 12, 4);
		}

		public new byte [] Serialize ()
		{
			this.Length = 8;
			var head = base.Serialize ();
			Array.Resize (ref head, 16);
			Util.IntToBytes (this.LastGoodStreamID, ref head, 8, 4);
			Util.IntToBytes (this.StatusCode, ref head, 12, 4);
			return head;
		}
	}
}

