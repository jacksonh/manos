using System;
using Manos.Managed;
using Manos;
using System.Net;
using Manos.Threading;
using System.Threading;
using System.Threading.Tasks;

namespace test
{
	class MainClass
	{
		public static void Main( string[] args )
		{
			Task.Factory.StartNew( () => Boundary.Instance.ExecuteOnTargetLoop( () => Thread.Sleep( 500 )));
			
			Task.Factory.StartNew( () => {
				Thread.Sleep( 5000 );
			for( int i = 0; i < 50; i++ )
			{
				int ii = i;
				Boundary.Instance.ExecuteOnTargetLoop( () => 
				                                      { 
					Console.WriteLine( "cheese :" + ii ); 
				} );
			}
			} );
			
			IOLoop.Instance.Start();
		}
	
		public static void MainZ (string[] args)
		{
			AppHost.ListenAt( new IPEndPoint( IPAddress.Parse( "127.0.0.1"), 8080 ));
			AppHost.Start( new HelloWorld.HelloWorld());
		}
	}
}

