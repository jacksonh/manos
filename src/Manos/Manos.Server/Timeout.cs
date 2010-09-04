using System;


namespace Manos.Server
{
	public class Timeout
	{
		internal DateTime expires;
		internal TimeSpan span;
		internal IRepeatBehavior repeat;
		internal object data;
		internal TimeoutCallback callback;
		
		public Timeout (DateTime expires, TimeSpan span, IRepeatBehavior repeat, object data, TimeoutCallback callback)
		{
			this.expires = expires;
			this.span = span;
			this.repeat = repeat;
			this.data = data;
			this.callback = callback;
		}
		
		public void Run (ManosApp app)
		{
			try {
				callback (app, data);
			} catch (Exception e) {
				Console.Error.WriteLine ("Exception in timeout callback.");
				Console.Error.WriteLine (e);
			}
			
			repeat.RepeatPerformed ();
		}
		
		public bool ShouldContinueToRepeat ()
		{
			return repeat.ShouldContinueToRepeat ();	
		}
	}
}

