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
		}
	}
}

