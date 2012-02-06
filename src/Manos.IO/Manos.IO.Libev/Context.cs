using System;
using Libev;
using System.Runtime.InteropServices;
using Mono.Unix.Native;
using System.IO;

namespace Manos.IO.Libev
{
	class Context : Manos.IO.Context
	{
		public Context ()
		{
			Loop = new Loop ();
			Eio = new EioContext (Loop);
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

		public override ITcpSocket CreateTcpSocket (AddressFamily addressFamily)
		{
			return new TcpSocket (this, addressFamily);
		}
		
		public override ITcpServerSocket CreateTcpServerSocket (AddressFamily addressFamily)
		{
			return new TcpSocket (this, addressFamily);
		}

		public override ITcpSocket CreateSecureSocket (string certFile, string keyFile)
		{
			throw new NotSupportedException ();
		}

		public override IByteStream OpenFile (string fileName, OpenMode openMode, int blockSize)
		{
			OpenFlags openFlags = 0;
			switch (openMode) {
				case OpenMode.Read:
					openFlags = OpenFlags.O_RDONLY;
					break;
						
				case OpenMode.ReadWrite:
					openFlags = OpenFlags.O_RDWR;
					break;
						
				case OpenMode.Write:
					openFlags = OpenFlags.O_WRONLY;
					break;
						
				default:
					throw new ArgumentException ("openMode");
			}
			return FileStream.Open (this, fileName, blockSize, openFlags);
		}

		public override IByteStream CreateFile (string fileName, int blockSize)
		{
			return FileStream.Create (this, fileName, blockSize);
		}
		
		public override Manos.IO.IUdpSocket CreateUdpSocket (AddressFamily family)
		{
			return new UdpSocket (this, family);
		}

		public override INotifier CreateNotifier (Action callback)
		{
			return new Notifier (this, callback);
		}
	}
}

