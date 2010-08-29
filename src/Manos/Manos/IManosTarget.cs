
using System;

namespace Manos
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
