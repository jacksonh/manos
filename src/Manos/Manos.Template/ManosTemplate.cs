using System;

using Manos;
using Manos.Server;


namespace Manos.Templates
{
	public abstract class ManosTemplate
	{
		public void Render (IManosContext context, object the_arg)
		{
			RenderToResponse (context.Response, the_arg);
		}
		
		public abstract void RenderToResponse (IHttpResponse response, object the_arg);
	}
}

