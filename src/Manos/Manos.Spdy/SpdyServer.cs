using System;
using System.IO;
using System.Text;
using System.Net;
using System.Linq;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;

using Libev;

using Manos.IO;
using Manos.Http;

namespace Manos.Spdy
{

    public delegate void SpdyConnectionCallback(IHttpTransaction transaction);

    public class SpdyServer : IDisposable
    {

        public static readonly string ServerVersion;

        private SpdyConnectionCallback callback;
        Socket socket;
        private bool closeOnEnd;

        static SpdyServer()
        {
            Version v = Assembly.GetExecutingAssembly().GetName().Version;
            ServerVersion = "Manos/" + v.ToString();
        }

        public SpdyServer(Context context, SpdyConnectionCallback callback, Socket socket, bool closeOnEnd = false)
        {
            this.callback = callback;
            this.socket = socket;
            this.closeOnEnd = closeOnEnd;
			this.Context = context;
        }

        public Context Context
        {
			get;
			private set;
        }

        public void Listen(string host, int port)
        {
            socket.Listen(host, port, ConnectionAccepted);
        }

        public void Dispose()
        {
            if (socket != null) {
                socket.Dispose();
                socket = null;
            }
        }

        private void ConnectionAccepted(Socket socket)
        {
            var t = new SpdySession(socket, callback);
        }
    }
}