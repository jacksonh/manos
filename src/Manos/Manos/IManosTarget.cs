
using System;

namespace Mango
{
	public interface IManosTarget
	{
		ManosAction Action {
			get;
			set;
		}
		
		void Invoke (IManosContext ctx);
	}
}
