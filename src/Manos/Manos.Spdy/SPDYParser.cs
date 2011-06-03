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
				switch((ControlFrameType)Convert.ToInt32(data[offset + 3]))
				{
				case ControlFrameType.SYN_STREAM:
					if (OnSynStream != null)
					{
						OnSynStream(new SynStreamFrame(data, offset, length));
					}
					break;
				case ControlFrameType.SYN_REPLY:
					if (OnSynReply != null)
					{
						OnSynReply(new SynReplyFrame(data, offset, length));
					}
					break;
				case ControlFrameType.RST_STREAM:
					if (OnRstStream != null)
					{
						OnRstStream(new RstStreamFrame(data, offset, length));
					}
					break;
				case ControlFrameType.SETTINGS:
					if (OnSettings != null)
					{
						OnSettings(new SettingsFrame(data, offset, length));
					}
					break;
				case ControlFrameType.PING:
					if (OnPing != null)
					{
						OnPing(new PingFrame(data, offset, length));
					}
					break;
				case ControlFrameType.GOAWAY:
					if (OnGoaway != null)
					{
						OnGoaway(new GoawayFrame(data, offset, length));
					}
					break;
				case ControlFrameType.HEADERS:
					if (OnHeaders != null)
					{
						OnHeaders(new HeadersFrame(data, offset, length));
					}
					break;
				case ControlFrameType.WINDOW_UPDATE:
					if (OnWindowUpdate != null)
					{
						OnWindowUpdate(new WindowUpdateFrame(data, offset, length));
					}
					break;
				case ControlFrameType.VERSION:
					if (OnVersion != null)
					{
						OnVersion(new VersionFrame(data, offset, length));
					}
					break;
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

