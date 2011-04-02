using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Libev;
using System.Runtime.InteropServices;

namespace Manos.IO.Libev
{
    public class IOLoop : Manos.IO.IOLoop
    {
        private bool running;

        private LibEvLoop evloop;
        private Libeio.Libeio eio;
        private IntPtr libmanos_data;

        public IOLoop()
        {
            evloop = LibEvLoop.CreateDefaultLoop(0);
            eio = new Libeio.Libeio();

            //			eio.Initialize (evloop);

            libmanos_data = manos_init(evloop.Handle);
        }


        public override Loop EventLoop
        {
            get { return evloop; }
        }

        public Libeio.Libeio Eio
        {
            get { return eio; }
        }

        public override void Start()
        {
            running = true;

            evloop.RunBlocking();
        }

        public override void Stop()
        {
            running = false;
        }

        public override void AddTimeout(Timeout timeout)
        {
            TimerWatcher t = new TimerWatcher(timeout.begin, timeout.span, evloop, HandleTimeout);
            t.UserData = timeout;
            t.Start();
        }

        private void HandleTimeout(LibEvLoop loop, TimerWatcher timeout, EventTypes revents)
        {
            Timeout t = (Timeout)timeout.UserData;

            AppHost.RunTimeout(t);
            if (!t.ShouldContinueToRepeat())
                timeout.Stop();
        }

        [DllImport("libmanos", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr manos_init(IntPtr handle);

        public override IAsyncWatcher NewAsyncWatcher(AsyncWatcherCallback cb)
        {
            return new AsyncWatcher(EventLoop, cb);
        }

        public override IO.ISocketStream CreateSocketStream()
        {
            return new Manos.IO.Libev.SocketStream (this);
        }
    }
}
