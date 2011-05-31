using System;
using System.Collections.Specialized;

namespace Manos.Spdy
{
	public class NameValueHeaderBlock : NameValueCollection
	{
		public NameValueHeaderBlock ()
		{
		}
		public static NameValueHeaderBlock Parse(byte[] data, int offset, int length)
		{
			return null;
		}
		public byte[] UncompressedSerialize()
		{
			return default(byte[]);
		}
	}
}

