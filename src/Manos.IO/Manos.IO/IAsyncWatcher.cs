using System;

namespace Manos.IO
{
	public interface IAsyncWatcher : IBaseWatcher
	{
		void Send ();
	}
}

