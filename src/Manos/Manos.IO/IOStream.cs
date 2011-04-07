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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using Manos.Collections;

namespace Manos.IO
{

    public delegate void ReadCallback(IIOStream stream, byte[] data, int offset, int count);
    public delegate void WriteCallback();

    public interface IIOStream
    {
        event EventHandler Error;
        event EventHandler Closed;
        event EventHandler TimedOut;

        IOLoop IOLoop { get; }
        void QueueWriteOperation (IWriteOperation op);
        void ReadBytes (ReadCallback callback);

        void Close ();

        void DisableWriting();
        void EnableWriting();
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

    public interface ISocketStream : IIOStream, IDisposable
    {
        IntPtr Handle { get; }
        void Connect (string host, int port);
		void Connect (int port);
		void Listen (string host, int port);
        string Address { get; }
        int Port { get; }
        event Action<Manos.IO.ISocketStream> Connected;
        event EventHandler<ConnectionAcceptedEventArgs> ConnectionAccepted;

        void Write (byte[] data, WriteCallback callback);
        void Write (byte[] data, int offset, int count, WriteCallback callback);
        int Send (ByteBuffer buffer, out int error);

        ISendFileOperation MakeSendFile (string file);

        //int SendFile (string name, bool chunked, long length, Action<long, int> callback);
    }
}
