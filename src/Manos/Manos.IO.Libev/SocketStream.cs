//
// Copyright (C) 2010 Jackson Harper (jackson@manosdemono.com)
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//
//


using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;


using Libev;
using Manos.Collections;
using System.Net;

namespace Manos.IO.Libev {

	[StructLayout (LayoutKind.Sequential)]
	public struct SocketInfo {
		public int fd;
		public int port;
		public int is_ipv4;
		// i wish i knew a sane way to get this right using arrays
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

	public class SocketStream : IOStream, Manos.IO.ISocketStream, IDisposable {

		public enum SocketState {
			None,

			AcceptingConnections,
			Open,
		}

		private SocketState state;

		internal int fd;
		internal IntPtr handle;
		internal string host;
		internal int port;
		internal IPAddress address;

		private static readonly int MAX_ACCEPT = 100;
		private SocketInfo [] accept_infos;

		public SocketStream (IOLoop ioloop) : base (ioloop)
		{
		}

		public SocketStream (SocketInfo info, IOLoop ioloop) : base (ioloop)
		{
			fd = info.fd;

			if (fd > 0) {
				SetHandle (fd);
				state = SocketState.Open;
			}

			port = info.port;
			address = info.Address;
		}

		public string Address {
			get {
				if (state == SocketState.None)
					return null;
				int port;
				return GetAddress (out port);
			}
		}

		public int Port {
			get {
				if (state == SocketState.None)
					return -1;
				int port;
				GetAddress (out port);
				return port;
			}
		}

		private void SetHandle (int fd)
		{
			handle = new IntPtr (fd);
			SetHandle (handle);
		}
		
		//
		// Eventually everything will be stored as IPAddress only, since
		// DNS will be handled by our special DNS code
		//
		private string GetAddress (out int port)
		{
			/*
			EndPoint end = null;
			switch (state) {
			case SocketState.Open:
				end = socket.RemoteEndPoint;
				break;
			case SocketState.AcceptingConnections:
				end = socket.LocalEndPoint;
				break;
			}

			IPEndPoint ip = end as IPEndPoint;
			if (ip != null) {
				port = ip.Port;
				return ip.Address.ToString ();
			}

			DnsEndPoint dns  = end as DnsEndPoint;
			if (dns != null) {
				port = dns.Port;
				return dns.Host;
			}
			*/
			port = -1;
			return null;

		}

		public void Dispose ()
		{
			Close ();
		}

		public override void Close ()
		{

			//		IOWatcher.ReleaseHandle (socket, Handle);

			base.Close ();

			if (fd == -1)
				return;

			int error;
			int res = manos_socket_close (fd, out error);

			if (res < 0) {
				Console.Error.WriteLine ("Error '{0}' closing socket: {1}", error, fd);
				Console.Error.WriteLine (Environment.StackTrace);
			}

			fd = -1;
		}

		public void Connect (string host, int port)
		{
			int error;
			fd = manos_socket_connect (host, port, out error);

			if (fd < 0)
				throw new Exception (String.Format ("An error occurred while trying to connect to {0}:{1} errno: {2}", host, port, error));
			
			
			IOWatcher iowatcher = new IOWatcher (new IntPtr (fd), EventTypes.Write, IOLoop.EventLoop, (l, w, r) => {
				w.Stop ();

				this.host = host;
				this.port = port;
				OnConnected ();
			});
			iowatcher.Start ();
		}

		public void Connect (int port)
		{
			Connect ("127.0.0.1", port);
		}

		public void Listen (string host, int port)
		{
			int error;
			fd = manos_socket_listen (host, port, 128, out error);

			if (fd < 0) {
			       if (error == 98)
				       throw new Exception (String.Format ("Address {0}::{1} is already in use.", host, port));
			       throw new Exception (String.Format ("An error occurred while trying to liste to {0}:{1} errno: {2}", host, port, error));
			}

			SetHandle (fd);

			DisableTimeout ();
			EnableReading ();
			state = SocketState.AcceptingConnections;
			accept_infos = new SocketInfo [MAX_ACCEPT];
		}	

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
		
		private void OnConnected ()
		{
			SetHandle (fd);
			state = SocketState.Open;

			if (Connected != null)
				Connected (this);
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

		private void AcceptConnections ()
		{
			int error;

			int amount = manos_socket_accept_many (fd, accept_infos, MAX_ACCEPT, out error);
			if (amount < 0)
				throw new Exception (String.Format ("Exception while accepting. errno: {0}", error));

//			Console.WriteLine ("Accepted: '{0}' connections.", amount);
			for (int i = 0; i < amount; i++) {
//				Console.WriteLine ("Accepted: '{0}'", accept_infos [i]);
				SocketStream iostream = new SocketStream (accept_infos [i], IOLoop);
				OnConnectionAccepted (iostream);
			}
		}

		private void Read ()
		{
			int size;
			int error;

			size = manos_socket_receive (fd, ReadChunk, ReadChunk.Length, out error);
			if (size < 0 && error != 0) {
				Close ();
				return;
			}

			if (size == 0) {
				read_callback (this, ReadChunk, 0, 0);
				Close ();
				return;
			}

			read_callback (this, ReadChunk, 0, size);
		}

        public int Send(ByteBufferS[] buffers, int length, out int error)
		{
			return manos_socket_send (fd, buffers, length, out error);
		}

		public int SendFile (string name, bool chunked, long length, Action<long,int> callback)
		{
			GCHandle handle = GCHandle.Alloc (callback);
			int result = manos_socket_send_file (fd, name, chunked ? 1 : 0, new IntPtr (length), LengthCallbackHandler, GCHandle.ToIntPtr (handle));
			return result;
		}

		private static void LengthCallbackHandler (IntPtr gchandle, IntPtr length, int error)
		{
			GCHandle handle = GCHandle.FromIntPtr (gchandle);
			Action<long,int> cb = (Action<long,int>) handle.Target;

			handle.Free ();

			cb (length.ToInt64 (), error);
		}

		private void OnConnectionAccepted (SocketStream stream)
		{
			if (ConnectionAccepted != null)
				ConnectionAccepted (this, new ConnectionAcceptedEventArgs (stream));
		}

		public event Action<Manos.IO.ISocketStream> Connected;
		public event EventHandler<ConnectionAcceptedEventArgs> ConnectionAccepted;


		[DllImport ("libmanos", CallingConvention = CallingConvention.Cdecl)]
		private static extern int manos_socket_connect (string host, int port, out int err);

		[DllImport ("libmanos", CallingConvention = CallingConvention.Cdecl)]
		private static extern int manos_socket_close (int fd, out int err);

		[DllImport ("libmanos", CallingConvention = CallingConvention.Cdecl)]
		private static extern int manos_socket_listen (string host, int port, int backlog, out int err);

		[DllImport ("libmanos", CallingConvention = CallingConvention.Cdecl)]
		private static extern int manos_socket_accept_many (int fd, SocketInfo [] infos, int max, out int err);

		[DllImport ("libmanos", CallingConvention = CallingConvention.Cdecl)]
		private static extern int manos_socket_receive (int fd, byte [] buffer, int max, out int err);

		[DllImport ("libmanos", CallingConvention = CallingConvention.Cdecl)]
		internal static extern int manos_socket_send (int fd, ByteBufferS [] buffers, int len, out int err);

		[DllImport ("libmanos", CallingConvention = CallingConvention.Cdecl)]
		internal static extern int manos_socket_send_file (int socket_fd, string name, int chunked, IntPtr length, Action<IntPtr,IntPtr,int> callback, IntPtr gchandle);
	}
}

