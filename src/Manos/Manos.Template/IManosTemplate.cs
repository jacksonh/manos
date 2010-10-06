

using System;
using System.IO;

using Manos;
using Manos.Server;

namespace Manos.Templates {
	
	public interface IManosTemplate
	{
		void Render (IManosContext context, Stream stream, object the_arg);
	}	
}

