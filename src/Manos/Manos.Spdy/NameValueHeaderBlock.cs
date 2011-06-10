using System;
using System.Collections.Specialized;
using System.Text;
using Manos.Http;

namespace Manos.Spdy
{
	public class NameValueHeaderBlock : NameValueCollection
	{
		public NameValueHeaderBlock ()
		{
		}
		public static NameValueHeaderBlock Parse(byte[] data, int offset, int length, InflatingZlibContext inflate)
		{
			int bytelength = 2; //for version 2, changes to 4 in version 3
			byte[] def = new byte[0];
			NameValueHeaderBlock ret = new NameValueHeaderBlock();
			int len = inflate.Inflate(data, offset, length, out def);
			int NumberPairs = Util.BuildInt(def, 0, bytelength);
			int index = bytelength;
			while (NumberPairs-- >= 0)
			{
				int namelength = Util.BuildInt(def, index, bytelength);
				index +=bytelength;
				string name = Encoding.UTF8.GetString(def, index, namelength);
				index += namelength;
				int vallength = Util.BuildInt(def, index, bytelength);
				index += bytelength;
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
			int bytelength = 2;
			int arrlen = bytelength;
			// Shouldn't iterate twice?
			// If I have to resize the array at the end, then that'll be O(n) in itself
			// So either way, its O(2n) (I think)
			foreach (var key in this.AllKeys)
			{
				arrlen += bytelength;
				arrlen += key.Length;
				string val = this[key];
				arrlen += bytelength;
				arrlen += val.Length;
			}
			byte[] ret = new byte[arrlen];
			Util.IntToBytes(this.Count, ref ret, 0, bytelength);
			int index = bytelength;
			foreach (var key in this.AllKeys)
			{
				Util.IntToBytes(key.Length, ref ret, index, bytelength);
				index += bytelength;
				foreach (char c in key)
				{
					ret[index] = (byte)c;
					index++;
				}
				Util.IntToBytes(this[key].Length, ref ret, index, bytelength);
				index += bytelength;
				string vals = this[key].Replace(' ', char.MinValue);
				foreach (char c in vals)
				{
					ret[index] = (byte)c;
					index ++;
				}
			}
			return ret;
		}
		public byte[] Serialize(DeflatingZlibContext deflate)
		{
			byte[] inarr = this.UncompressedSerialize();
			byte[] outarr = new byte[0];
			var len = deflate.Deflate(inarr, 0, inarr.Length, out outarr);
			Array.Resize(ref outarr, len);
			return outarr;
		}
		public HttpHeaders ToHttpHeaders(string[] exclude)
		{
			HttpHeaders h = new HttpHeaders();
			foreach (var key in this.AllKeys)
			{
				foreach (var str in exclude)
				{
					if (str == key)
					{
						continue;
					}
				}
				if (!string.IsNullOrEmpty(key)) {
					h.SetHeader(key, this[key]);
				}
			}
			return h;
		}
	}
}

