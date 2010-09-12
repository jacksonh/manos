using System;

using Manos.Server;
using Manos.Routing;

namespace Manos
{
	public interface IManosPipe
	{
		
		void OnPreProcessRequest (ManosApp app, IHttpTransaction transaction);

		IManosTarget OnPreProcessTarget (IManosContext ctx);

		void OnPostProcessTarget (IManosContext ctx, IManosTarget target);

		void OnPostProcessRequest (ManosApp app, IHttpTransaction transaction);
		
		void OnError (IManosContext ctx);
		
	}
}

