using System;
using Mono.Unix.Native;

namespace Manos.IO
{
	public abstract class Context
	{
		protected Context ()
		{
		}

		public abstract void Start ();

		public abstract void RunOnce ();

		public abstract void Stop ();

		public abstract ITimerWatcher CreateTimerWatcher (TimeSpan timeout, TimeSpan repeat, Action cb);

		public abstract ITimerWatcher CreateTimerWatcher (TimeSpan timeout, Action cb);

		public abstract IAsyncWatcher CreateAsyncWatcher (Action cb);

		public abstract IPrepareWatcher CreatePrepareWatcher (Action cb);

		public abstract ICheckWatcher CreateCheckWatcher (Action cb);

		public abstract IIdleWatcher CreateIdleWatcher (Action cb);

		public abstract Socket CreateSocket ();

		public abstract Socket CreateSecureSocket (string certFile, string keyFile);

		public abstract Stream Open (string fileName, int blockSize, OpenFlags openFlags, FilePermissions perms);
	}
}

