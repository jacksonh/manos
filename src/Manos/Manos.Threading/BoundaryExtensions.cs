using System;
namespace Manos.Threading
{
	public static class BoundaryExtensions
	{
		public static void Synchronize( this IManosContext context, Action action )
		{
			Boundary.Instance.ExecuteOnTargetLoop( action );
		}
	}
}

