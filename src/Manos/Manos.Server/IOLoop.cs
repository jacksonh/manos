

using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Collections;
using System.Collections.Generic;

using Mono.Unix.Native;


namespace Mango.Server {

	public delegate void IOCallback ();
	public delegate void IOHandler (IntPtr fd, EpollEvents events);

	public class IOLoop {

		public static readonly int MAX_EVENTS = 24;

		public static readonly EpollEvents EPOLL_READ_EVENTS = EpollEvents.EPOLLIN;
		public static readonly EpollEvents EPOLL_WRITE_EVENTS = EpollEvents.EPOLLOUT;
		public static readonly EpollEvents EPOLL_ERROR_EVENTS = EpollEvents.EPOLLERR | EpollEvents.EPOLLHUP | EpollEvents.EPOLLRDHUP;

		private int epfd;
		private bool running;

		private Queue<EpollEvent> events = new Queue<EpollEvent> ();

		private List<IOCallback> callbacks = new List<IOCallback> ();
		private Dictionary<IntPtr,IOHandler> handlers = new Dictionary<IntPtr,IOHandler> ();


		public IOLoop ()
		{
			epfd = Syscall.epoll_create (MAX_EVENTS);
		}

		public void Start ()
		{
			running = true;
			
			EpollEvent [] new_events = new EpollEvent [MAX_EVENTS];
			while (true) {
				int timeout = 5000;

				RunCallbacks ();

				if (callbacks.Count > 0)
					timeout = 0;

				if (!running)
					break;
				
				int num_events = Syscall.epoll_wait (epfd, new_events, MAX_EVENTS, timeout);
				
				if (num_events == -1)
					throw new Exception ("Something catastrophic happened.");
				
				RunHandlers (new_events, num_events);
			}
			running = false;
		}

		public void Stop ()
		{
			running = false;
		}

		private void RunCallbacks ()
		{
			List<IOCallback> callbacks = new List<IOCallback> (this.callbacks);

			foreach (IOCallback callback in callbacks) {

				// A callback can remove another callback
				if (!this.callbacks.Contains (callback))
					continue;

				RunCallback (callback);
			}
		}

		private void RunHandlers (EpollEvent [] new_events, int num_events)
		{
			for (int i = 0; i < num_events; i++) {
				events.Enqueue (new_events [i]);
			}

			while (events.Count > 0) {
				var e = events.Dequeue ();

				RunHandler (e.fd, e.events);
			}
		}

		private void RunHandler (IntPtr fd, EpollEvents events)
		{
			handlers [fd] (fd, events);
		}
		
		public void AddHandler (IntPtr fd, IOHandler handler, EpollEvents events)
		{
			handlers [fd] = handler;
			Register (fd, events | EPOLL_ERROR_EVENTS);
		}

		public void UpdateHandler (IntPtr fd, EpollEvents events)
		{
			
			Modify (fd, events | EPOLL_ERROR_EVENTS);
		}

		public void RemoveHandler (IntPtr fd)
		{
			handlers.Remove (fd);
			// events.Remove (fd);

			Unregister (fd);
		}

		public void AddCallback (IOCallback callback)
		{
			callbacks.Add (callback);
		}

		public void RemoveCallback (IOCallback callback)
		{
			callbacks.Remove (callback);
		}

		public void RunCallback (IOCallback callback)
		{
			try {
				callback ();
			} catch (Exception e) {
				HandleCallbackException (e);
			}
		}

		public void HandleCallbackException (Exception e)
		{
			Console.WriteLine ("Exception in callback");
			Console.WriteLine (e);
		}


		private void Register (IntPtr fd, EpollEvents events)
		{
			Syscall.epoll_ctl (epfd, EpollOp.EPOLL_CTL_ADD, fd, events);
		}

		
		private void Modify (IntPtr fd, EpollEvents events)
		{
			Syscall.epoll_ctl (epfd, EpollOp.EPOLL_CTL_MOD, fd, events);
		}

		private void Unregister (IntPtr fd)
		{
			Syscall.epoll_ctl (epfd, EpollOp.EPOLL_CTL_DEL, fd, 0);
		}
		
	}
}

