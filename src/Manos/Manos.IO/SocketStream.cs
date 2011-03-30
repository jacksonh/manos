using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;


using Libev;
using Manos.Collections;
using System.Net;

namespace Manos.IO
{
	[StructLayout (LayoutKind.Sequential)]
	public struct SocketInfo
	{
		public int fd;
		public int port;
		public int is_ipv4;
		public byte a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15, a16;

		public IPAddress Address {
			get {
				if (is_ipv4 != 0) {
					return new IPAddress (new byte [] { a1, a2, a3, a4 });
				} else {
					return new IPAddress (new byte [] { a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15, a16 });
				}
			}
		}
	}

	public abstract class SocketStream : IOStream, IDisposable
	{
		public enum SocketState
		{
			None,

			AcceptingConnections,
			Open,
		}
		
		protected SocketState state;
		protected int port;
		private IPAddress address;

		public SocketStream (IOLoop ioloop) : base (ioloop)
		{
		}

		public SocketStream (SocketInfo info, IOLoop ioloop) : base (ioloop)
		{
			state = SocketState.Open;

			port = info.port;
			address = info.Address;
		}

		public string Address {
			get {
				if (state == SocketState.None)
					return null;
				return address.ToString ();
			}
		}

		public int Port {
			get {
				if (state == SocketState.None)
					return -1;
				return port;
			}
		}

		public void Dispose ()
		{
			Close ();
		}
		
		internal abstract SendFileOperation MakeSendFile (string file);

		public void Write (byte [] data, WriteCallback callback)
		{
			Write (data, 0, data.Length, callback);
		}

		public void Write (byte [] data, int offset, int count, WriteCallback callback)
		{
			var bytes = new List<ByteBuffer> ();
			bytes.Add (new ByteBuffer (data, offset, count));

			var write_bytes = new SendBytesOperation (bytes, callback);
			QueueWriteOperation (write_bytes);
		}

		public abstract void Listen (string host, int port);

		protected abstract void AcceptConnections ();

		protected abstract int ReadOneChunk (out int error);

		private void Read ()
		{
			int size;
			int error;

			size = ReadOneChunk (out error);
			if (size < 0 && error != 0) {
				Close ();
				return;
			}

			if (size == 0) {
				read_callback (this, ReadChunk, 0, 0);
				Close ();
				return;
			} else if (size > 0) {
				read_callback (this, ReadChunk, 0, size);
			}

			read_callback (this, ReadChunk, 0, size);
		}

		protected override void HandleRead ()
		{
			if (state == SocketState.AcceptingConnections) {
				AcceptConnections ();
				return;
			}

			if (state == SocketState.Open) {
				Read ();
				return;
			}
		}

		internal abstract int Send (ByteBufferS [] buffers, int length, out int error);

		protected void OnConnectionAccepted (SocketStream stream)
		{
			if (ConnectionAccepted != null)
				ConnectionAccepted (this, new ConnectionAcceptedEventArgs (stream));
		}

		public event EventHandler<ConnectionAcceptedEventArgs> ConnectionAccepted;
	}
}

