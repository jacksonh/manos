using System;

using Manos;
using Manos.Server;


namespace Manos.Templates.Testing
{
	public class ManosTemplateStub : IManosTemplate
	{
		public ManosTemplateStub ()
		{
		}
	
		public object RenderedArgument {
			get;
			private set;
		}
		
		public void Render (IManosContext context, object the_arg)
		{
			RenderToResponse (context.Response, the_arg);
		}

		public void RenderToResponse (IHttpResponse response, object the_arg)
		{
			RenderedArgument = the_arg;
		}
	}
}

