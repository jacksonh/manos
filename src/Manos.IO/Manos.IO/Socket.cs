using System;
using System.Net;

namespace Manos.IO
{
	abstract class Socket<TFragment, TStream, TEndPoint> : IStreamSocket<TFragment, TStream, TEndPoint>
		where TFragment : class
		where TStream : IStream<TFragment>
		where TEndPoint : EndPoint
	{
		public Socket (Context context, AddressFamily addressFamily)
		{
			this.AddressFamily = addressFamily;
			this.Context = context;
		}
		
		public AddressFamily AddressFamily {
			get;
			private set;
		}
		
		public Context Context {
			get;
			private set;
		}
		
		public bool IsConnected {
			get;
			protected set;
		}
		
		public bool IsBound {
			get;
			protected set;
		}
		
		public abstract TEndPoint LocalEndpoint {
			get;
		}
		
		public abstract TEndPoint RemoteEndpoint {
			get;
		}
		
		public abstract void Bind (TEndPoint endpoint);
		
		public abstract void Connect (TEndPoint endpoint, Action callback, Action<Exception> error);
		
		public abstract TStream GetSocketStream ();
		
		public virtual void Close ()
		{
			Dispose ();
		}
		
		~Socket ()
		{
			Dispose (false);
		}
		
		public virtual void Dispose ()
		{
			Dispose (true);
			GC.SuppressFinalize (this);
		}
		
		protected virtual void Dispose (bool disposing)
		{
		}
	}
}

