using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Manos
{
    public abstract class Loop: IDisposable
    {
        private static readonly bool _windows;

        static Loop()
        {
            _windows =
                Environment.OSVersion.Platform == PlatformID.Win32NT ||
                Environment.OSVersion.Platform == PlatformID.Win32S ||
                Environment.OSVersion.Platform == PlatformID.Win32Windows ||
                Environment.OSVersion.Platform == PlatformID.WinCE;
        }

        public static bool IsWindows { get { return _windows; } }

        public virtual void Dispose() { }
    }
}
