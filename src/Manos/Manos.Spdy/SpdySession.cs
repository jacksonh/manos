using System;
using System.Text;
using System.Collections.Generic;

using Manos.IO;

namespace Manos.Spdy
{
	public class SpdySession
	{
		private Socket socket;
		private SpdyConnectionCallback callback;
		private SPDYParser parser;
		private int laststreamid;
		
		public InflatingZlibContext Inflate;
		public DeflatingZlibContext Deflate;
		public Context Context { get; set; }
		public SpdySession (Context context, Socket sock, SpdyConnectionCallback cb)
		{
			this.socket = sock;
			this.callback = cb;
			this.Inflate = new InflatingZlibContext();
			this.Deflate = new DeflatingZlibContext();
			this.Context = context;
			this.parser = new SPDYParser(this.Inflate);
			parser.OnSynStream += HandleSynStream;
			parser.OnRstStream += HandleRstStream;
			parser.OnPing += HandlePing;
			this.socket.GetSocketStream().Read(onData, onError, onEndOfStream);
		}

		void HandlePing (PingFrame packet)
		{
			this.socket.GetSocketStream().Write(packet.Serialize());
		}

		void HandleRstStream (RstStreamFrame packet)
		{
			this.socket.Close();
		}

		void HandleSynStream (SynStreamFrame packet)
		{
			if (packet.StreamID < laststreamid) {
				RstStreamFrame rst = new RstStreamFrame();
				rst.StreamID = packet.StreamID;
				rst.StatusCode = RstStreamStatusCode.PROTOCOL_ERROR;
				this.socket.GetSocketStream().Write(rst.Serialize());
				this.socket.Close();
				return;
			}
			this.laststreamid = packet.StreamID;
			var t = new SpdyTransaction(Context, packet, parser, new SpdyStream(socket, this.Deflate), callback);
		}
		private void onData(ByteBuffer data)
		{
			parser.Parse(data.Bytes, data.Position, data.Length);
		}
		private void onError(Exception error)
		{
		}
		private void onEndOfStream()
		{
		}
	}
}

