using System;

namespace Manos.Spdy
{
	public class DataFrame
	{
		public int StreamID { get; set; }
		public byte Flags { get; set; }
		public int Length { get; set; }
		public byte[] Data { get; set; }
		public DataFrame ()
		{
		}
		public DataFrame(byte[] data, int offset, int length)
		{
			this.StreamID = Util.BuildInt(data, offset, 4);
			this.Flags = data[offset + 4];
			this.Length = Util.BuildInt(data, offset + 5, 3);
			this.Data = new byte[this.Length];
			Array.Copy(data, offset + 8, this.Data, 0, this.Length);
		}
		public byte[] Serialize()
		{
			return default(byte[]);
		}
	}
}

