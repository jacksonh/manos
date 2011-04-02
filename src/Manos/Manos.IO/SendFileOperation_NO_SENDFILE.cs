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
using System.Net;
using System.Net.Sockets;
using Manos.Collections;
using System.Threading;


// WARNING: This whole file is just a hack so people can develop/debug
// on windows. Once there is a async API for loading / streaming files
// we need to use that instead of this file, which blocks to load the
// entire file and queues it.

namespace Manos.IO {

#if DISABLE_POSIX

        public class SendFileOperation : IWriteOperation
        {
            string fileName;
            WriteCallback callback;
            FileStream fd;
            byte[] transferBuffer = new byte[8192];

            public SendFileOperation(string filename, WriteCallback callback)
            {
                this.fileName = filename;
                this.callback = callback;

                Length = -1;
                fd = null;
            }

            public bool Chunked
            {
                get;
                set;
            }

            public long Length
            {
                get { if (fd == null) return -1; else return fd.Length; }
                set {}
            }

            public void Dispose()
            {
            }

            public void BeginWrite(IOStream stream)
            {
            }

            public bool Combine(IWriteOperation other)
            {
                return false;
            }

            public void EndWrite(IOStream stream)
            {
                fd.Close();
                if (callback != null)
                {
                    callback();
                }
            }

            public void HandleWrite(IOStream stream)
            {
                if (fd == null)
                {
                    fd = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                }
                int len = fd.Read(transferBuffer, 0, transferBuffer.Length);
                ISocketStream sstream = (ISocketStream) stream;
                int err;
                int sent = 0;
                
                var lendata = Chunked ? System.Text.Encoding.ASCII.GetBytes(len.ToString("X") + "\r\n") : null;
                if (len > 0)
                {
                    if (Chunked)
                        sent = sstream.Send(new ByteBufferS[] { new ByteBufferS(lendata, 0, lendata.Length), new ByteBufferS(transferBuffer, 0, len) }, 2, out err);
                    else
                        sent = sstream.Send(new ByteBufferS[] { new ByteBufferS(transferBuffer, 0, len) }, 1, out err);
                }
                else
                {
                    sent = 0;
                    err = 0;
                }

                if (err == 0)
                {
                    if (sent < (lendata == null? 0 :lendata.Length) - len)
                        fd.Seek(sent - len, SeekOrigin.Current);
                    else if (len < transferBuffer.Length)
                    {
                        this.IsComplete = true;
                        if (Completed != null)
                            Completed(this, EventArgs.Empty);
                    }
                }
            }

            public bool IsComplete
            {
                get;
                private set;
            }

            
            public event EventHandler Completed;

            internal void SetLength(long l)
            {
                // not really needed
            }
        }

#endif
    }



