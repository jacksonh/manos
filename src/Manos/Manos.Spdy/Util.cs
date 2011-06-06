using System;

namespace Manos.Spdy
{
	public static class Util
	{
		public static int BuildInt(byte[] data, int offset, int length)
		{
			int ret = 0;
			for (int i = 0; i < length; i++)
			{
				ret += (data[offset + i] << ((length-(i+1))*8));
			}
			return ret;
		}
		public static void IntToBytes(int val, ref byte[] arr, int offset, int length)
		{
			for (int i = 0; i < length; i++)
			{
				arr[offset + i] = Convert.ToByte((val >> ((length-(i + 1))*8)) & 0xFF); //kinda gross
			}
		}
	}
}

