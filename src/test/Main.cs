using System;
using Manos.Managed;
using Manos;
using System.Net;

namespace test
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			AppHost.ListenAt( new IPEndPoint( IPAddress.Parse( "127.0.0.1"), 8080 ));
			AppHost.Start( new HelloWorld.HelloWorld());
		}
	}
}

