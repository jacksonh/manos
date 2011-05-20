using System;

namespace Manos.IO
{
	/// <summary>
	/// Prepare watchers are invoked before any other watchers.
	/// <para>Corresponding event: event loop woken up by some other event</para>
	/// </summary>
	public interface IPrepareWatcher : IBaseWatcher
	{
	}
}

