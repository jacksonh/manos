using System;

namespace Manos.IO.Libev
{
	public abstract class EventedSocket : Socket
	{
		public EventedSocket (IOLoop loop)
		{
			if (loop == null)
				throw new ArgumentNullException ("loop");
			
			this.Loop = loop;
		}

		protected EventedSocket (IOLoop loop, SocketInfo info)
			: this (loop)
		{
			address = info.Address.ToString ();
			port = info.port;
		}


		public IOLoop Loop {
			get;
			private set;
		}
	}
}

