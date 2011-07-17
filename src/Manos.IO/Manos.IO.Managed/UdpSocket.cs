using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;

namespace Manos.IO.Managed
{
	class UdpSocket : Manos.IO.UdpSocket
	{
		class UdpStream : ManagedStream<UdpPacket>
		{
			UdpSocket socket;
			byte [] buffer = new byte[64 * 1024];
			long readLimit;
			bool readAllowed, writeAllowed;
			EndPoint dummy = new IPEndPoint (IPAddress.Any, 0);
			
			internal UdpStream (UdpSocket socket)
				: base (socket.Context)
			{
				this.socket = socket;
			}
			
			public override long Position {
				get { throw new NotSupportedException (); }
				set { throw new NotSupportedException (); }
			}
			
			public override bool CanRead {
				get { return true; }
			}
			
			public override bool CanWrite {
				get { return true; }
			}
			
			public override void ResumeReading()
			{
				ResumeReading (long.MaxValue);
			}
			
			public override void ResumeReading(long forFragments)
			{
				if (forFragments < 0)
					throw new ArgumentException ("forFragments");

				readLimit = forFragments;
				if (!readAllowed) {
					readAllowed = true;
					Receive ();
				}
			}
			
			public override void ResumeWriting()
			{
				if (!writeAllowed) {
					writeAllowed = true;
					HandleWrite ();
				}
			}

			public override void PauseReading ()
			{
				readAllowed = false;
			}

			public override void PauseWriting ()
			{
				writeAllowed = false;
			}
			
			public override void Flush()
			{
			}
			
			protected override long FragmentSize (UdpPacket fragment)
			{
				return 1;
			}

			protected override void HandleWrite ()
			{
				if (writeAllowed) {
					base.HandleWrite ();
				}
			}
		
			void Receive ()
			{
				try {
					socket.socket.BeginReceiveFrom (buffer, 0, buffer.Length, SocketFlags.None, ref dummy, ReceiveFrom, null);
				} catch (Exception e) {
					RaiseError (e);
				}
			}
			
			void ReceiveFrom (IAsyncResult ar)
			{
				EndPoint remote = new IPEndPoint (IPAddress.Any, 0);
	
				int length = socket.socket.EndReceiveFrom (ar, ref remote);
				
				IPEndPoint ipremote = (IPEndPoint) remote;
				
				byte [] newBuffer = new byte [length];
				Buffer.BlockCopy (buffer, 0, newBuffer, 0, length);
				
				var info = new UdpPacket (
					ipremote.Address.ToString (),
					ipremote.Port,
					new ByteBuffer (newBuffer));
					
				Context.Enqueue (delegate {
					RaiseData (info);
					Receive ();
				});
			}
			
			protected override WriteResult WriteSingleFragment(UdpPacket packet)
			{
				IPEndPoint ep = new IPEndPoint (System.Net.IPAddress.Parse (packet.Address), packet.Port);
				socket.socket.BeginSendTo (packet.Buffer.Bytes, packet.Buffer.Position, packet.Buffer.Length, SocketFlags.None, ep, WriteCallback, null);
				
				return WriteResult.Consume;
			}
			
			void WriteCallback (IAsyncResult ar)
			{
				Context.Enqueue (delegate {
					if (socket == null)
						return;
					
					SocketError err;
					socket.socket.EndSend (ar, out err);
					if (err == SocketError.Success) {
						HandleWrite ();
					}
				});
			}
		}
		
		System.Net.Sockets.Socket socket;
		UdpStream stream;
		
		public UdpSocket (Context context, AddressFamily addressFamily)
			: base (context)
		{
			AddressFamily = addressFamily;
			socket = new System.Net.Sockets.Socket (GetFamily (addressFamily), SocketType.Dgram, ProtocolType.Udp);
		}
		
		public new Context Context {
			get { return (Context) base.Context; }
		}
		
		System.Net.Sockets.AddressFamily GetFamily (AddressFamily family)
		{
			switch (family) {
				case AddressFamily.InterNetwork:
					return System.Net.Sockets.AddressFamily.InterNetwork;
				case AddressFamily.InterNetworkV6:
					return System.Net.Sockets.AddressFamily.InterNetworkV6;
				default:
					throw new Exception ("Address family not supported");
			}
		}
		
		public override IStream<UdpPacket> GetSocketStream()
		{
			if (stream == null) {
				stream = new UdpStream (this);
			}
			return stream;
		}
		
		public override void Bind (string host, int port)
		{
			socket.Bind (new IPEndPoint (IPAddress.Any, port));
		}
		
		public override void Close ()
		{
			if (socket != null) {
				socket.Close ();
			}
		}
	}
}

