
using System;

namespace Manos.Routing
{
	public interface IManosTarget
	{
		Delegate Action {
			get;
			set;
		}
		
		void Invoke (ManosApp app, IManosContext ctx);
	}
}
