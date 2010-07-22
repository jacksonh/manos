
using System;

namespace Mango
{
	public class MangoTarget : IMangoTarget
	{
		private MangoAction action;
		
		public MangoTarget (MangoAction action)
		{
			Action = action;
		}
		
		public MangoAction Action {
			get { return action; }
			set {
				if (value == null)
					throw new ArgumentNullException ("action");
				action = value;
			}
		}
		
		public void Invoke (IMangoContext ctx)
		{
			Action (ctx);
		}
	}
}
