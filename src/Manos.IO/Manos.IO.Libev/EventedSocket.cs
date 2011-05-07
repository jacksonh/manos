using System;
using Libev;

namespace Manos.IO.Libev
{
	abstract class EventedSocket : Socket
	{
		public EventedSocket (Loop loop)
		{
			if (loop == null)
				throw new ArgumentNullException ("loop");
			
			this.Loop = loop;
		}

		protected EventedSocket (Loop loop, SocketInfo info)
			: this (loop)
		{
			address = info.Address.ToString ();
			port = info.port;
		}


		public Loop Loop {
			get;
			private set;
		}
	}
}

