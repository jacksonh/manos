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
using System.Net.Sockets;
using System.Collections;
using System.Collections.Generic;

using Manos;
using Manos.Collections;

namespace Manos.IO
{
    public class SendBytesOperation : IWriteOperation
    {
        private ByteBuffer buffer;
        private WriteCallback callback;

        public SendBytesOperation(ByteBuffer buffer, WriteCallback callback)
        {
            this.buffer = buffer;
            this.callback = callback;
        }

        public void Dispose()
        {
        }

        public bool IsComplete
        {
            get;
            private set;
        }

        ISocketStream sstream;

        public void BeginWrite(IIOStream stream)
        {
            sstream = (ISocketStream)stream;
        }

        public void HandleWrite(IIOStream stream)
        {
            while (!IsComplete)
            {
                int len = -1;
                int error;
                len = sstream.Send(buffer, out error);

                if (len > 0)
                {
					if (buffer.Length == len) {
						IsComplete = true;
						if (callback != null) {
							callback ();
						}
					} else {
						buffer.Position += len;
						buffer.Length -= len;
					}
                }
                else
                {
                    return;
                }
            }
        }

        public void EndWrite(IIOStream stream)
        {
        }
    }
}

