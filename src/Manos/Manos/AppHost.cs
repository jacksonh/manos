

using System;
using System.Net;
using System.Collections;
using System.Collections.Generic;

using Manos.Server;
using Manos.Caching;


namespace Manos
{
	public static class AppHost
	{
		private static ManosApp app;
		private static bool started;
		
		private static int port = 8080;
		private static IPAddress ip_address = IPAddress.Parse ("0.0.0.0");
		
		private static HttpServer server;
		private static IManosCache cache = new ManosInProcCache ();
		private static List<IManosPipe> pipes = new List<IManosPipe> ();
		private static IOLoop ioloop = IOLoop.Instance;
		
		public static ManosApp App {
			get { return app; }	
		}
		
		public static HttpServer Server {
			get { return server; }	
		}
		
		public static IManosCache Cache {
			get { return cache; }	
		}
		
		public static IOLoop IOLoop {
			get { return ioloop; }	
		}
		
		public static IList<IManosPipe> Pipes {
			get { return pipes; }	
		}
		
		public static IPAddress IPAddress {
			get { return ip_address; }
			set {
				if (started)
					throw new InvalidOperationException ("IPAddress can not be changed once the server has been started.");
				if (value == null)
					throw new ArgumentNullException ("value");
				ip_address = value;
			}
		}
		
		public static int Port {
			get { return port; }
			set {
				if (started)
					throw new InvalidOperationException ("Port can not be changed once the server has been started.");
				if (port <= 0)
					throw new ArgumentOutOfRangeException ("Invalid port specified, port must be a positive integer.");
				port = value;
			}
		}
		
		public static void Start (ManosApp application)
		{
			if (application == null)
				throw new ArgumentNullException ("application");
			
			app = application;
			
			started = true;
			server = new HttpServer (HandleTransaction, ioloop);
			
			IPEndPoint endpoint = new IPEndPoint (IPAddress, port);
			
			Server.Bind (endpoint);
			
			server.Start ();
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
		
		public static void AddTimeout (TimeSpan timespan, IRepeatBehavior repeat, object data, TimeoutCallback callback)
		{
			Timeout t = new Timeout (DateTime.UtcNow + timespan, timespan, repeat, data, callback);
			
			ioloop.AddTimeout (t);
		}
		
		public static void RunTimeout (Timeout t)
		{
			t.Run (app);	
		}
	}
}

