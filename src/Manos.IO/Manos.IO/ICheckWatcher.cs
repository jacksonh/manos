using System;

namespace Manos.IO
{
	/// <summary>
	/// Check watchers are invoked after all other watchers.
	/// <para>Corresponding event: event loop about to enter resting state</para>
	/// </summary>
	public interface ICheckWatcher : IBaseWatcher
	{
	}
}

