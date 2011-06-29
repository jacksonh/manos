using System;

namespace Manos.Spdy
{
	public abstract class ControlFrame
	{
		public int Version { get; set; }

		public ControlFrameType Type { get; set; }

		public byte Flags { get; set; }

		public int Length { get; set; }

		private byte [] FrameData { get; set; }

		public ControlFrame ()
		{
		}

		public void Parse (byte [] data, int offset, int length)
		{
			this.Version = data [offset + 1];
			this.Flags = data [offset + 4];
			this.Length = Util.BuildInt (data, offset + 5, 3);
		}

		public byte [] Serialize ()
		{
			byte[] header = new byte[8];
			header [0] = (byte) (0x80 | ((Version >> 8) & 0xff));
			header [1] = (byte) (this.Version & 0xff);
			header [2] = (byte) ((int) this.Type >> 8);
			header [3] = (byte) this.Type;
			header [4] = this.Flags;
			header [5] = (byte) ((this.Length >> 16) & 0xFF);
			header [6] = (byte) ((this.Length >> 8) & 0xFF);
			header [7] = (byte) (this.Length & 0xFF);
			return header;
		}
	}

	public enum ControlFrameType
	{
		SYN_STREAM = 1,
		SYN_REPLY = 2,
		RST_STREAM = 3,
		SETTINGS = 4,
		PING = 6,
		GOAWAY = 7,
		HEADERS = 8,
		WINDOW_UPDATE = 9,
		VERSION = 10
	}
}

