using System;

namespace Manos.IO.Managed
{
	abstract class ManagedStream : Manos.IO.Stream
	{
		Context context;
		
		internal ManagedStream (Context ctx)
		{
			if (ctx == null)
				throw new ArgumentNullException ("ctx");
			context = ctx;
		}
		
		protected void Enqueue (Action action)
		{
			lock (this) {
				context.Enqueue (action);
			}
		}
	}
}

