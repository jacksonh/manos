using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Manos.IO;

namespace Manos
{
    public abstract class BaseWatcher: IBaseWatcher, IDisposable
    {
        public BaseWatcher(Loop loop)
        {
            this.Loop = loop;
        }
        protected bool running;
		
        public bool IsRunning
        {
            get { return running; }
        }

        public Loop Loop
        {
            get;
            private set;
        }

        public object Tag
        {
            get;
            set;
        }

        public virtual void Dispose () { }

        public abstract void Start ();
        public abstract void Stop ();

    }
}
