using System;
using Libev;

namespace Manos.IO.Libev
{
	abstract class EventedSocket : Socket
	{
		public EventedSocket (Context context)
			: base (context)
		{
		}

		protected EventedSocket (Context context, SocketInfo info)
			: this (context)
		{
			address = info.Address.ToString ();
			port = info.port;
		}

		public new Context Context {
			get { return (Context) base.Context; }
		}
	}
}

