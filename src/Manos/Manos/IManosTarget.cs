
using System;

namespace Mango
{
	public interface IMangoTarget
	{
		MangoAction Action {
			get;
			set;
		}
		
		void Invoke (IMangoContext ctx);
	}
}
