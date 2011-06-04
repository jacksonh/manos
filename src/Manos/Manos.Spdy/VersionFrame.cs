using System;

namespace Manos.Spdy
{
	public class VersionFrame : ControlFrame
	{
		public int[] SupportedVersions { get; set; }
		public VersionFrame ()
		{
		}
		public VersionFrame(byte[] data, int offset, int length)
		{
			this.Type = ControlFrameType.VERSION;
			base.Parse(data, offset, length);
			int versionscount = Util.BuildInt(data, offset + 8, 4);
			int index = 12;
			this.SupportedVersions = new int[versionscount];
			for (int i = 0; i < versionscount; i++)
			{
				SupportedVersions[i] = Util.BuildInt(data, index, 2);
				index += 2;
			}
		}
	}
}

