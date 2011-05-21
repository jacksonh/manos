using System;

namespace Manos.IO.Managed
{
	abstract class ManagedStream : Manos.IO.Stream
	{
		internal ManagedStream (Context ctx)
			: base (ctx)
		{
		}
		
		public new Context Context {
			get { return (Context) base.Context; }
		}
		
		protected void Enqueue (Action action)
		{
			lock (this) {
				Context.Enqueue (action);
			}
		}
	}
}

