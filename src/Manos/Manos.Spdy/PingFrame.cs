using System;

namespace Manos.Spdy
{
	public class PingFrame : ControlFrame
	{
		public int ID { get; set; }

		public PingFrame ()
		{
			this.Type = ControlFrameType.PING;
		}

		public PingFrame (byte [] data,int offset,int length)
		{
			this.Type = ControlFrameType.PING;
			base.Parse (data, offset, length);
			this.ID = Util.BuildInt (data, offset + 8, 4);
		}

		public new byte [] Serialize ()
		{
			this.Length = 4;
			var headers = base.Serialize ();
			Array.Resize (ref headers, 12);
			Util.IntToBytes (this.ID, ref headers, 8, 4);
			return headers;
		}
	}
}

