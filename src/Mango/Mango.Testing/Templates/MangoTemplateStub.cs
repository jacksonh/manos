using System;

using Mango;
using Mango.Server;


namespace Mango.Templates.Testing
{
	public class MangoTemplateStub : IMangoTemplate
	{
		public MangoTemplateStub ()
		{
		}
	
		public object RenderedArgument {
			get;
			private set;
		}
		
		public void Render (IMangoContext context, object the_arg)
		{
			RenderToResponse (context.Response, the_arg);
		}

		public void RenderToResponse (IHttpResponse response, object the_arg)
		{
			RenderedArgument = the_arg;
		}
	}
}

