using System;

namespace Manos.Spdy
{
	public class WindowUpdateFrame : ControlFrame
	{
		public int StreamID { get; set; }

		public int DeltaWindowSize { get; set; }

		public WindowUpdateFrame ()
		{
			this.Type = ControlFrameType.WINDOW_UPDATE;
		}

		public WindowUpdateFrame (byte [] data,int offset,int length)
		{
			this.Type = ControlFrameType.WINDOW_UPDATE;
			base.Parse (data, offset, length);
			this.StreamID = Util.BuildInt (data, offset + 8, 4);
			this.DeltaWindowSize = Util.BuildInt (data, offset + 12, 4);
		}

		public new byte [] Serialize ()
		{
			this.Length = 8;
			var headers = base.Serialize ();
			Array.Resize (ref headers, 16);
			Util.IntToBytes (this.StreamID, ref headers, 8, 4);
			Util.IntToBytes (this.DeltaWindowSize, ref headers, 12, 4);
			return headers;
		}
	}
}

