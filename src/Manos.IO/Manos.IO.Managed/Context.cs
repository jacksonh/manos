using System;
using System.Threading;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using Mono.Unix.Native;

namespace Manos.IO.Managed
{
	class Context : Manos.IO.Context
	{
		private AutoResetEvent pulse;
		private ConcurrentQueue<Action> outstanding;
		private ConcurrentQueue<Action> processing;
		private List<PrepareWatcher> prepares;
		private List<CheckWatcher> checks;
		private List<IdleWatcher> idles;
		private List<AsyncWatcher> asyncs;
		private List<TimerWatcher> timers;
		private volatile bool running;
		private ManagedFileOperations fileOps;

		public Context ()
		{
			pulse = new AutoResetEvent (false);
			outstanding = new ConcurrentQueue<Action> ();
			processing = new ConcurrentQueue<Action> ();
			asyncs = new List<AsyncWatcher> ();
			prepares = new List<PrepareWatcher> ();
			checks = new List<CheckWatcher> ();
			idles = new List<IdleWatcher> ();
			timers = new List<TimerWatcher> ();
			fileOps = new ManagedFileOperations (this);
		}

		internal void Enqueue (Action cb)
		{
			outstanding.Enqueue (cb);
			pulse.Set ();
		}

		internal void Remove (AsyncWatcher async)
		{
			asyncs.Remove (async);
		}

		internal void Remove (PrepareWatcher prepare)
		{
			prepares.Remove (prepare);
		}

		internal void Remove (CheckWatcher check)
		{
			checks.Remove (check);
		}

		internal void Remove (IdleWatcher check)
		{
			idles.Remove (check);
		}

		internal void Remove (TimerWatcher timer)
		{
			timers.Remove (timer);
		}

		protected override void Dispose (bool disposing)
		{
			if (pulse != null) {
				pulse.Dispose ();
				
				Dispose (ref checks);
				Dispose (ref prepares);
				Dispose (ref idles);
				Dispose (ref timers);
				
				outstanding = null;
				processing = null;
				checks = null;
				prepares = null;
				idles = null;
				timers = null;
			}
		}

		static void Dispose<T> (ref List<T> items)
			where T : IBaseWatcher
		{
			var localItems = items;
			items = new List<T> ();
			foreach (var item in localItems) {
				item.Dispose ();
			}
		}

		public override void Start ()
		{
			running = true;
			while (running) {
				RunOnce ();
			}
		}

		public override void RunOnce ()
		{
			pulse.WaitOne ();
			RunOnceNonblocking ();
		}

		public override void RunOnceNonblocking ()
		{
			if (processing.Count == 0) {
				processing = Interlocked.Exchange (ref outstanding, processing);
			}
			foreach (var prep in prepares) {
				prep.Invoke ();
			}
			while (processing.Count > 0) {
				Action cb;
				processing.TryDequeue (out cb);
				cb ();
			}
			foreach (var idle in idles) {
				idle.Invoke ();
				pulse.Set ();
			}
			foreach (var check in checks) {
				check.Invoke ();
			}
		}

		public override void Stop ()
		{
			running = false;
		}

		public override IAsyncWatcher CreateAsyncWatcher (Action cb)
		{
			var result = new AsyncWatcher (this, cb);
			asyncs.Add (result);
			return result;
		}

		public override ICheckWatcher CreateCheckWatcher (Action cb)
		{
			var result = new CheckWatcher (this, cb);
			checks.Add (result);
			return result;
		}

		public override IIdleWatcher CreateIdleWatcher (Action cb)
		{
			var result = new IdleWatcher (this, cb);
			idles.Add (result);
			return result;
		}

		public override IPrepareWatcher CreatePrepareWatcher (Action cb)
		{
			var result = new PrepareWatcher (this, cb);
			prepares.Add (result);
			return result;
		}

		public override ITimerWatcher CreateTimerWatcher (TimeSpan timeout, Action cb)
		{
			return CreateTimerWatcher (timeout, TimeSpan.Zero, cb);
		}

		public override ITimerWatcher CreateTimerWatcher (TimeSpan timeout, TimeSpan repeat, Action cb)
		{
			var result = new TimerWatcher (this, cb, timeout, repeat);
			timers.Add (result);
			return result;
		}

		public override Manos.IO.Socket CreateSocket ()
		{
			return new Socket (this);
		}

		public override Manos.IO.Socket CreateSecureSocket (string certFile, string keyFile)
		{
			throw new NotSupportedException ();
		}
		
		class ManagedFileOperations : FileOperations
		{
			private Context parent;

			public ManagedFileOperations (Context parent)
			{
				this.parent = parent;
			}

			public override Stream Open (string fileName, int blockSize, Mono.Unix.Native.OpenFlags openFlags, Mono.Unix.Native.FilePermissions perms)
			{
				FileAccess access = FileAccess.ReadWrite;
				OpenFlags mask = OpenFlags.O_RDONLY | OpenFlags.O_RDWR | OpenFlags.O_WRONLY;
				if ((openFlags & mask) == OpenFlags.O_RDWR) {
					access = FileAccess.ReadWrite;
				} else if ((openFlags & mask) == OpenFlags.O_RDONLY) {
					access = FileAccess.Read;
				} else if ((openFlags & mask) == OpenFlags.O_WRONLY) {
					access = FileAccess.Write;
				} 
				var fs = new System.IO.FileStream (fileName, FileMode.Open, access, FileShare.ReadWrite, 0x1000, true);
				return new FileStream (parent, fs, blockSize);
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

