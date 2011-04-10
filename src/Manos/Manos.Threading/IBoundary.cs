using System;
namespace Manos.Threading
{
	public interface IBoundary
	{
		void ExecuteOnTargetLoop (Action action);
	}
}

