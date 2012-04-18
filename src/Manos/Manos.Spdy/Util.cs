using System;
using System.Linq;

namespace Manos.Spdy
{
	public static class Util
	{
		public static int BuildInt (byte [] data, int offset, int length)
		{
			int ret = 0;
			for (int i = 0; i < length; i++) {
				ret += (data [offset + i] << ((length - (i + 1)) * 8));
			}
			return ret;
		}

		public static void IntToBytes (int val, ref byte [] arr, int offset, int length)
		{
			for (int i = 0; i < length; i++) {
				arr [offset + i] = Convert.ToByte ((val >> ((length - (i + 1)) * 8)) & 0xFF); //kinda gross
			}
		}

		public static byte [] Combine (params byte [][] all)
		{
			int totallength = all.Select (x => x.Length).Sum ();
			byte[] ret = new byte[totallength];
			int index = 0;
			foreach (var b in all) {
				b.CopyTo (ret, index);
				index += b.Length;
			}
			return ret;
		}
	}
}

