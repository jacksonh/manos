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
		
		public UdpSocket (Manos.IO.Context context)
		{
			Context = (Context) context;
			socket = new System.Net.Sockets.Socket (AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
		}
		
		public override void Listen (string host, int port, Action<UdpPacket> readCallback)
		{
			this.readCallback = readCallback;
			DnsResolve (host, delegate (IPAddress addr) {
				StartListeningSocket (addr, port);
			});
		}
		
		void StartListeningSocket (IPAddress addr, int port)
		{
			try {
				socket.Bind (new IPEndPoint (addr, port));
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
				
			Enqueue (delegate {
				readCallback(info);
				socket.BeginReceiveFrom (buffer, 0, buffer.Length, SocketFlags.None, ref dummy, ReceiveFrom, null);
			});
		}
		
		void DnsResolve (string host, Action<IPAddress> callback)
		{
			IPAddress addr;
			if (!IPAddress.TryParse (host, out addr)) {
				Dns.BeginGetHostEntry (host, (a) => {
					Enqueue (delegate {
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
		
		protected void Enqueue (Action action)
		{
			lock (this) {
				Context.Enqueue (action);
			}
		}

		public override void Bind (int port)
		{
			socket.Bind(new IPEndPoint (IPAddress.Any, port));
		}
		
		public override void Send (IEnumerable<UdpPacket> packet)
		{
			base.Send (packet);
			ResumeWriting ();
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
			Enqueue (delegate {
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

