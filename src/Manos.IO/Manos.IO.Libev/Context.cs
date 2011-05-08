using System;
using Libev;
using System.Runtime.InteropServices;
using Mono.Unix.Native;

namespace Manos.IO.Libev
{
	class Context : Manos.IO.Context
	{
		private LibevFileOperations fileOps;

		public Context ()
		{
			Loop = new Loop ();
			Eio = new EioContext (Loop);
			fileOps = new LibevFileOperations (this);
		}

		protected override void Dispose (bool disposing)
		{
			if (Loop != null) {
				Eio.Dispose ();
				Loop.Dispose ();
			}
		}

		public Loop Loop {
			get;
			private set;
		}

		public EioContext Eio {
			get;
			private set;
		}

		public override void Start ()
		{
			Loop.RunBlocking ();
		}

		public override void RunOnce ()
		{
			Loop.RunOneShot ();
		}

		public override void RunOnceNonblocking ()
		{
			Loop.RunNonBlocking ();
		}

		public override void Stop ()
		{
			Loop.Unloop (UnloopType.All);
		}

		public override ITimerWatcher CreateTimerWatcher (TimeSpan timeout, Action cb)
		{
			return CreateTimerWatcher (timeout, TimeSpan.Zero, cb);
		}

		public override ITimerWatcher CreateTimerWatcher (TimeSpan timeout, TimeSpan repeat, Action cb)
		{
			return new TimerWatcher (timeout, repeat, Loop, delegate {
				cb ();
			});
		}

		public override IAsyncWatcher CreateAsyncWatcher (Action cb)
		{
			return new AsyncWatcher (Loop, delegate {
				cb ();
			});
		}

		public override ICheckWatcher CreateCheckWatcher (Action cb)
		{
			return new CheckWatcher (Loop, delegate {
				cb ();
			});
		}

		public override IIdleWatcher CreateIdleWatcher (Action cb)
		{
			return new IdleWatcher (Loop, delegate {
				cb ();
			});
		}

		public override IPrepareWatcher CreatePrepareWatcher (Action cb)
		{
			return new PrepareWatcher (Loop, delegate {
				cb ();
			});
		}

		public override Socket CreateSocket ()
		{
			return new PlainSocket (this);
		}

		public override Socket CreateSecureSocket (string certFile, string keyFile)
		{
			return new SecureSocket (this, certFile, keyFile);
		}
		
		class LibevFileOperations : FileOperations
		{
			Context parent;

			public LibevFileOperations (Context parent)
			{
				this.parent = parent;
			}

			public override Stream Open (string fileName, int blockSize, OpenFlags openFlags, FilePermissions perms)
			{
				return FileStream.Open (parent, fileName, blockSize, openFlags, perms);
			}

			public override long GetLength (string fileName)
			{
				return FileStream.GetLength (fileName);
			}
		}
		
		public override FileOperations File {
			get { return fileOps; }
		}
	}
}

