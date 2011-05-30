using System;

namespace Manos.Spdy
{
	public class VersionFrame : ControlFrame
	{
		public int SupportedVersionsCount { get; set; }
		public int[] SupportedVersions { get; set; }
		public VersionFrame ()
		{
		}
	}
}

