using System;

namespace Manos.Spdy
{
	public class SettingsFrame : ControlFrame
	{
		private int[] Settings { get; set; }
		public bool Persist
		{
			get
			{
				return (this.Flags & 0x1) == 1;
			}
			set
			{
				if (value)
					this.Flags |= 0x1;
				else
					this.Flags ^= 0x1;
			}
		}
		public bool Persisted
		{
			get
			{
				return (this.Flags & 0x2) == 1;
			}
			set
			{
				if (value)
					this.Flags |= 0x2;
				else
					this.Flags ^= 0x2;
			}
		}
		
		public SettingsFrame ()
		{
			Settings = new int[6];
			Settings[0] = -1;
		}
		public int UploadBandwidth { get; set; }
		public int DownloadBandwidth { get; set; }
		public int RoundTripTime { get; set; }
		public int MaxConcurrentStreams { get; set; }
		public int CWND { get; set; }
	}
}

