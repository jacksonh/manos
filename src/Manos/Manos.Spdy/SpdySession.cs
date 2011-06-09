using System;
using System.Text;

using Manos.IO;

namespace Manos.Spdy
{
	public class SpdySession
	{
		private Socket socket;
		private SpdyConnectionCallback callback;
		private Stream socketstream;
		private SPDYParser parser;
		private InflatingZlibContext inflate;
		private DeflatingZlibContext deflate;
		public SpdySession (Socket sock, SpdyConnectionCallback cb)
		{
			this.socket = sock;
			this.callback = cb;
			this.inflate = new InflatingZlibContext();
			this.deflate = new DeflatingZlibContext();
			this.parser = new SPDYParser(this.inflate);
			parser.OnSynStream += HandleSynStream;
			parser.OnRstStream += HandleRstStream;
			this.socketstream = this.socket.GetSocketStream();
			this.socketstream.Read(onData, onError, onEndOfStream);
		}

		void HandleRstStream (RstStreamFrame packet)
		{
			Console.WriteLine(((RstStreamStatusCode)packet.StatusCode).ToString());
			this.socket.Close();
		}

		void HandleSynStream (SynStreamFrame packet)
		{
			if (packet.Headers["url"].Contains("favicon.ico"))
			{
				SynReplyFrame rep = new SynReplyFrame();
				rep.Version = 2;
				rep.StreamID = packet.StreamID;
				rep.Flags = 0x01;
				NameValueHeaderBlock n = new NameValueHeaderBlock();
				n.Add("status", "404");
				n.Add("version", "HTTP/1.1");
				rep.Headers = n;
				byte[] res = rep.Serialize(this.deflate);
				this.socketstream.Write(res);
				return;
			}
			SynReplyFrame reply = new SynReplyFrame();
			reply.Version = 2;
			reply.StreamID = packet.StreamID;
			reply.Flags = 0x00;
			NameValueHeaderBlock nv = new NameValueHeaderBlock();
			nv.Add("status", "200");
			nv.Add("version", "HTTP/1.1");
			nv.Add("Content-Type", "text/plain");
			reply.Headers = nv;
			byte[] resp = reply.Serialize(this.deflate);
			this.socketstream.Write(resp);
			DataFrame final = new DataFrame();
			final.StreamID = packet.StreamID;
			final.Flags = 0x01;
			final.Data = Encoding.UTF8.GetBytes("Hello World\n");
			this.socketstream.Write(final.Serialize());
//			Console.WriteLine("Data 1 Sent");
//			DataFrame f2 = new DataFrame();
//			f2.StreamID = 1;
//			f2.Flags = 0x01;
//			this.socketstream.Write(f2.Serialize());
//			Console.WriteLine("Data 2 Sent");
		}
		private void onData(ByteBuffer data)
		{
			parser.Parse(data.Bytes, data.Position, data.Length);
		}
		private void onError(Exception error)
		{
			//Console.WriteLine("ERROR: {0}", error.Message);
		}
		private void onEndOfStream()
		{
			//this.socket.Close();
		}
	}
}

