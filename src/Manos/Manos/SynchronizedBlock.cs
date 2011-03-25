using System;

namespace Manos
{
	public class SynchronizedBlock
	{
		internal Action<IManosContext, object> callback;
		internal IManosContext context;
		internal object data;

		public SynchronizedBlock (Action<IManosContext, object> callback, 
			IManosContext context, object data)
		{
			this.data = data;
			this.context = context;
			this.callback = callback;
		}
		
		/// <summary>
		/// Causes the action specified in the constructor to be executed. Infrastructure.
		/// </summary>
		public void Run ()
		{
			if (context.Response != null) {
				try {
					callback (context, data);
				} catch (Exception e) {
					Console.Error.WriteLine ("Exception in synchronous block.");
					Console.Error.WriteLine (e);
				}
			}
		}
	}
}

