using System;
using System.Runtime.InteropServices;
using Manos;

namespace Libev
{
    public class CheckWatcher : Watcher
    {
        private CheckWatcherCallback callback;
        private static UnmanagedWatcherCallback unmanaged_callback;

        static CheckWatcher()
        {
            unmanaged_callback = StaticCallback;
        }

        public CheckWatcher(LibEvLoop loop, CheckWatcherCallback callback)
            : base(loop)
        {
            this.callback = callback;

            watcher_ptr = manos_check_watcher_create(unmanaged_callback, GCHandle.ToIntPtr(gc_handle));
        }

        protected override void DestroyWatcher()
        {
            manos_check_watcher_destroy(watcher_ptr);
        }

        private static void StaticCallback(IntPtr data, EventTypes revents)
        {
            try
            {
                var handle = GCHandle.FromIntPtr(data);
                var watcher = (CheckWatcher)handle.Target;
                watcher.callback(watcher.Loop, watcher, revents);
            }
            catch (Exception e)
            {
                Console.Error.WriteLine("Error handling check event: {0}", e.Message);
                Console.Error.WriteLine(e.StackTrace);
            }
        }

        protected override void StartImpl()
        {
            ev_check_start(Loop.Handle, watcher_ptr);
        }

        protected override void StopImpl()
        {
            ev_check_stop(Loop.Handle, watcher_ptr);
        }

        [DllImport("libev", CallingConvention = CallingConvention.Cdecl)]
        private static extern void ev_check_start(IntPtr loop, IntPtr watcher);

        [DllImport("libev", CallingConvention = CallingConvention.Cdecl)]
        private static extern void ev_check_stop(IntPtr loop, IntPtr watcher);

        [DllImport("libmanos", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr manos_check_watcher_create(UnmanagedWatcherCallback callback, IntPtr data);

        [DllImport("libmanos", CallingConvention = CallingConvention.Cdecl)]
        private static extern void manos_check_watcher_destroy(IntPtr watcher);
    }

    [UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
    public delegate void CheckWatcherCallback(Loop loop, CheckWatcher watcher, EventTypes revents);
}

