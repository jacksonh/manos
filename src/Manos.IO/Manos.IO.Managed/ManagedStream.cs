using System;

namespace Manos.IO.Managed
{
	abstract class ManagedStream<TFragment> : FragmentStream<TFragment>
		where TFragment : class
	{
		internal ManagedStream (Context ctx)
			: base (ctx)
		{
		}
		
		public new Context Context {
			get { return (Context) base.Context; }
		}
	}
}

