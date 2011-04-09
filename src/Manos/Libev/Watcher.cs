using System;
using System.Runtime.InteropServices;
using Manos;

namespace Libev
{
    public abstract class Watcher : BaseWatcher
    {
        protected IntPtr watcher_ptr;
        private bool disposed;
        protected GCHandle gc_handle;

        internal Watcher(LibEvLoop loop) : base(loop)
        {
            Loop = loop;
            gc_handle = GCHandle.Alloc(this);
        }

        
        public new LibEvLoop Loop
        {
            get;
            private set;
        }

        
        ~Watcher()
        {
            if (watcher_ptr != IntPtr.Zero)
            {
                Dispose();
            }
        }

        public override void Dispose()
        {
            if (disposed)
            {
                throw new ObjectDisposedException(GetType().Name);
            }

            Stop();

            DestroyWatcher();

            watcher_ptr = IntPtr.Zero;
            gc_handle.Free();

            GC.SuppressFinalize(this);
            disposed = true;
        }

        public override void Start()
        {
            if (running)
                return;

            running = true;

            StartImpl();
        }

        public override void Stop()
        {
            if (!running)
                return;

            running = false;

            StopImpl();
        }

        protected abstract void StartImpl();

        protected abstract void StopImpl();

        protected abstract void DestroyWatcher();

        protected abstract void UnmanagedCallbackHandler(IntPtr loop, IntPtr watcher, EventTypes revents);
    }
}

