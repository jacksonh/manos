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
