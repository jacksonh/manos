using System;
using Libev;
using System.Threading;
using System.Collections.Concurrent;
using System.Runtime.InteropServices;

namespace Manos.IO.Libev
{
	class EioContext : IDisposable
	{
		static Thread eioHandlerThread;
		static Loop eioLoop;

		static EioContext ()
		{
			eioLoop = new Loop ();
			manos_init (eioLoop.Handle);
			
			eioHandlerThread = new Thread (EioHandler);
			eioHandlerThread.Start ();
		}

		static void EioHandler ()
		{
			eioLoop.Run (LoopType.Blocking);
		}

		[DllImport("libmanos", CallingConvention = CallingConvention.Cdecl)]
		private static extern void manos_init (IntPtr handle);

		private AsyncWatcher pulse;
		private Action<AsyncWatcher, EventTypes> eioHandlerCb;
		private ConcurrentQueue<Action> outstanding;

		public EioContext (Loop parent)
		{
			eioHandlerCb = EioHandler;
			outstanding = new ConcurrentQueue<Action> ();
			pulse = new AsyncWatcher (parent, eioHandlerCb);
			pulse.Start ();
		}

		public void Dispose ()
		{
			if (eioHandlerCb != null) {
				pulse.Dispose ();
				eioHandlerCb = null;
				outstanding = null;
				GC.SuppressFinalize (this);
			}
		}

		~EioContext ()
		{
			Dispose ();
		}

		void EioHandler (AsyncWatcher watcher, EventTypes events)
		{
			var count = outstanding.Count;
			while (count-- > 0) {
				Action cb;
				outstanding.TryDequeue (out cb);
				try {
					cb ();
				} catch (Exception e) {
					Console.WriteLine ("Exception in eio callback:");
					Console.WriteLine (e.StackTrace);
				}
			}
		}

		public void Read (int fd, byte[] buffer, long offset, long length, Action<int, byte[], int> callback)
		{
			Libeio.read (fd, buffer, offset, length, (arg1, arg2, arg3) => {
				outstanding.Enqueue (() => callback (arg1, arg2, arg3));
				pulse.Send ();
			});
		}

		public void Write (int fd, byte[] buffer, long offset, long length, Action<int, int> callback)
		{
			Libeio.write (fd, buffer, offset, length, (arg1, arg2) => {
				outstanding.Enqueue (() => callback (arg1, arg2));
				pulse.Send ();
			});
		}

		public void SendFile (int out_fd, int in_fd, long offset, long length, Action<long, int> callback)
		{
			Libeio.sendfile (out_fd, in_fd, offset, length, (arg1, arg2) => {
				outstanding.Enqueue (() => callback (arg1, arg2));
				pulse.Send ();
			});
		}
	}
}

