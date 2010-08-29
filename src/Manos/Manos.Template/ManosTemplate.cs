using System;

using Mango;
using Mango.Server;


namespace Mango.Templates
{
	public abstract class MangoTemplate
	{
		public void Render (IManosContext context, object the_arg)
		{
			RenderToResponse (context.Response, the_arg);
		}
		
		public abstract void RenderToResponse (IHttpResponse response, object the_arg);
	}
}

