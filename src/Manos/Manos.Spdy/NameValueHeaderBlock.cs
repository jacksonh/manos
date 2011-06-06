using System;
using System.Collections.Specialized;
using System.Text;

namespace Manos.Spdy
{
	public class NameValueHeaderBlock : NameValueCollection
	{
		public NameValueHeaderBlock ()
		{
		}
		public static NameValueHeaderBlock Parse(byte[] data, int offset, int length)
		{
			byte[] def = new byte[0];
			NameValueHeaderBlock ret = new NameValueHeaderBlock();
			int len = Compression.Inflate(data, offset, length, out def);
			int NumberPairs = Util.BuildInt(def, 0, 4);
			int index = 4;
			while (NumberPairs-- >= 0)
			{
				int namelength = Util.BuildInt(def, index, 4);
				index +=4;
				string name = Encoding.UTF8.GetString(def, index, namelength);
				index += namelength;
				int vallength = Util.BuildInt(def, index, 4);
				index += 4;
				string vals = Encoding.UTF8.GetString(def, index, vallength);
				index += vallength;
				string[] splitvals = vals.Split(char.MinValue);
				foreach (var str in splitvals)
				{
					ret.Add(name, str);
				}
			}
			return ret;
		}
		public byte[] UncompressedSerialize()
		{
			int arrlen = 4;
			// Shouldn't iterate twice?
			// If I have to resize the array at the end, then that'll be O(n) in itself
			// So either way, its O(2n) (I think)
			foreach (var key in this.AllKeys)
			{
				arrlen += 4;
				arrlen += key.Length;
				string val = this[key];
				arrlen += 4;
				arrlen += val.Length;
			}
			byte[] ret = new byte[arrlen];
			Util.IntToBytes(this.Count, ref ret, 0, 4);
			int index = 4;
			foreach (var key in this.AllKeys)
			{
				Util.IntToBytes(key.Length, ref ret, index, 4);
				index += 4;
				foreach (char c in key)
				{
					ret[index] = (byte)c;
					index++;
				}
				Util.IntToBytes(this[key].Length, ref ret, index, 4);
				index += 4;
				string vals = this[key].Replace(' ', char.MinValue);
				foreach (char c in vals)
				{
					ret[index] = (byte)c;
					index ++;
				}
			}
			return ret;
		}
		public byte[] Serialize()
		{
			byte[] inarr = this.UncompressedSerialize();
			byte[] outarr = new byte[0];
			var len = Compression.Deflate(inarr, 0, inarr.Length, out outarr);
			Array.Resize(ref outarr, len);
			return outarr;
		}
	}
}

