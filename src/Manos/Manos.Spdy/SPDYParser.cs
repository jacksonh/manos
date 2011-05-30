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
		public SPDYParser ()
		{
		}
		public void Parse(byte[] data, int offset, int length)
		{
			var p = new SynStreamFrame();
			if (OnSynStream != null)
			{
				OnSynStream(p);
			}
			var q = new SynReplyFrame();
			if (OnSynReply != null)
			{
				OnSynReply(q);
			}
			var r = new RstStreamFrame();
			if (OnRstStream != null)
			{
				OnRstStream(r);
			}
			var s = new SettingsFrame();
			if (OnSettings != null)
			{
				OnSettings(s);
			}
			var t = new PingFrame();
			if (OnPing != null)
			{
				OnPing(t);
			}
			var u = new GoawayFrame();
			if (OnGoaway != null)
			{
				OnGoaway(u);
			}
			var v = new HeadersFrame();
			if (OnHeaders != null)
			{
				OnHeaders(v);
			}
			var w = new WindowUpdateFrame();
			if (OnWindowUpdate != null)
			{
				OnWindowUpdate(w);
			}
		}
	}
}

