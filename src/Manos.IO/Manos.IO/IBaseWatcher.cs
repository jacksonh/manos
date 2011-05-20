using System;

namespace Manos.IO
{
	/// <summary>
	/// Watchers represent a subscription to an event that can be active (started)
	/// or inactive (stopped).
	/// Each watcher instance manages a unique subscription, independent of all other
	/// watcher instances.
	/// <para>After the event has occured, the watcher callback is invoked
	/// at least once. Occurences of the watched event and callback invocations
	/// need not be 1-to-1, depending on the concrete watcher.</para>
	/// <para>Callbacks are only invoked for started watchers.</para>
	/// <remarks>Generally, operations on watchers are not thread-safe.</remarks>
	/// </summary>
	public interface IBaseWatcher : IDisposable
	{
		/// <summary>
		/// Start the watcher.
		/// </summary>
		void Start ();
		
		/// <summary>
		/// Stop the watcher.
		/// </summary>
		void Stop ();
		
		/// <summary>
		/// Gets a value indicating whether this watcher is running.
		/// </summary>
		/// <value>
		/// <c>true</c> if this instance is running and may receive callbacks; otherwise, <c>false</c>.
		/// </value>
		bool IsRunning {
			get;
		}
	}
}

