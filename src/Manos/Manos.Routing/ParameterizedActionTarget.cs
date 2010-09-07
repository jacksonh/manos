
using System;

namespace Manos
{
	public class ManosTarget : IManosTarget
	{
		private ManosAction action;
		
		public ManosTarget (ManosAction action)
		{
			Action = action;
		}
		
		public ManosAction Action {
			get { return action; }
			set {
				if (value == null)
					throw new ArgumentNullException ("action");
				action = value;
			}
		}
		
		public void Invoke (IManosContext ctx)
		{
			Action (ctx);
		}
	}
}
