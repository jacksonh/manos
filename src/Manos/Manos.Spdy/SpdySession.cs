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
		private Stream socketstream;
		private SPDYParser parser;
		public InflatingZlibContext Inflate;
		public DeflatingZlibContext Deflate;
		Dictionary<int, Tuple<SynStreamFrame, List<byte[]>>> pendingreqs = new Dictionary<int, Tuple<SynStreamFrame, List<byte[]>>>();
		public SpdySession (Socket sock, SpdyConnectionCallback cb)
		{
			this.socket = sock;
			this.callback = cb;
			this.Inflate = new InflatingZlibContext();
			this.Deflate = new DeflatingZlibContext();
			this.parser = new SPDYParser(this.Inflate);
			parser.OnSynStream += HandleSynStream;
			parser.OnRstStream += HandleRstStream;
			parser.OnData += HandleData;
			this.socketstream = this.socket.GetSocketStream();
			this.socketstream.Read(onData, onError, onEndOfStream);
		}

		void HandleData (DataFrame packet)
		{
			if (pendingreqs.ContainsKey(packet.StreamID)) {
				pendingreqs[packet.StreamID].Item2.Add(packet.Data);
				if ((packet.Flags & 0x01) == 1) {
					var t = new SpdyTransaction(this.socket, pendingreqs[packet.StreamID].Item1, parser, callback, this, pendingreqs[packet.StreamID].Item2);
					t.LoadRequest();
				}
			} else {
				RstStreamFrame rst = new RstStreamFrame();
				rst.StreamID = packet.StreamID;
				rst.StatusCode = RstStreamStatusCode.PROTOCOL_ERROR;
				this.socketstream.Write(rst.Serialize());
			}
		}

		void HandleRstStream (RstStreamFrame packet)
		{
			this.socket.Close();
		}

		void HandleSynStream (SynStreamFrame packet)
		{
			if ((packet.Flags & 0x01) != 1) {
				pendingreqs[packet.StreamID] = Tuple.Create(packet, new List<byte[]>());
			}
			else {
				var t = new SpdyTransaction(this.socket, packet, parser, callback, this);
				t.LoadRequest();
			}
		}
		private void onData(ByteBuffer data)
		{
			parser.Parse(data.Bytes, data.Position, data.Length);
		}
		private void onError(Exception error)
		{
			Console.WriteLine("ERROR: {0}", error.InnerException);
			Console.WriteLine("{0}", error.Source);

		}
		private void onEndOfStream()
		{
			Console.WriteLine("End of Stream");
		}
	}
}

