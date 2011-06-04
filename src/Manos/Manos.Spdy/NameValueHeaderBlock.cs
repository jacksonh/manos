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
			return default(byte[]);
		}
	}
}

