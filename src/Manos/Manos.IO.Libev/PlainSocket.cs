using System;
using Manos.Collections;
using System.Runtime.InteropServices;
using Libev;
using System.Collections.Generic;

namespace Manos.IO.Libev
{
	public class PlainSocket : EventedSocket
	{
		Action<Socket> acceptCallback;
		PlainSocketStream stream;
		
		class PlainSocketStream : EventedStream, ISendfileCapable
		{
			PlainSocket parent;
			byte [] receiveBuffer = new byte[4096];
			SocketInfo [] socketInfos;
			SendFileOperation currentSendFile;
			Queue<string> sendFileQueue;

			public PlainSocketStream (PlainSocket parent, IntPtr handle)
				: base (parent.Loop, handle)
			{
				this.parent = parent;
			}

			public void SendFile (string file)
			{
				if (sendFileQueue == null) {
					sendFileQueue = new Queue<string> ();
				}
				sendFileQueue.Enqueue (file);
				writeQueue.Enqueue (null);
				ResumeWriting ();
			}

			protected override bool EnsureActiveBuffer ()
			{
				return base.EnsureActiveBuffer () || currentSendFile != null;
			}

			protected override bool EnsureActiveWriter ()
			{
				if (currentWriter == null && writeQueue.Count > 0 && writeQueue.Peek () == null) {
					writeQueue.Dequeue ();
					currentSendFile = new SendFileOperation (this, sendFileQueue.Dequeue ());
					return false;
				} else {
					return base.EnsureActiveWriter ();
				}
			}
			
			protected override void SendCurrentBuffer()
			{
				if (currentSendFile != null) {
					if (currentSendFile.Run ()) {
						currentSendFile.Dispose ();
						currentSendFile = null;
					}
				} else {
					base.SendCurrentBuffer ();
				}
			}

			public override void Close ()
			{
				if (parent == null) {
					throw new ObjectDisposedException (GetType ().FullName);
				}
				
				int error;
				int res = manos_socket_close (Handle.ToInt32 (), out error);

				if (res < 0) {
					Console.Error.WriteLine ("Error '{0}' closing socket: {1}", error, Handle.ToInt32 ());
					Console.Error.WriteLine (Environment.StackTrace);
				}
				
				receiveBuffer = null;
				socketInfos = null;
				parent = null;
				
				base.Close ();
			}

			protected override void HandleRead ()
			{
				switch (parent.state) {
					case SocketState.Invalid:
						throw new InvalidOperationException ();
						
					case SocketState.Listening:
						HandleAccept ();
						break;
						
					case SocketState.Open:
						HandleData ();
						break;
				}
			}

			void HandleAccept ()
			{
				if (socketInfos == null)
					socketInfos = new SocketInfo[100];
				
				int error;
				int amount = manos_socket_accept_many (Handle.ToInt32 (), socketInfos, 
					socketInfos.Length, out error);
				
				if (amount < 0)
					throw new Exception (String.Format ("Exception while accepting. errno: {0}", error));

				for (int i = 0; i < amount; i++) {
					var socket = new PlainSocket (parent.Loop, socketInfos [i]);
					parent.acceptCallback (socket);
				}
			}

			void HandleData ()
			{
				int err;
				var received = manos_socket_receive (Handle.ToInt32 (), receiveBuffer, receiveBuffer.Length, out err);
				if (received == 0) {
					Close ();
				} else {
					RaiseData (new ByteBuffer (receiveBuffer, 0, received));
				}
			}

			protected override int WriteSingleBuffer (ByteBuffer buffer)
			{
				int err;
				return manos_socket_send (Handle.ToInt32 (), buffer.Bytes, buffer.Position, buffer.Length, out err);
			}

			[DllImport ("libmanos", CallingConvention = CallingConvention.Cdecl)]
			private static extern int manos_socket_close (int fd, out int err);

			[DllImport ("libmanos", CallingConvention = CallingConvention.Cdecl)]
			private static extern int manos_socket_accept_many (int fd, SocketInfo [] infos, int max, out int err);

			[DllImport ("libmanos", CallingConvention = CallingConvention.Cdecl)]
			private static extern int manos_socket_receive (int fd, byte [] buffer, int max, out int err);

			[DllImport ("libmanos", CallingConvention = CallingConvention.Cdecl)]
			internal static extern int manos_socket_send (int fd, byte [] buffer, int offset, int len, out int err);
		}
		
		public PlainSocket (IOLoop loop)
			: base (loop)
		{
		}

		PlainSocket (IOLoop loop, SocketInfo info)
			: base (loop, info)
		{
			stream = new PlainSocketStream (this, new IntPtr (info.fd));
			this.state = Socket.SocketState.Open;
		}

		public override Stream GetSocketStream ()
		{
			if (state != Socket.SocketState.Open)
				throw new InvalidOperationException ();
			return stream;
		}

		public override void Connect (string host, int port, Action callback)
		{
			int error;
			var fd = manos_socket_connect (host, port, out error);

			if (fd < 0)
				throw new Exception (String.Format ("An error occurred while trying to connect to {0}:{1} errno: {2}", host, port, error));
			
			stream = new PlainSocketStream (this, new IntPtr (fd));
			
			var connectWatcher = new IOWatcher (new IntPtr (fd), EventTypes.Write, Loop.EVLoop, (loop, watcher, revents) => {
				watcher.Stop ();
				watcher.Dispose ();
				
				this.address = host;
				this.port = port;
				
				this.state = Socket.SocketState.Open;
				
				callback ();
			});
			connectWatcher.Start ();
		}

		public override void Listen (string host, int port, Action<Socket> callback)
		{
			if (callback == null)
				throw new ArgumentNullException ("callback");
			
			this.acceptCallback = callback;
			
			int error;
			int fd = manos_socket_listen (host, port, 128, out error);

			if (fd < 0) {
				if (error == 98)
					throw new Exception (String.Format ("Address {0}::{1} is already in use.", host, port));
				throw new Exception (String.Format ("An error occurred while trying to liste to {0}:{1} errno: {2}", host, port, error));
			}
			
			state = Socket.SocketState.Listening;
			
			stream = new PlainSocketStream (this, new IntPtr (fd));
			stream.ResumeReading ();
		}

		public override void Close ()
		{
			if (stream != null)
				stream.Close ();
			base.Close ();
		}

		[DllImport ("libmanos", CallingConvention = CallingConvention.Cdecl)]
		private static extern int manos_socket_connect (string host, int port, out int err);

		[DllImport ("libmanos", CallingConvention = CallingConvention.Cdecl)]
		private static extern int manos_socket_listen (string host, int port, int backlog, out int err);
/*

        public override ISendFileOperation MakeSendFile (string file)
		{
#if DISABLE_POSIX
			return new PosixSendFileOperation (file, null);
#else
			return new CopyingSendFileOperation (file, null);
#endif
		}
 */
	}
}

