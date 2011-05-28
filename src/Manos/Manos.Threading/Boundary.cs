// 
//  Copyright (C) 2011 Robin Duerden (rduerden@gmail.com)
// 
//  Permission is hereby granted, free of charge, to any person obtaining
//  a copy of this software and associated documentation files (the
//  "Software"), to deal in the Software without restriction, including
//  without limitation the rights to use, copy, modify, merge, publish,
//  distribute, sublicense, and/or sell copies of the Software, and to
//  permit persons to whom the Software is furnished to do so, subject to
//  the following conditions:
// 
//  The above copyright notice and this permission notice shall be
//  included in all copies or substantial portions of the Software.
// 
//  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
//  EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
//  MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
//  NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
//  LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
//  OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
//  WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// 
// 
// 
using System;
using Manos.IO;
using Libev;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Concurrent;

namespace Manos.Threading
{
	public class Boundary : IBoundary
	{
		public static readonly Boundary Instance;

		static Boundary ()
		{
			Instance = new Boundary (AppHost.Context);
		}

		private readonly IAsyncWatcher asyncWatcher;
		private readonly ConcurrentQueue<Action> workQueue;
		private int maxWorkPerLoop;

		public Boundary (Context context) : this (context, 18)
		{
		}

		public Boundary (Context context, int maxWorkPerLoop)
		{
			asyncWatcher = context.CreateAsyncWatcher (ProcessWork);
			asyncWatcher.Start ();
			
			workQueue = new ConcurrentQueue<Action> ();
			this.maxWorkPerLoop = maxWorkPerLoop;
		}

		public void ExecuteOnTargetLoop (Action action)
		{
			workQueue.Enqueue (action);
			
			asyncWatcher.Send ();
		}

		private void ProcessWork ()
		{
			int remaining = maxWorkPerLoop + 1;
			while (--remaining > 0) {
				Action action;
				if (workQueue.TryDequeue (out action)) {
					try {
						action ();
					} catch (Exception ex) {
						Console.WriteLine ("Error in processing synchronized action");
						Console.WriteLine (ex);
					}
				} else
					break;
			}

			if (remaining == 0)
				asyncWatcher.Send ();
		}
	}
}

