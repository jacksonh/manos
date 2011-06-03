using System;

namespace Manos.Spdy
{
	public class SettingsFrame : ControlFrame
	{
		public SettingsFrame ()
		{
		}
		public SettingsFrame(byte[] data, int offset, int length)
		{
		}
		public int UploadBandwidth { get; set; }
		public int DownloadBandwidth { get; set; }
		public int RoundTripTime { get; set; }
		public int MaxConcurrentStreams { get; set; }
		public int CWND { get; set; }
	}
}

