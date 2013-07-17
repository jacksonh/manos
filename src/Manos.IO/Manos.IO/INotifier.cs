using System;

namespace Manos.IO
{
	public interface INotifier : IBaseWatcher
	{
		void Notify();
	}
}

