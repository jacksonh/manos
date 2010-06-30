
using System;
using System.IO;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Collections;
using System.Collections.Generic;

using Mono.Unix.Native;


namespace Mango.Server {

	public delegate void ReadCallback (IOStream stream, byte [] data);

	public class IOStream {

		private Socket socket;
		private IOLoop ioloop;
		private EpollEvents state;

		private MemoryStream read_buffer;
		private StringBuilder write_buffer;

		private int read_bytes = -1;
		private byte [] read_delimiter;
		private ReadCallback read_callback;

		private static readonly int DefaultReadChunkSize  = 4096;

		public IOStream (Socket socket, IOLoop ioloop)
		{
			this.socket = socket;
			this.ioloop = ioloop;

			ReadChunkSize = DefaultReadChunkSize;

			socket.Blocking = false;

			state = IOLoop.EPOLL_ERROR_EVENTS;
			ioloop.AddHandler (socket.Handle, HandleEvents, state);
		}

		public int ReadChunkSize {
			get { return ReadChunk.Length; }
			set {
				if (IsReading)
					throw new Exception ("ReadChunkSize can not be changed while reading.");

				if (ReadChunk != null && value == ReadChunk.Length)
					return;
				ReadChunk = new byte [value];
			}
		}

		private byte [] ReadChunk {
			get;
			set;
		}

		public bool IsReading {
			get { return read_callback != null; }
		}

		public bool IsClosed {
			get { return socket == null || !socket.Connected; }
		}

		public void ReadUntil (string delimiter, ReadCallback callback)
		{
			CheckCanRead ();

			read_delimiter = Encoding.ASCII.GetBytes (delimiter);
			read_callback = callback;

			AddIOState (IOLoop.EPOLL_READ_EVENTS);
		}

		public void ReadBytes (int num_bytes, ReadCallback callback)
		{
			CheckCanRead ();

			read_bytes = num_bytes;
			read_callback = callback;

			AddIOState (IOLoop.EPOLL_READ_EVENTS);
		}

		public void Close ()
		{
			if (true)
				throw new Exception ();
			if (socket == null)
				return;
			ioloop.RemoveHandler (socket.Handle);
			socket.Close ();
			socket = null;
		}

		private void AddIOState (EpollEvents events)
		{
			if ((state & events) != 0)
				return;
			ioloop.UpdateHandler (socket.Handle, events);
				
		}

		private void CheckCanRead ()
		{
			if (IsReading)
				throw new Exception ("Attempt to read bytes while we are already performing a read operation.");
			if (IsClosed)
				throw new Exception ("Attempt to read on a closed socket.");
		}

		private void HandleEvents (IntPtr fd, EpollEvents events)
		{
			if (IsClosed) {
				Console.Error.WriteLine ("events on closed socket.");
				return;
			}

			if (fd != socket.Handle)
				throw new Exception ("Incorrectly routed event.");

			if ((events & IOLoop.EPOLL_READ_EVENTS) != 0)
				HandleRead ();
		}

		private void HandleRead ()
		{
			int size;

			try {
				size = socket.Receive (ReadChunk);
			} catch (SocketException se) {
				if (se.SocketErrorCode == SocketError.WouldBlock || se.SocketErrorCode == SocketError.TryAgain)
					return;
				Close ();
				throw se;
			} catch {
				Close ();
				throw;
			}

			if (size == 0) {
				Console.Error.WriteLine ("No data found. Closing socket.");
				Close ();
				return;
			}

			if (read_buffer == null)
				read_buffer = new MemoryStream ();

			read_buffer.Write (ReadChunk, 0, size);
			read_buffer.Flush ();

			if (read_bytes != -1) {
				if (read_buffer.Length >= read_bytes) {
					FinishRead (read_bytes);
				}
			}

			if (read_delimiter != null) {
				int delimiter = FindDelimiter ();
				if (delimiter != -1) {
					FinishRead (delimiter);
				}
			}
		}

		private int FindDelimiter ()
		{
			byte [] data = read_buffer.GetBuffer ();

			int start = 0;

			start = Array.IndexOf (data, read_delimiter [0], start);
			
			while (start > 0) {
				bool match = true;
				for (int i = 1; i < read_delimiter.Length; i++) {
					if (data [start + i] == read_delimiter [i])
						continue;
					match = false;
					break;
				}
				if (match)
					return start + read_delimiter.Length;
				start = Array.IndexOf (data, read_delimiter [0], start + 1);
			}

			return -1;
		}

		private void FinishRead (int end)
		{
			ReadCallback callback = read_callback;
			byte [] data = read_buffer.GetBuffer ();
			byte [] read = new byte [end + 1];

			Array.Copy (data, 0, read, 0, end); 

			read_bytes = -1;
			read_delimiter = null;
			read_callback = null;
			read_buffer.Close ();
			read_buffer = new MemoryStream ();
			read_buffer.Write (data, end, data.Length - end);
			
			callback (this, read);
		}
	}

}

