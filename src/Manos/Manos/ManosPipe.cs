
using System;

using Manos.Server;
using Manos.Routing;

namespace Manos
{
	public class ManosPipe : IManosPipe
	{
		public ManosPipe ()
		{
		}
			
		public void OnPreProcessRequest (ManosApp app, IHttpTransaction transaction)
		{
		}

		public IManosTarget OnPreProcessTarget (IManosContext ctx)
		{
			return null;
		}

		public void OnPostProcessTarget (IManosContext ctx, IManosTarget target)
		{
		}

		public void OnPostProcessRequest (ManosApp app, IHttpTransaction transaction)
		{
		}
		
		public void OnError (IManosContext ctx)
		{
		}
	}
}

