

using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using Libev;


namespace Manos.Server {

	public class IOLoop {
	       
		private static IOLoop instance = new IOLoop ();
		
		private bool running;

		private Loop evloop;
		private PrepareWatcher prepare_watcher;


		public IOLoop ()
		{
			evloop = Loop.CreateDefaultLoop (0);

			prepare_watcher = new PrepareWatcher (evloop, HandlePrepareEvent);
			prepare_watcher.Start ();
		}

		public static IOLoop Instance {
			get { return instance; }
		}

		public Loop EventLoop {
		       get { return evloop; }
		}
		
		public void QueueTransaction (HttpTransaction trans)
		{
			//
			// Since the switch to libev, it seems best to just run them
			// for now I'll leave the queueing function call in, so its
			// easy to experiment later.

			trans.Run ();
		}
		
		public void Start ()
		{
			running = true;
			
			evloop.RunBlocking ();
		}

		public void Stop ()
		{
			// This will need to tickle the loop so it wakes up and calls 
			// the prepare handler.

			running = false;
		}

		private void HandlePrepareEvent (Loop loop, PrepareWatcher watcher, int revents)
		{
			if (!running) {
			   loop.Unloop (UnloopType.All);
			   prepare_watcher.Stop ();
		        }
		}

		public void AddTimeout (Timeout timeout)
		{
			TimerWatcher t = new TimerWatcher (timeout.begin, timeout.span, evloop, HandleTimeout);
			t.UserData = timeout;
			t.Start ();
		}

		private void HandleTimeout (Loop loop, TimerWatcher timeout, int revents)
		{
			Timeout t = (Timeout) timeout.UserData;

			AppHost.RunTimeout (t);
			if (!t.ShouldContinueToRepeat ())
			   timeout.Stop ();
		}
	}
}

