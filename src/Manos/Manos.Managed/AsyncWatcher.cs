using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Libev;

namespace Manos.Managed
{
    class AsyncWatcher: BaseWatcher, IAsyncWatcher
    {
        private AsyncWatcherCallback cb;
        public AsyncWatcher(BaseLoop loop, AsyncWatcherCallback cb)
            : base(loop)
        {
            this.cb = cb;
        }

        public override void Start()
        {
        }

        public override void Stop()
        {
        }

        public void Send()
        {
            cb.Invoke(Loop, this, EventTypes.None);
        }
    }
}
