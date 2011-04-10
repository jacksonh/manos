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

namespace Manos.Threading
{
    public class Boundary
    {
        public static readonly Boundary Instance = new Boundary (IOLoop.Instance);
        
        private readonly AsyncWatcher asyncWatcher;

        private readonly Queue<Action> workQueue;

        private int maxWorkPerLoop;

        public Boundary( IOLoop loop ) : this( loop, 18 ) {}
        public Boundary( IOLoop loop, int maxWorkPerLoop )
        {
            asyncWatcher = new AsyncWatcher ((LibEvLoop)loop.EventLoop, ( l, w, et ) => processWork());
            asyncWatcher.Start ();

            workQueue = new Queue<Action> ();
            this.maxWorkPerLoop = maxWorkPerLoop;
        }

        public void ExecuteOnTargetLoop (Action action)
        {
            int count;

            lock (workQueue) {
                workQueue.Enqueue (action);

                count = workQueue.Count;
            }

            if (count < 2) asyncWatcher.Send ();
        }

        private readonly List<Action> scratch = new List<Action> ();
        private void processWork ()
        {
            int remaining;

            lock (workQueue) {
                while ((remaining = workQueue.Count) > 0 &&
                         scratch.Count < maxWorkPerLoop) {
                    scratch.Add (workQueue.Dequeue ());
                }
            }

            int count = scratch.Count;
            for (int i = 0; i< count; i++)
            {
                try {
                    scratch[i]();
                }
                catch (Exception ex) {
                    Console.WriteLine (ex);
                }
            }

            scratch.Clear ();

            if (remaining > 0) asyncWatcher.Send ();
        }
    }
}

