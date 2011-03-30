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
using System.Net;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using Manos.IO;
using Manos.Http;
using Manos.Caching;
using Manos.Logging;

using Libev;

namespace Manos
{
	/// <summary>
	/// The app runner. This is where the magic happens.
	/// </summary>
	public static class AppHost
	{
		private static ManosApp app;
		private static bool started;
		
		private static List<IPEndPoint> listenEndPoints = new List<IPEndPoint> ();
		private static Dictionary<IPEndPoint, Tuple<string, string>> secureListenEndPoints =
			new Dictionary<IPEndPoint, Tuple<string, string>> ();
		
		private static List<HttpServer> servers = new List<HttpServer> ();
		private static IManosCache cache;
		private static IManosLogger log;
		private static List<IManosPipe> pipes;

		private static ConcurrentQueue<SynchronizedBlock> waitingSyncBlocks = new ConcurrentQueue<SynchronizedBlock> ();
		private static AsyncWatcher syncBlockWatcher;

		private static IOLoop ioloop = IOLoop.Instance;
		
		public static ManosApp App {
			get { return app; }	
		}
		
		public static IManosCache Cache {
			get {
				if (cache == null)
					cache = new ManosInProcCache ();
				return cache;
			}
		}

		public static IManosLogger Log {
			get {
				if (log == null)
					log = new Manos.Logging.ManosConsoleLogger ("manos", LogLevel.Debug);
				return log;
			}
		}

		public static IOLoop IOLoop {
			get { return ioloop; }	
		}
		
		public static IList<IManosPipe> Pipes {
			get { return pipes; }
		}
		
		public static ICollection<IPEndPoint> ListenEndPoints {
			get {
				return listenEndPoints.AsReadOnly ();
			}
		}
		
		public static void ListenAt (IPEndPoint endPoint)
		{
			if (endPoint == null)
				throw new ArgumentNullException ("endPoint");
			
			if (listenEndPoints.Contains (endPoint) || secureListenEndPoints.ContainsKey (endPoint))
				throw new InvalidOperationException ("Endpoint already registered");
			
			listenEndPoints.Add (endPoint);
		}
		
		public static void SecureListenAt (IPEndPoint endPoint, string cert, string key)
		{
			if (endPoint == null)
				throw new ArgumentNullException ("endPoint");
			if (cert == null)
				throw new ArgumentNullException ("cert");
			if (key == null)
				throw new ArgumentNullException ("key");
			
			if (secureListenEndPoints.ContainsKey (endPoint) || listenEndPoints.Contains (endPoint))
				throw new InvalidOperationException ("Endpoint already registered");
			
			secureListenEndPoints.Add (endPoint,
				Tuple.Create (cert, key));
		}
		
		public static void InitializeTLS (string priorities)
		{
			manos_tls_global_init (priorities);
			manos_tls_regenerate_dhparams (1024);
		}
		
		[DllImport ("libmanos", CallingConvention = CallingConvention.Cdecl)]
		private static extern int manos_tls_global_init (string priorities);
		
		[DllImport ("libmanos", CallingConvention = CallingConvention.Cdecl)]
		private static extern int manos_tls_regenerate_dhparams (int bits);
		
		public static void Start (ManosApp application)
		{
			if (application == null)
				throw new ArgumentNullException ("application");
			
			app = application;

			app.StartInternal ();

			started = true;
			
			foreach (var ep in listenEndPoints) {
				var server = new HttpServer (HandleTransaction, new PlainSocketStream (ioloop));
				server.Listen (ep.Address.ToString (), ep.Port);
				
				servers.Add (server);
			}
			foreach (var ep in secureListenEndPoints.Keys) {
				var keypair = secureListenEndPoints [ep];
				var socket = new SecureSocketStream (keypair.Item1, keypair.Item2, ioloop);
				var server = new HttpServer (HandleTransaction, socket);
				server.Listen (ep.Address.ToString (), ep.Port);
				
				servers.Add (server);
			}
			
			syncBlockWatcher = new AsyncWatcher (IOLoop.EventLoop, HandleSynchronizationEvent);
			syncBlockWatcher.Start ();

			ioloop.Start ();
		}
		
		public static void Stop ()
		{
			ioloop.Stop ();
		}
		
		public static void HandleTransaction (IHttpTransaction con)
		{
			app.HandleTransaction (app, con);
		}

		public static void AddPipe (IManosPipe pipe)
		{
			if (pipes == null)
				pipes = new List<IManosPipe> ();
			pipes.Add (pipe);
		}

		public static Timeout AddTimeout (TimeSpan timespan, IRepeatBehavior repeat, object data, TimeoutCallback callback)
		{
			return AddTimeout (timespan, timespan, repeat, data, callback);
		}

		public static Timeout AddTimeout (TimeSpan begin, TimeSpan timespan, IRepeatBehavior repeat, object data, TimeoutCallback callback)
		{
			Timeout t = new Timeout (begin, timespan, repeat, data, callback);
			
			ioloop.AddTimeout (t);

			return t;
		}
		
		public static void RunTimeout (Timeout t)
		{
			t.Run (app);
		}

		public static void Synchronize<T> (Action<IManosContext, T> action,
			IManosContext context, T arg)
		{
			if (action == null) {
				throw new ArgumentNullException ("action");
			}
			if (context == null) {
				throw new ArgumentNullException ("context");
			}
			
			if (context.Transaction.ResponseReady && context.Transaction.Response == null) {
				throw new InvalidOperationException ("Response stream has been closed");
			}
			
			waitingSyncBlocks.Enqueue (new SynchronizedBlock<T> (action, context, arg));
			syncBlockWatcher.Send ();
		}
		
		private static void HandleSynchronizationEvent (Loop loop, AsyncWatcher watcher, EventTypes revents)
		{
			// we don't want to empty the whole queue here, as any number of threads can enqueue
			// sync blocks at will while we try to empty it. only process a fixed number of blocks
			// to not starve other events.
			int pendingBlocks = waitingSyncBlocks.Count;
			
			while (pendingBlocks-- > 0) {
				SynchronizedBlock oneBlock;
				// perhaps we should bail here instead, as a result of 'false' would indicate that
				// somebody is stealing our events.
				if (waitingSyncBlocks.TryDequeue (out oneBlock)) {
					oneBlock.Run ();
				}
			}
		}
	}
	
	public static class SynchronizedExtension
	{
		public static void Synchronize<T> (this IManosContext context,
			Action<IManosContext, T> action, T arg)
		{
			AppHost.Synchronize (action, context, arg);
		}
		
		public static void Synchronize (this IManosContext context,
			Action<IManosContext, object> action, object arg)
		{
			AppHost.Synchronize (action, context, arg);
		}
	}
}

