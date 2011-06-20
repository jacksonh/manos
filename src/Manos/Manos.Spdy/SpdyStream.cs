using System;
using Manos.IO;
using System.IO;
namespace Manos.Spdy
{
	public class SpdyStream
	{
		private Socket Socket;
		private DeflatingZlibContext Deflate;
		public bool ReplyWritten { get; set; }
		public int StreamID { get; set; }
		public SpdyStream (Socket socket, DeflatingZlibContext deflate)
		{
			this.Socket = socket;
			this.Deflate = deflate;
			ReplyWritten = false;
		}
		public void SendFile(string filename)
		{
			var info = new FileInfo(filename);
			if (this.Socket.GetSocketStream() is ISendfileCapable) {
				DataFrame header = new DataFrame();
				header.StreamID = this.StreamID;
				header.Length = (int)info.Length;
				this.Socket.GetSocketStream().Write(header.SerializeHeader());
				((ISendfileCapable) this.Socket.GetSocketStream()).SendFile (filename);
			} else {
				var str = Socket.Context.OpenFile(filename, FileAccess.Read, 64 * 1024);
				str.Read((buf) => { 
					DataFrame d = new DataFrame();
					d.Flags = 0x00;
					d.StreamID = this.StreamID;
					d.Length = buf.Length - buf.Position;
					d.Data = new byte[d.Length];
					Array.Copy(buf.Bytes, buf.Position, d.Data, 0, d.Length);
					var ret = d.Serialize();
					this.Socket.GetSocketStream().Write (new ByteBuffer(ret, 0, ret.Length));
				},(e)=>{}, () => {
					DataFrame d = new DataFrame();
					d.Flags = 0x01;
					d.StreamID = this.StreamID;
					d.Length = 0;
					d.Data = new byte[d.Length];
					var ret = d.Serialize();
					this.Socket.GetSocketStream().Write (new ByteBuffer(ret, 0, ret.Length));
				});
			}
		}
		public void WriteReply(SpdyResponse res, bool done = false)
		{
			SynReplyFrame rep = new SynReplyFrame();
			this.StreamID = rep.StreamID = res.Request.StreamID;
			rep.Version = 2;
			if (done)
				rep.Flags = 0x01;
			else
				rep.Flags = 0x00;
			rep.Headers = new NameValueHeaderBlock();
			rep.Headers["version"] = "HTTP/" + res.Request.MajorVersion + "." + res.Request.MinorVersion;
			rep.Headers["status"] = res.StatusCode.ToString();
			foreach (var header in res.Headers.Keys)
			{
				rep.Headers[header] = res.Headers[header];
			}
			this.Socket.GetSocketStream().Write(rep.Serialize(this.Deflate));
			ReplyWritten = true;
		}
		public void Write(ByteBuffer buf)
		{
			Write(buf.Bytes, buf.Position, buf.Length);
		}
		public void Write(byte[] data, int offset, int length)
		{
			DataFrame d = new DataFrame();
			d.Flags = 0x00;
			d.StreamID = this.StreamID;
			d.Length = length - offset;
			d.Data = new byte[d.Length];
			Array.Copy(data, offset, d.Data, 0, length);
			var ret = d.Serialize();
			this.Socket.GetSocketStream().Write (new ByteBuffer(ret, 0, ret.Length));
		}
		public void End()
		{
			DataFrame d = new DataFrame();
			d.Flags = 0x01;
			d.StreamID = this.StreamID;
			d.Length = 0;
			d.Data = new byte[0];
			var ret = d.Serialize();
			this.Socket.GetSocketStream().Write (new ByteBuffer(ret, 0, ret.Length));
		}
	}
}

