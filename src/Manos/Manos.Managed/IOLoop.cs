using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Manos.IO;
using System.Net;
using System.Threading;
using Libev;
using System.Net.Sockets;
using System.Collections.Concurrent;

namespace Manos.Managed
{
    class ManagedLoop : Loop {

        private IOLoop owner;
        public IOLoop Owner { get { return owner; } }

        public ManagedLoop(IOLoop owner)
        {
            this.owner = owner;
        }
    }

    public class IOLoop: Manos.IO.IOLoop
    {
        private Loop loop;

        public IOLoop()
        {
            Synchronize = true;
            loop = new ManagedLoop(this);
        }


        public bool Synchronize { get; set; }
        private volatile bool stop;
        private ConcurrentQueue<Action> actions = new  ConcurrentQueue<Action>();
        private AutoResetEvent ev = new AutoResetEvent(false);

        public override Loop EventLoop
        {
            get { return loop; }
        }


        public override void Start()
        {
            while (!stop)
            {
                ev.WaitOne();
                Action act;
                while (actions.TryDequeue(out act))
                {
                    act();
                }
            }
        }

        public override void Stop()
        {
            stop = true;
            ev.Set();
        }

        public void BlockInvoke(Action t)
        {
            if (Synchronize)
            {
                object o = new object();
                bool done = false;
                NonBlockInvoke(delegate()
                {
                    done = true;
                    Monitor.Pulse(o);
                });
                while (!done)
                    Monitor.Wait(o);
            }
            else
                t();
        }

        public void NonBlockInvoke(Action t)
        {
            if (Synchronize)
            {
                actions.Enqueue(t);
                ev.Set();
            }
            else
            {
                t();
            }
        }

        public override void AddTimeout(Timeout timeout) 
        {
            new Timer((a) =>
            {
                BlockInvoke(delegate()
                {
                    timeout.Run(null);
                    if (!timeout.ShouldContinueToRepeat())
                        ((Timer)a).Dispose();
                });
            }, null, timeout.begin, timeout.span);
        }

        public override IAsyncWatcher NewAsyncWatcher(AsyncWatcherCallback cb)
        {
            return new AsyncWatcher(loop, cb);
        }

        public override Manos.IO.Socket CreateSocketStream()
        {
            return new Socket(this);
        }

        public override Manos.IO.Socket CreateSecureSocket(string certFile, string keyFile)
        {
            throw new NotSupportedException ();
        }
    }
}
