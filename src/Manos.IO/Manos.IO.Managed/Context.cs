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
		private Queue<Action> outstanding;
		private List<PrepareWatcher> prepares;
		private List<CheckWatcher> checks;
		private List<IdleWatcher> idles;
		private List<AsyncWatcher> asyncs;
		private List<TimerWatcher> timers;
		private volatile bool running;
		private object syncRoot = new object ();

		public Context ()
		{
			pulse = new AutoResetEvent (false);
			outstanding = new Queue<Action> ();
			asyncs = new List<AsyncWatcher> ();
			prepares = new List<PrepareWatcher> ();
			checks = new List<CheckWatcher> ();
			idles = new List<IdleWatcher> ();
			timers = new List<TimerWatcher> ();
		}

		internal void Enqueue (Action cb)
		{
			if (cb == null)
				throw new ArgumentNullException ("cb");
			lock (syncRoot) {
				outstanding.Enqueue (cb);
			}
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
			foreach (var prep in prepares.ToArray ()) {
				prep.Invoke ();
			}
			int count = 0;
			lock (this) {
				count = outstanding.Count;
			}
			while (count-- > 0) {
				Action cb;
				lock (this) {
					cb = outstanding.Dequeue ();
				}
				cb ();
			}
			foreach (var idle in idles.ToArray ()) {
				idle.Invoke ();
				pulse.Set ();
			}
			foreach (var check in checks.ToArray ()) {
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

		public override Manos.IO.ITcpSocket CreateTcpSocket (AddressFamily addressFamily)
		{
			return new TcpSocket (this, addressFamily);
		}
		
		public override ITcpServerSocket CreateTcpServerSocket(AddressFamily addressFamily)
		{
			return new TcpSocket (this, addressFamily);
		}

		public override Manos.IO.ITcpSocket CreateSecureSocket (string certFile, string keyFile)
		{
			throw new NotSupportedException ();
		}

		public override IByteStream OpenFile (string fileName, FileAccess openMode, int blockSize)
		{
			var fs = new System.IO.FileStream (fileName, FileMode.Open, openMode, FileShare.ReadWrite, blockSize, true);
			return new FileStream (this, fs, blockSize);
		}

		public override IByteStream CreateFile (string fileName, int blockSize)
		{
			var fs = System.IO.File.Create (fileName);
			return new FileStream (this, fs, blockSize);
		}
		
		public override Manos.IO.IUdpSocket CreateUdpSocket (AddressFamily family)
		{
			return new UdpSocket (this, family);
		}
	}
}

