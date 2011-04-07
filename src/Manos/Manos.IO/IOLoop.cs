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
using Libeio;


namespace Manos.IO {

    public abstract class IOLoop
    {
        private static IOLoop instance;

        public static IOLoop Instance
        {
            get
            {
                if (instance == null)
                    if (LibEvLoop.IsWindows)
                        instance = new Managed.IOLoop();
                    else
                        instance = new Libev.IOLoop();
                return instance;
            }
            set
            {
                if (instance != null) throw new ArgumentException("Cannot set the instance once it''s been used");
                instance = value;
            }
        }

        public abstract Loop EventLoop
        {
            get;
        }

        public abstract void Start();

        public abstract void Stop();

        public abstract void AddTimeout(Timeout timeout);

        public abstract IAsyncWatcher NewAsyncWatcher(AsyncWatcherCallback cb);

        public abstract ISocketStream CreateSocketStream ();

        public abstract ISocketStream CreateSecureSocket (string certFile, string keyFile);
    }
}

