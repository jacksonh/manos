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
		}
		public byte[] Serialize()
		{
			return default(byte[]);
		}
	}
}

