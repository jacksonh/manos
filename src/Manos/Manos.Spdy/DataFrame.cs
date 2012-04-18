using System;

namespace Manos.Spdy
{
	public class DataFrame
	{
		public int StreamID { get; set; }

		public byte Flags { get; set; }

		public int Length { get; set; }

		public byte [] Data { get; set; }

		public DataFrame ()
		{
		}

		public DataFrame (byte [] data,int offset,int length)
		{
			this.StreamID = Util.BuildInt (data, offset, 4);
			this.Flags = data [offset + 4];
			this.Length = Util.BuildInt (data, offset + 5, 3);
			this.Data = new byte[this.Length];
			Array.Copy (data, offset + 8, this.Data, 0, this.Length);
		}

		public byte [] Serialize ()
		{
			byte[] ret = new byte[8 + this.Data.Length];
			Util.IntToBytes (this.StreamID, ref ret, 0, 4);
			ret [4] = this.Flags;
			Util.IntToBytes (this.Data.Length, ref ret, 5, 3);
			Array.Copy (this.Data, 0, ret, 8, this.Data.Length);
			return ret;
		}

		public byte [] SerializeHeader ()
		{
			byte[] ret = new byte[8];
			Util.IntToBytes (this.StreamID, ref ret, 0, 4);
			ret [4] = this.Flags;
			Util.IntToBytes (this.Length, ref ret, 5, 3);
			return ret;
		}
	}
}

