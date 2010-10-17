
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
			
		public virtual void OnPreProcessRequest (ManosApp app, IHttpTransaction transaction)
		{
		}

		public virtual IManosTarget OnPreProcessTarget (IManosContext ctx)
		{
			return null;
		}

		public virtual void OnPostProcessTarget (IManosContext ctx, IManosTarget target)
		{
		}

		public virtual void OnPostProcessRequest (ManosApp app, IHttpTransaction transaction)
		{
		}
		
		public virtual void OnError (IManosContext ctx)
		{
		}
	}
}

