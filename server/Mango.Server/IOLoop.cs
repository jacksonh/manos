

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

		private int epfd;
		private bool running;

		private Queue<EpollEvent> events = new Queue<EpollEvent> ();

		private List<IOCallback> callbacks = new List<IOCallback> ();
		private Dictionary<IntPtr,IOHandler> handlers = new Dictionary<IntPtr,IOHandler> ();

		private EpollEvents EPOLL_ERROR = EpollEvents.EPOLLERR | EpollEvents.EPOLLHUP | EpollEvents.EPOLLRDHUP;


		public IOLoop ()
		{
			epfd = Syscall.epoll_create (MAX_EVENTS);
		}

		public void Start ()
		{
			// int res = Syscall.epoll_ctl (epfd, EpollOp.EPOLL_CTL_ADD, (int) socket.Handle, EpollEvents.EPOLLIN);

			running = true;
			while (true) {
				int timeout = 2;

				RunCallbacks ();

				if (callbacks.Count > 0)
					timeout = 0;

				if (!running)
					break;
				
				var new_events = Syscall.epoll_wait (epfd, timeout);
				RunHandlers (new_events);
			}
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

		private void RunHandlers (EpollEvent [] new_events)
		{
			foreach (EpollEvent e in new_events) {
				events.Enqueue (e);
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
			Register (fd, events | EPOLL_ERROR);
		}

		public void UpdateHandler (IntPtr fd, EpollEvents events)
		{
			Modify (fd, events | EPOLL_ERROR);
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
			Syscall.epoll_ctl (epfd, EpollOp.EPOLL_CTL_ADD, (int) fd, events);
		}

		
		private void Modify (IntPtr fd, EpollEvents events)
		{
			Syscall.epoll_ctl (epfd, EpollOp.EPOLL_CTL_MOD, (int) fd, events);
		}

		private void Unregister (IntPtr fd)
		{
			Syscall.epoll_ctl (epfd, EpollOp.EPOLL_CTL_DEL, (int) fd, 0);
		}
		
	}
}

