using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;

namespace Manos.IO.Managed
{
	class UdpSocket : IPSocket<UdpPacket, IStream<UdpPacket>>, IUdpSocket
	{
		UdpStream stream;
		
		class UdpStream : ManagedStream<UdpPacket>
		{
			UdpSocket parent;
			EndPoint remote = new IPEndPoint (0, 0);
			byte [] buffer = new byte[64 * 1024];
			long? readLimit;
			bool readAllowed, writeAllowed;
			
			internal UdpStream (UdpSocket parent)
				: base (parent.Context)
			{
				this.parent = parent;
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
			
			public override void ResumeReading ()
			{
				readLimit = null;
				if (!readAllowed) {
					readAllowed = true;
					Receive ();
				}
			}
			
			public override void ResumeReading (long forFragments)
			{
				if (forFragments < 0)
					throw new ArgumentException ("forFragments");

				readLimit = forFragments;
				if (!readAllowed) {
					readAllowed = true;
					Receive ();
				}
			}
			
			public override void ResumeWriting ()
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

			public override void Flush ()
			{
			}
			
			protected override long FragmentSize (UdpPacket fragment)
			{
				return 1;
			}

			void Receive ()
			{
				try {
					parent.socket.BeginReceiveFrom (buffer, 0, buffer.Length, SocketFlags.None,
						ref remote, ReceiveFrom, null);
				} catch (Exception e) {
					RaiseError (e);
				}
			}
			
			void ReceiveFrom (IAsyncResult ar)
			{
				int length = parent.socket.EndReceiveFrom (ar, ref remote);
				
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
			
			protected override void HandleWrite ()
			{
				if (writeAllowed) {
					base.HandleWrite ();
				}
			}
			
			protected override WriteResult WriteSingleFragment (UdpPacket packet)
			{
				IPEndPoint ep = new IPEndPoint (IPAddress.Parse (packet.Address), packet.Port);
				parent.socket.BeginSendTo (packet.Buffer.Bytes, packet.Buffer.Position, packet.Buffer.Length,
					SocketFlags.None, ep, WriteCallback, null);
				
				return WriteResult.Consume;
			}
			
			void WriteCallback (IAsyncResult ar)
			{
				Context.Enqueue (delegate {
					if (parent == null)
						return;
					
					SocketError err;
					parent.socket.EndSend (ar, out err);
					if (err == SocketError.Success) {
						HandleWrite ();
					}
				});
			}
		}
		
		public UdpSocket (Context context, AddressFamily addressFamily)
			: base (context, addressFamily, ProtocolFamily.Udp)
		{
		}
		
		public override void Connect (IPEndPoint endpoint, Action callback)
		{
			socket.Connect (endpoint);
			callback ();
		}
		
		public override IStream<UdpPacket> GetSocketStream ()
		{
			if (stream == null) {
				stream = new UdpStream (this);
			}
			return stream;
		}
	}
}

