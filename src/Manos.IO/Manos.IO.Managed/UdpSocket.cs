using System;
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
		
		public UdpSocket (Manos.IO.Context context)
		{
			Context = (Context) context;
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
			socket = new System.Net.Sockets.Socket (addr.AddressFamily, SocketType.Dgram, ProtocolType.Udp);
			try {
				socket.Bind(new IPEndPoint (IPAddress.Any, port));
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
		
		public override void Close ()
		{
			if (socket != null) {
				socket.Close ();
			}
		}
	}
}

