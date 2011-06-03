using System;

namespace Manos.Spdy
{
	public class SPDYParser
	{
		public delegate void SynStreamHandler(SynStreamFrame packet);
		public event SynStreamHandler OnSynStream;
		public delegate void SynReplyHandler(SynReplyFrame packet);
		public event SynReplyHandler OnSynReply;
		public delegate void RstStreamHandler(RstStreamFrame packet);
		public event RstStreamHandler OnRstStream;
		public delegate void SettingsHandler(SettingsFrame packet);
		public event SettingsHandler OnSettings;
		public delegate void PingHandler(PingFrame packet);
		public event PingHandler OnPing;
		public delegate void GoawayHandler(GoawayFrame packet);
		public event GoawayHandler OnGoaway;
		public delegate void HeadersHandler(HeadersFrame packet);
		public event HeadersHandler OnHeaders;
		public delegate void WindowUpdateHandler(WindowUpdateFrame packet);
		public event WindowUpdateHandler OnWindowUpdate;
		public delegate void VersionHandler(VersionFrame packet);
		public event VersionHandler OnVersion;
		public delegate void DataHandler(DataFrame packet);
		public event DataHandler OnData;
		public SPDYParser ()
		{
		}
		public void Parse(byte[] data, int offset, int length)
		{
			if (IsControlFrame(data, offset)) {
				switch(data[offset + 3])
				{
				case 0x01:
					if (OnSynStream != null)
					{
						OnSynStream(new SynStreamFrame(data, offset, length));
					}
					break;
				case 0x02:
					if (OnSynReply != null)
					{
						OnSynReply(new SynReplyFrame(data, offset, length));
					}
					break;
				case 0x03:
					if (OnRstStream != null)
					{
						OnRstStream(new RstStreamFrame(data, offset, length));
					}
				}
			} else {
				if (OnData != null)
				{
					OnData(new DataFrame(data, offset, length));
				}
			}
		}
		public bool IsControlFrame(byte[] data, int offset)
		{
			return (data[offset] >> 7) == 1;
		}
	}
}

