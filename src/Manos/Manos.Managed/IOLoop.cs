using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Manos.IO;
using System.Net;
using System.Threading;
using Libev;
using System.Net.Sockets;

namespace Manos.Managed
{
    class Loop : BaseLoop {

        private IOLoop owner;
        public IOLoop Owner { get { return owner; } }

        public Loop(IOLoop owner)
        {
            this.owner = owner;
        }
    }

    public class IOLoop: Manos.IO.IOLoop
    {
        private Loop loop;

        public IOLoop()
        {
            loop = new Loop(this);
        }
    

        public override BaseLoop EventLoop
        {
            get { return loop; }
        }

        private AutoResetEvent ev = new AutoResetEvent(false);

        public override void Start()
        {
            ev.WaitOne();
        }

        public override void Stop()
        {
            ev.Set();
        }

        public override void AddTimeout(Timeout timeout) 
        {
            new Timer((a) =>
            {
                timeout.Run(null);
                if (!timeout.ShouldContinueToRepeat())
                    ((Timer)a).Dispose();
            }, null, timeout.begin, timeout.span);
        }

        public override IAsyncWatcher NewAsyncWatcher(AsyncWatcherCallback cb)
        {
            return new AsyncWatcher(loop, cb);
        }

        public override ISocketStream CreateSocketStream()
        {
            return new SocketStream();
        }
    }
}
