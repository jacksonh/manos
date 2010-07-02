
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
	public delegate void WriteCallback ();

	public class IOStream {

		private Socket socket;
		private IOLoop ioloop;
		private EpollEvents state;

		private MemoryStream read_buffer;
		private MemoryStream write_buffer;

		private int read_bytes = -1;
		private byte [] read_delimiter;
		private ReadCallback read_callback;

		private int write_index;
		private List<WriteOperation> write_operations;

		private static readonly int DefaultReadChunkSize  = 4096;

		private class WriteOperation {
			public int index;
			public WriteCallback callback;

			public WriteOperation (int index, WriteCallback callback)
			{
				this.index = index;
				this.callback = callback;
			}
		}

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

		public bool IsWriting {
			get { return write_operations != null; }
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

		public void Write (byte [] data, WriteCallback callback)
		{
			CheckCanWrite ();

			if (write_buffer == null)
				write_buffer = new MemoryStream (data.Length);
			write_buffer.Write (data, 0, data.Length);

			if (write_operations == null)
				write_operations = new List<WriteOperation> ();
			write_operations.Add (new WriteOperation (write_index + data.Length, callback));

			AddIOState (IOLoop.EPOLL_WRITE_EVENTS);
		}

		public void SendFile (string file)
		{
			socket.SendFile (file);
		}

		public void Close ()
		{
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

		private void CheckCanWrite ()
		{
			if (IsClosed)
				throw new Exception ("Attempt to write on a closed socket.");
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

			if ((events & IOLoop.EPOLL_WRITE_EVENTS) != 0)
				HandleWrite ();
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

		private void HandleWrite ()
		{
			//
			// TODO:  This really sucks, eventually I think I'll move to a list of byte [] chunks
			// to reduce the number of copies/buffer resizes needed. However, for now just to get
			// things working we use a MemoryStream for the write buffer.
			//
			// The reason I am procrastinating on the chunk list is because I want to get a better
			// idea of the amount of data that is sent typically and the number/size of writes 
			// being made. 
			//

			byte [] write_data = write_buffer.GetBuffer ();
			int ind = socket.Send (write_data, write_index, write_data.Length - write_index, SocketFlags.None);

			write_index += ind;

			var ops = new List<WriteOperation> (write_operations);
			foreach (WriteOperation op in ops) {
				if (op.index <= write_index) {
					FinishWriteOperation (op);
					write_operations.Remove (op);
				}
			}

			if (write_operations.Count == 0)
				FinishWrite ();
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
	
		private void FinishWriteOperation (WriteOperation op)
		{
			op.callback ();
		}

		private void FinishWrite ()
		{
			write_index = 0;
			write_buffer.Close ();
			write_operations = null;
		}
	}

}

