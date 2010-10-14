
using System;
using System.IO;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Collections;
using System.Collections.Generic;

using Libev;
using Mono.Unix.Native;


namespace Manos.Server {

	public delegate void ReadCallback (IOStream stream, byte [] data);
	public delegate void WriteCallback ();

	public class IOStream {

		private Socket socket;
		private IOLoop ioloop;

		private MemoryStream read_buffer;
		private int read_bytes = -1;
		private byte [] read_delimiter;
		private int last_delimiter_check = -1;
		private ReadCallback read_callback;

		private IList<ArraySegment<byte>> write_data;
		private WriteCallback write_callback;

		private FileStream send_file;
		private long send_file_count;
		private long send_file_offset;
		
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

		private IOWatcher read_watcher;
		private IOWatcher write_watcher;

		public IOStream (Socket socket, IOLoop ioloop)
		{
			this.socket = socket;
			this.ioloop = ioloop;

			ReadChunkSize = DefaultReadChunkSize;

			socket.Blocking = false;

			read_watcher = new IOWatcher (socket.Handle, EventTypes.Read, ioloop.EventLoop, HandleIORead);
			write_watcher = new IOWatcher (socket.Handle, EventTypes.Write, ioloop.EventLoop, HandleIOWrite);
		}

		public IOLoop IOLoop {
			get { return ioloop; }
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
			get { return write_callback != null; }
		}

		public bool IsClosed {
			get { return socket == null || !socket.Connected; }
		}

		public void ReadUntil (string delimiter, ReadCallback callback)
		{
			CheckCanRead ();

			read_delimiter = Encoding.ASCII.GetBytes (delimiter);
			read_callback = callback;

			int di = FindDelimiter ();
			if (di != -1) {
				FinishRead (di);
				return;
			}

			EnableReading ();
		}

		public void ReadBytes (int num_bytes, ReadCallback callback)
		{
			CheckCanRead ();

			read_bytes = num_bytes;
			read_callback = callback;

			if (read_buffer != null && read_buffer.Length >= num_bytes) {
				FinishRead (num_bytes);
				return;
			}
			
			EnableReading ();
		}

		public void Write (IList<ArraySegment<byte>> data, WriteCallback callback)
		{
			CheckCanWrite ();

			write_data = data;
			write_callback = callback;

			EnableWriting ();
		}

		public void SendFile (string file, WriteCallback callback)
		{
			CheckCanRead ();
			
			write_callback = callback;
			
			send_file = new FileStream (file, FileMode.Open, FileAccess.Read);
			send_file_offset = 0;
			send_file_count = send_file.Length;

			EnableWriting ();
		}

		public void Close ()
		{			
			if (socket == null)
				return;			

			DisableReading ();
			DisableWriting ();

			socket.Close ();
			socket = null;
		}

		private void EnableReading ()
		{
			read_watcher.Start ();
		}

		private void EnableWriting ()
		{
			write_watcher.Start ();
		}

		public void DisableReading ()
		{
			read_watcher.Stop ();
		}

		public void DisableWriting ()
		{
			write_watcher.Stop ();
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
			if (IsWriting)
				throw new Exception ("Attempt to write while already performing a write operation.");
			if (IsClosed)
				throw new Exception ("Attempt to write on a closed socket.");
		}

		private void HandleIORead (Loop loop, IOWatcher watcher, int revents)
		{
			HandleRead ();
		}
		
		private void HandleIOWrite (Loop loop, IOWatcher watcher, int revents)
		{
			// write ready can still be raised after we are done writing.
			if (send_file == null && write_data == null)
			   return;

			if (send_file != null) {
			   HandleSendFile ();
			   return;
			}

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
			} catch (Exception e) {
			  	Console.WriteLine (e);
				Close ();
				throw;
			}

			if (size == 0) {
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

		private void HandleSendFile ()
		{
			//
			// TODO: Need to handle WOULDBLOCK here.
			// 
			
			while (send_file_offset < send_file_count) {
			      try {
			      	  Syscall.sendfile (socket.Handle.ToInt32 (), 
			          		    send_file.Handle.ToInt32 (), 
			                  	    ref send_file_offset,
			                  	    (ulong) (send_file_count - send_file_offset));
			      } catch (SocketException se) {
				   if (se.SocketErrorCode == SocketError.WouldBlock || se.SocketErrorCode == SocketError.TryAgain)
					return;
				   Close ();
				   throw se;
			      }
			}

			if (send_file_offset >= send_file_count)
				FinishSendFile ();
		}
		
		private void HandleWrite ()
		{
			while (write_data.Count > 0) {
			    int len = -1;
			    try {
			        len = socket.Send (write_data);
		            } catch (SocketException se) {
				if (se.SocketErrorCode == SocketError.WouldBlock || se.SocketErrorCode == SocketError.TryAgain)
					return;
				Close ();
				throw se;
			    } finally {
			        if (len != -1)
			            AdjustSegments (len, write_data);
			    }
			}

			if (write_data.Count == 0)
				FinishWrite ();
		}

		/// This could use some tuning, but the basic idea is that we need to remove
		/// all of the data that has been sent already.
		public static void AdjustSegments (int len, IList<ArraySegment<byte>> write_data)
		{
			var remove = new List<ArraySegment<byte>>  ();
			int total = 0;
			for (int i = 0; i < write_data.Count; i++) {
				int seg_len = write_data [i].Count;
				if (total + seg_len <= len) {
					// The entire segment was written so we can pop it 
					remove.Add (write_data [i]);

					// If we finished exactly at the end of this segment we are done adjusting
					if (total + seg_len == len)
						break;
				} else if (total + seg_len > len) {
					// Move to the point in the segment where we stopped writing

					int offset = write_data [i].Offset + (len - total);
					write_data [i] = new ArraySegment<byte> (write_data [i].Array,
							offset,
							write_data [i].Array.Length - offset);
					break;
				}
					
				total += seg_len;
			}

			foreach (var segment in remove) {
				write_data.Remove (segment);
			}
		}

		private int FindDelimiter ()
		{
			if (read_buffer == null)
				return -1;

			byte [] data = read_buffer.GetBuffer ();

			int start = Math.Max (0, last_delimiter_check - read_delimiter.Length);

			last_delimiter_check = read_bytes;
			return ByteUtils.FindDelimiter (read_delimiter, data, start);
		}

		private void FinishRead (int end)
		{
			ReadCallback callback = read_callback;
			byte [] data = read_buffer.GetBuffer ();
			byte [] read = new byte [end];

			Array.Copy (data, 0, read, 0, end); 

			int length = (int) read_buffer.Length;

			read_bytes = -1;
			read_delimiter = null;
			last_delimiter_check = -1;
			read_callback = null;
			read_buffer = new MemoryStream ();
			read_buffer.Write (data, end, length - end);
			
			callback (this, read);
		}

		private void FinishWrite ()
		{
			WriteCallback callback = write_callback;

			write_data = null;
			write_callback = null;

			callback ();
		}
		
		private void FinishSendFile ()
		{
			WriteCallback callback = write_callback;
			write_callback = null;
			
			send_file.Close ();
			send_file = null;

			send_file_count = 0;
			send_file_offset = 0;

			callback ();
		}
	}

}

