using System;

namespace Manos.Spdy
{
	public abstract class ControlFrame
	{
		public int Version { get; set; }
		public ControlFrameType Type { get; set; }
		public byte Flags { get; set; }
		public int Length { get; set; }
		private byte[] FrameData { get; set;}
		public ControlFrame ()
		{
		}
		public void Parse(byte[] data, int offset, int length)
		{
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

