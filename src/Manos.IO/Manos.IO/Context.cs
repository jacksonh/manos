using System;
using Mono.Unix.Native;
using System.IO;

namespace Manos.IO
{
	public abstract class Context : IDisposable
	{
		protected Context ()
		{
		}

		~Context ()
		{
			Dispose (false);
		}

		private static readonly bool useManagedImpl;

		static Context ()
		{
#if ALWAYS_USE_MANAGED_IO
			useManagedImpl = true;
#else
			useManagedImpl = Environment.OSVersion.Platform == PlatformID.Win32NT
				|| Environment.OSVersion.Platform == PlatformID.Win32S
				|| Environment.OSVersion.Platform == PlatformID.Win32Windows
				|| Environment.OSVersion.Platform == PlatformID.WinCE;
#endif
		}

		public static Context Create ()
		{
			if (useManagedImpl) {
				return new Manos.IO.Managed.Context ();
			} else {
				return new Manos.IO.Libev.Context ();
			}
		}

		public void Dispose ()
		{
			Dispose (true);
			GC.SuppressFinalize (this);
		}

		protected abstract void Dispose (bool disposing);

		public abstract void Start ();

		public abstract void RunOnce ();

		public abstract void RunOnceNonblocking ();

		public abstract void Stop ();

		public abstract ITimerWatcher CreateTimerWatcher (TimeSpan timeout, TimeSpan repeat, Action cb);

		public abstract ITimerWatcher CreateTimerWatcher (TimeSpan timeout, Action cb);

		public abstract IAsyncWatcher CreateAsyncWatcher (Action cb);

		public abstract IPrepareWatcher CreatePrepareWatcher (Action cb);

		public abstract ICheckWatcher CreateCheckWatcher (Action cb);

		public abstract IIdleWatcher CreateIdleWatcher (Action cb);

		public abstract Socket CreateSocket ();

		public abstract Socket CreateSecureSocket (string certFile, string keyFile);

		public abstract Stream OpenFile (string fileName, FileAccess openMode, int blockSize);

		public abstract Stream CreateFile (string fileName, int blockSize);
	}
}

