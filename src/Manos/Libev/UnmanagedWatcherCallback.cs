

using System;
using System.Runtime.InteropServices;

namespace Libev {
    [UnmanagedFunctionPointer (System.Runtime.InteropServices.CallingConvention.Cdecl)]
	internal delegate void UnmanagedWatcherCallback (IntPtr loop, IntPtr watcher, int revents);
}

