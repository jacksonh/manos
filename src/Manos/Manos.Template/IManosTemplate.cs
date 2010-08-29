

using System;

using Manos;
using Manos.Server;

namespace Manos.Templates {
	
	
	public interface IManosTemplate
	{
		void Render (IManosContext context, object the_arg);
		void RenderToResponse (IHttpResponse response, object the_arg);
	}
	
}

