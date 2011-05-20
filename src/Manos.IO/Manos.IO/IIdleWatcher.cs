using System;

namespace Manos.IO
{
	/// <summary>
	/// Idle watchers are invoked on every loop iteration without
	/// waiting for external events. Thus, one continuously active
	/// idle watcher will cause the event loop never enter a resting
	/// state.
	/// <para>Corresponding event: none. The idle watcher just executes
	/// whenever it can.</para>
	/// </summary>
	public interface IIdleWatcher : IBaseWatcher
	{
	}
}

