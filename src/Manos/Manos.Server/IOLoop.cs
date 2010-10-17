//
// Copyright (C) 2010 Jackson Harper (jackson@manosdemono.com)
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//
//



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

		public void Start ()
		{
			running = true;
			
			evloop.RunBlocking ();
		}

		public void Stop ()
		{
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

