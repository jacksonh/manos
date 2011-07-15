using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;

namespace Manos.IO.Managed
{
	class UdpSocket : Manos.IO.UdpSocket
	{
		Context Context { get; set; }
		
		byte [] buffer = new byte[64 * 1024];
		System.Net.Sockets.Socket socket;
		Action<UdpPacket> readCallback;
		EndPoint dummy = new IPEndPoint (IPAddress.Any, 0);
		bool writeAllowed = false;
		
		public UdpSocket (Manos.IO.Context context, AddressFamily addressFamily)
		{
			Context = (Context) context;
			AddressFamily = addressFamily;
			socket = new System.Net.Sockets.Socket (GetFamily(addressFamily), SocketType.Dgram, ProtocolType.Udp);
		}
		
		System.Net.Sockets.AddressFamily GetFamily (AddressFamily family)
		{
			switch (family) {
			case AddressFamily.InternNetwork:
				return System.Net.Sockets.AddressFamily.InterNetwork;
			case AddressFamily.InternNetwork6:
				return System.Net.Sockets.AddressFamily.InterNetworkV6;
			default:
				throw new Exception("Address family not supported");
			}
		}
		
		public override void Receive (Action<UdpPacket> readCallback)
		{
			
			this.readCallback = readCallback;
			try {
				socket.BeginReceiveFrom(buffer, 0, buffer.Length, SocketFlags.None, ref dummy, ReceiveFrom, null);
			} catch {
			}
		}
		
		void ReceiveFrom (IAsyncResult ar)
		{
			EndPoint remote = new IPEndPoint (IPAddress.Any, 0);

			int length = socket.EndReceiveFrom (ar, ref remote);
			
			IPEndPoint ipremote = (IPEndPoint) remote;
			
			var info = new UdpPacket() {
				Address = ipremote.Address.ToString(),
				Port = ipremote.Port,
				Buffer = new ByteBuffer (buffer, 0, length)
			};
				
			Context.Enqueue (delegate {
				readCallback(info);
				socket.BeginReceiveFrom (buffer, 0, buffer.Length, SocketFlags.None, ref dummy, ReceiveFrom, null);
			});
		}
		
		void DnsResolve (string host, Action<IPAddress> callback)
		{
			IPAddress addr;
			if (!IPAddress.TryParse (host, out addr)) {
				Dns.BeginGetHostEntry (host, (a) => {
					Context.Enqueue (delegate {
						try {
							IPHostEntry ep = Dns.EndGetHostEntry (a);
							callback (ep.AddressList[0]);
						} catch {
						}
					});
				}, null);
			} else {
				callback (addr);
			}
		}
		
		public override void Bind (string host, int port)
		{
			socket.Bind(new IPEndPoint (IPAddress.Any, port));
		}
		
		public override void Send (IEnumerable<UdpPacket> packet)
		{
			base.Send (packet);
			ResumeWriting ();
		}
		
		public override void Send (UdpPacket packet)
		{
			CheckAddress (packet.Address);
			base.Send (packet);
		}
		
		void SendAsync(IAsyncResult ar)
		{
			socket.EndSendTo(ar);
		}
		
		public override void ResumeWriting ()
		{
			if (!writeAllowed) {
				writeAllowed = true;
				HandleWrite ();
			}
		}
		
		public override void PauseWriting ()
		{
			writeAllowed = false;
		}
		
		protected override void HandleWrite ()
		{
			if (writeAllowed) {
				base.HandleWrite ();
			}
		}
		
		public override void Close ()
		{
			if (socket != null) {
				socket.Close ();
			}
		}
		

		protected override int WriteSinglePacket (UdpPacket packet)
		{
			IPEndPoint ep = new IPEndPoint(System.Net.IPAddress.Parse(packet.Address), packet.Port);
			socket.BeginSendTo(packet.Buffer.Bytes, packet.Buffer.Position, packet.Buffer.Length, SocketFlags.None, ep, WriteCallback, null);
			
			return packet.Buffer.Length;
		}
		
		void WriteCallback (IAsyncResult ar)
		{
			Context.Enqueue (delegate {
				if (socket == null)
					return;
				
				SocketError err;
				socket.EndSend (ar, out err);
				if (err == SocketError.Success) {
					HandleWrite ();
				}
			});
		}
	}
}

