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
			this.Type = ControlFrameType.SETTINGS;
			base.Parse(data, offset, length);
			int numentries = Util.BuildInt(data, offset + 8, 4);
			int index = offset + 12;
			for (int i = 0; i < numentries; i++)
			{
				byte IDFlags = data[index];
				index++;
				int ID = Util.BuildInt(data, index, 3);
				index += 3;
				int val = Util.BuildInt(data, index, 4);
				switch (ID)
				{
				case 1:
					this.UploadBandwidth = val;
					break;
				case 2:
					this.DownloadBandwidth = val;
					break;
				case 3:
					this.RoundTripTime = val;
					break;
				case 4:
					this.MaxConcurrentStreams = val;
					break;
				case 5:
					this.CWND = val;
					break;
				}
			}
		}
		public int UploadBandwidth { get; set; }
		public int DownloadBandwidth { get; set; }
		public int RoundTripTime { get; set; }
		public int MaxConcurrentStreams { get; set; }
		public int CWND { get; set; }
	}
}

