using System;

namespace Manos
{
	public abstract class SynchronizedBlock
	{
		public abstract void Run ();
	}
	
	public class SynchronizedBlock<T> : SynchronizedBlock
	{
		internal Action<IManosContext, T> callback;
		internal IManosContext context;
		internal T data;

		public SynchronizedBlock (Action<IManosContext, T> callback, 
			IManosContext context, T data)
		{
			this.data = data;
			this.context = context;
			this.callback = callback;
		}
		
		/// <summary>
		/// Causes the action specified in the constructor to be executed. Infrastructure.
		/// </summary>
		public override void Run ()
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

