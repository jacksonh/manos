using System;


namespace Manos.Routing
{
	public class ActionTarget : IManosTarget
	{
		private ManosAction action;
		
		public ActionTarget (ManosAction action)
		{
			Action = action;
		}
	
		public void Invoke (ManosApp app, IManosContext ctx)
		{
			action (ctx);
		}

		public Delegate Action {
			get { return action; }
			set {
				if (value == null)
					throw new ArgumentNullException ("action");
				ManosAction a = value as ManosAction;
				if (a == null)
					throw new InvalidOperationException ("A ManosActionTarget::Action can only be of type ManosAction.");
				action = a;
			}
		}
	}
}

