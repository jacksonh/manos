using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using Manos.Collections;

namespace Manos.IO
{

    public delegate void ReadCallback(IOStream stream, byte[] data, int offset, int count);
    public delegate void WriteCallback();

    public interface IOStream
    {
        event EventHandler Error;
        event EventHandler Closed;
        event EventHandler TimedOut;

        void QueueWriteOperation (IWriteOperation op);
        void ReadBytes (ReadCallback callback);

        void Close ();
    }

    public static class IOStreamUtilities
    {
        /// This could use some tuning, but the basic idea is that we need to remove
        /// all of the data that has been sent already.
        public static void AdjustSegments(int len, IList<ByteBuffer> write_data)
        {
            var remove = new List<ByteBuffer>();
            int total = 0;
            for (int i = 0; i < write_data.Count; i++)
            {
                int seg_len = write_data[i].Length;
                if (total + seg_len <= len)
                {
                    // The entire segment was written so we can pop it 
                    remove.Add(write_data[i]);

                    // If we finished exactly at the end of this segment we are done adjusting
                    if (total + seg_len == len)
                        break;
                }
                else if (total + seg_len > len)
                {
                    // Move to the point in the segment where we stopped writing

                    int offset = write_data[i].Position + (len - total);
                    write_data[i].Position = offset;
                    write_data[i].Length = write_data[i].Bytes.Length - offset;
                    break;
                }

                total += seg_len;
            }

            foreach (var segment in remove)
            {
                write_data.Remove(segment);
            }
        }

    }

    public interface SocketStream : IOStream, IDisposable
    {
        void Connect (string host, int port);
		void Connect (int port);
		void Listen (string host, int port);
        string Address { get; }
        int Port { get; }
        event Action<Manos.IO.SocketStream> Connected;
        event EventHandler<ConnectionAcceptedEventArgs> ConnectionAccepted;

        void Write (byte[] data, WriteCallback callback);
        void Write (byte[] data, int offset, int count, WriteCallback callback);
        int Send (ByteBufferS[] buffers, int length, out int error);
        int SendFile (string name, bool chunked, long length, Action<long, int> callback);

    }
}
