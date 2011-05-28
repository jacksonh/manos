using System;

namespace Manos.IO
{
	/// <summary>
	/// Async watchers to signal an event in a threadsafe manner and
	/// wake the loop the watcher belongs to.
	/// This is an instance of a watche that may meld multiple event
	/// occurences into one calling.
	/// <para>Corresponding event: the <see cref="Send"/> method of this
	/// watcher was called at least once.</para>
	/// </summary>
	public interface IAsyncWatcher : IBaseWatcher
	{
		/// <summary>
		/// Send a signal to the watcher, waking up the watcher's event
		/// loop if necessary to allow callback invocation. After or
		/// during the callback invocation, the watcher can be signalled again.
		/// </summary>
		void Send ();
	}
}

