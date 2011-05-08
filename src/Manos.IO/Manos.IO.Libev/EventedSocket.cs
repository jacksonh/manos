using System;
using Libev;

namespace Manos.IO.Libev
{
	abstract class EventedSocket : Socket
	{
		public EventedSocket (Context context)
		{
			if (context == null)
				throw new ArgumentNullException ("context");
			
			this.Context = context;
		}

		protected EventedSocket (Context context, SocketInfo info)
			: this (context)
		{
			address = info.Address.ToString ();
			port = info.port;
		}


		public Context Context {
			get;
			private set;
		}
	}
}

