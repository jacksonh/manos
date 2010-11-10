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
using System.IO;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Collections;
using System.Collections.Generic;

using Libev;


namespace Manos.IO {

	public delegate void CloseCallback (IOStream stream);
	public delegate void ReadCallback (IOStream stream, byte [] data, int offset, int count);
	public delegate void WriteCallback ();

	public class IOStream {

		private Socket socket;
		private IOLoop ioloop;

		private CloseCallback close_callback;

		private MemoryStream read_buffer;
		private int num_bytes_read;
		private int read_bytes = -1;

		private byte [] read_delimiter;
		private int last_delimiter_check = -1;
		private ReadCallback read_callback;

		private IList<ArraySegment<byte>> write_data;
		private WriteCallback write_callback;

		private FileStream send_file;
		private long send_file_count;
		private long send_file_offset;
		
		private static readonly int DefaultReadChunkSize  = 1024;

		private IOWatcher read_watcher;
		private IOWatcher write_watcher;
		private IntPtr handle;

		public IOStream (Socket socket, IOLoop ioloop)
		{
			this.socket = socket;
			this.ioloop = ioloop;

			TimeOut = TimeSpan.FromMinutes (2);
			Expires = DateTime.UtcNow + TimeOut;

			ReadChunkSize = DefaultReadChunkSize;

			socket.Blocking = false;

            handle = IOWatcher.GetHandle (socket);
			read_watcher = new IOWatcher (handle, EventTypes.Read, ioloop.EventLoop, HandleIORead);
			write_watcher = new IOWatcher (handle, EventTypes.Write, ioloop.EventLoop, HandleIOWrite);
		}

		~IOStream ()
		{
			Close ();
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

		public DateTime Expires {
			get;
			set;
		}

		public TimeSpan TimeOut {
			get;
			private set;
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

		public void ClearReadBuffer ()
		{
			if (read_buffer == null)
				return;
			read_buffer.Position = 0;
		}

		public void OnClose (CloseCallback callback)
		{
			this.close_callback = callback;
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

		public void ReadBytes (ReadCallback callback)
		{
			ReadBytes (-1, callback);
		}

		public void ReadBytes (int num_bytes, ReadCallback callback)
		{
			CheckCanRead ();

			read_bytes = num_bytes;
			read_callback = callback;

			if (read_buffer != null && num_bytes == -1) {
				// If there is some queued data immediately call the callback.
				read_callback (this, read_buffer.GetBuffer (), 0, (int) read_buffer.Position);
				read_buffer.Position = 0;
			} else if (read_buffer != null && read_buffer.Position >= num_bytes) {
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

            IOWatcher.ReleaseHandle (socket, handle);

            handle = IntPtr.Zero;
			socket = null;

			if (close_callback != null)
				close_callback (this);
		}

		private void EnableReading ()
		{
			Expires = DateTime.UtcNow + TimeOut;
			read_watcher.Start ();
		}

		private void EnableWriting ()
		{
			Expires = DateTime.UtcNow + TimeOut;
			write_watcher.Start ();
		}

		public void DisableReading ()
		{
			read_callback = null;
			read_watcher.Stop ();
		}

		public void DisableWriting ()
		{
			write_callback = null;
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
			try {
				HandleRead ();
			} catch (Exception e) {
				Close ();
			}
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
				return;
			} catch (Exception e) {
			  	Console.WriteLine (e);
				Close ();
				return;
			}

			if (size == 0) {
				Close ();
				return;
			}

			if (read_delimiter != null || read_bytes != -1) {
				if (read_buffer == null)
					read_buffer = new MemoryStream ();

				read_buffer.Write (ReadChunk, 0, size);
				num_bytes_read += size;

				if (read_bytes != -1) {
					if (num_bytes_read >= read_bytes) {
						FinishRead (read_bytes);
						return;
					}
				} if (read_delimiter != null) {
					int delimiter = FindDelimiter ();
					if (delimiter != -1) {
						FinishRead (delimiter);
						return;
					}
				}
			}

			// We are doing an indefinite read
			read_callback (this, ReadChunk, 0, size);
		}
#if DISABLE_POSIX
        private void HandleSendFile()
#else 
        private Action sendFileAction;

        private void HandleSendFile ()
        {
            if (sendFileAction == null) LoadSendFile ();
            sendFileAction ();
        }

        private void LoadSendFile ()
        {
            if (Environment.OSVersion.Platform == PlatformID.Win32NT ||
                Environment.OSVersion.Platform == PlatformID.Win32S ||
                Environment.OSVersion.Platform == PlatformID.Win32Windows ||
                Environment.OSVersion.Platform == PlatformID.WinCE ||
                Environment.OSVersion.Platform == PlatformID.Xbox)            
                sendFileAction = new Action (SimpleHandleSendFile);
            else
                sendFileAction = new Action (PosixHandleSendFile);
        }

        private void SimpleHandleSendFile ()
#endif
        {
            byte[] data = new byte[4096];
            while (send_file_offset < send_file_count) {
                int len = -1;
                try {
                    int toRead = (int) (send_file_count > data.Length ? (long) data.Length : send_file_count);
                    int read = send_file.Read (data, 0, toRead);
                    if (read <= 0)
                        break;
                    len = socket.Send(data, 0, read, SocketFlags.None);
                    send_file_offset += read;
                }
                catch (SocketException se) {
                    if (se.SocketErrorCode == SocketError.WouldBlock || se.SocketErrorCode == SocketError.TryAgain)
                        return;
                    Close();
                }
                catch (Exception e) {
                    Close();
                }
                finally {
                    if (len != -1)
                        AdjustSegments (len, write_data);
                }
            }

            if (write_data.Count == 0)
                FinishWrite();
        }

#if !DISABLE_POSIX // option to get rid of the Mono.Posix reference all together
		private void PosixHandleSendFile ()
		{
			//
			// TODO: Need to handle WOULDBLOCK here.
			// 
			
			while (send_file_offset < send_file_count) {
			      try {
				      Mono.Unix.Native.Syscall.sendfile (socket.Handle.ToInt32 (), 
						      send_file.Handle.ToInt32 (), 
						      ref send_file_offset,
						      (ulong) (send_file_count - send_file_offset));
			      } catch (SocketException se) {
				      if (se.SocketErrorCode == SocketError.WouldBlock || se.SocketErrorCode == SocketError.TryAgain)
					      return;
				      Close ();
			      } catch (Exception e) {
				      Close ();
			      }
			}

			if (send_file_offset >= send_file_count)
				FinishSendFile ();
		}
#endif		
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
			    } catch (Exception e) {
				    Close ();
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
			return ByteUtils.FindDelimiter (read_delimiter, data, start, (int) read_buffer.Position);
		}

		private void FinishRead (int end)
		{
			ReadCallback callback = read_callback;
			byte [] data = read_buffer.GetBuffer ();
			byte [] read = new byte [end];

			Array.Copy (data, 0, read, 0, end); 

			int length = (int) read_buffer.Position;

			read_bytes = -1;
			read_delimiter = null;
			last_delimiter_check = -1;
			read_callback = null;

			read_buffer.Position = 0;
			num_bytes_read = length - end;
			read_buffer.Write (data, end, num_bytes_read);
			
			callback (this, read, 0, end);
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

