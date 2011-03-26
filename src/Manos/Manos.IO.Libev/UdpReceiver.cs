// 
//  Copyright (C) 2011 Robin Duerden (rduerden@gmail.com)
// 
//  Permission is hereby granted, free of charge, to any person obtaining
//  a copy of this software and associated documentation files (the
//  "Software"), to deal in the Software without restriction, including
//  without limitation the rights to use, copy, modify, merge, publish,
//  distribute, sublicense, and/or sell copies of the Software, and to
//  permit persons to whom the Software is furnished to do so, subject to
//  the following conditions:
// 
//  The above copyright notice and this permission notice shall be
//  included in all copies or substantial portions of the Software.
// 
//  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
//  EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
//  MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
//  NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
//  LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
//  OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
//  WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// 
// 
// 
using System;
using Libev;
using Manos.IO;
using System.Runtime.InteropServices;
using System.Net;

namespace Manos.IO.Libev
{
    public class UdpReceiver
    {
        public delegate void ReadCallback( UdpReceiver receiver, byte [] data, int count, IPEndPoint remoteEndPoint );

        private IOLoop loop;
        private IOWatcher watcher;

        private int fd = -1;
        private readonly byte[] readBuffer;
        private readonly int maxMessageSize;

        private ReadCallback readCallback;

        public UdpReceiver( IOLoop loop ) : this( loop, 128*1024 ) {}
        public UdpReceiver( IOLoop loop, int maxMessageSize )
        {
            this.loop = loop;

            readBuffer = new byte[maxMessageSize];
            this.maxMessageSize = maxMessageSize;
        }

        public void Listen( string host, int port )
        {
            int error;
            fd = manos_dgram_socket_listen( host, port, out error );

            if (fd < 0)
                throw new Exception (String.Format ("An error occurred while trying to connect to {0}:{1} errno: {2}", host, port, error));

            watcher = new IOWatcher( new IntPtr( fd ), EventTypes.Read, loop.EventLoop, (l, w, r) => onRead() );
            watcher.Start();
        }

        public void OnRead( ReadCallback callback )
        {
            readCallback = callback;
        }

        private void onRead()
        {
            int size;
            int error;
            SocketInfo socketInfo;

            size = manos_socket_receive_from( fd, readBuffer, maxMessageSize, 0, out socketInfo, out error );
            if( size <= 0 && error != 0 )
            {
                Close();
                return;
            }

            if( readCallback != null )
            {
                IPEndPoint remoteEndPoint = new IPEndPoint( socketInfo.Address, socketInfo.port );
                readCallback( this, readBuffer, size, remoteEndPoint );
            }
        }

        public void Close()
        {
            if( fd == -1 ) return;

            watcher.Dispose();

            int error;
            int res = manos_socket_close (fd, out error);

            if (res < 0) {
                Console.Error.WriteLine ("Error '{0}' closing socket: {1}", error, fd);
                Console.Error.WriteLine (Environment.StackTrace);
            }

            fd = -1;
        }


        [DllImport ("libmanos", CallingConvention = CallingConvention.Cdecl)]
        private static extern int manos_dgram_socket_listen (string host, int port, out int err);

        [DllImport ("libmanos", CallingConvention = CallingConvention.Cdecl)]
        private static extern int manos_socket_close (int fd, out int err);

        [DllImport ("libmanos", CallingConvention = CallingConvention.Cdecl)]
        private static extern int manos_socket_receive_from (int fd, byte [] buffer, int max, int flags, out SocketInfo info, out int err);
    }
}

