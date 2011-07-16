using System;
using Mono.Unix.Native;
using System.IO;

namespace Manos.IO
{
	/// <summary>
	/// Represent an IO context.
	/// <para>An IO context contains an event loop that is woken whenever
	/// something of interest happens in structes spawned by the context, i.e.
	/// a socket has new data to be read or another thread has signalled this
	/// context.</para>
	/// <para>Every context is able to spawn watchers, sockets, and streams that
	/// are said to be bound to the context. Events on the spawned structures are
	/// processed by the context event loop. Spawned structures cannot move between
	/// contexts.</para>
	/// </summary>
	public abstract class Context : IDisposable
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="Manos.IO.Context"/> class.
		/// </summary>
		protected Context ()
		{
		}
		
		/// <summary>
		/// Releases unmanaged resources and performs other cleanup operations before the <see cref="Manos.IO.Context"/> is
		/// reclaimed by garbage collection.
		/// </summary>
		~Context ()
		{
			Dispose (false);
		}

		private static readonly bool useManagedImpl;

		static Context ()
		{
#if ALWAYS_USE_MANAGED_IO
			useManagedImpl = true;
#else
			useManagedImpl = Environment.OSVersion.Platform == PlatformID.Win32NT
				|| Environment.OSVersion.Platform == PlatformID.Win32S
				|| Environment.OSVersion.Platform == PlatformID.Win32Windows
				|| Environment.OSVersion.Platform == PlatformID.WinCE;
#endif
		}
		
		/// <summary>
		/// Create a new IO context. The actual type of the context may vary
		/// depending on the operating system used to execute the program.
		/// </summary>
		public static Context Create ()
		{
			if (useManagedImpl) {
				return new Manos.IO.Managed.Context ();
			} else {
				return new Manos.IO.Libev.Context ();
			}
		}
		
		/// <summary>
		/// Releases all resource used by the <see cref="Manos.IO.Context"/> object.
		/// </summary>
		/// <remarks>
		/// Call <see cref="Dispose()"/> when you are finished using the <see cref="Manos.IO.Context"/>. The
		/// <see cref="Dispose()"/> method leaves the <see cref="Manos.IO.Context"/> in an unusable state. After calling
		/// <see cref="Dispose()"/>, you must release all references to the <see cref="Manos.IO.Context"/> so the garbage
		/// collector can reclaim the memory that the <see cref="Manos.IO.Context"/> was occupying.
		/// </remarks>
		public void Dispose ()
		{
			Dispose (true);
			GC.SuppressFinalize (this);
		}
		
		/// <summary>
		/// Dispose the current instance.
		/// </summary>
		/// <param name='disposing'>
		/// <c>true</c>, if the method was called by <see cref="Dispose()"/>,
		/// <c>false</c> if it was called from a finalizer.
		/// </param>
		protected abstract void Dispose (bool disposing);

		/// <summary>
		/// Start the event loop. The context will leave this method only when
		/// forced to by a call to <see cref="Stop"/>. When no events are pending,
		/// the loop will enter a resting state.
		/// </summary>
		public abstract void Start ();
		
		/// <summary>
		/// Runs the event loop iteration once. If no events are currently pending,
		/// the context will wait for events to process and block in the meantime.
		/// </summary>
		public abstract void RunOnce ();
		
		/// <summary>
		/// Runs the event loop once, with blocking. If no events are pending, this
		/// method returns immediatly. Otherwise, all pending events are processed,
		/// then the method returns.
		/// </summary>
		public abstract void RunOnceNonblocking ();
		
		/// <summary>
		/// Stop the event loop. This forces a prior call to <see cref="Start"/> to
		/// return control to the caller.
		/// </summary>
		public abstract void Stop ();
		
		/// <summary>
		/// Creates a new timer watcher. The created watcher will first fire after
		/// <paramref name="timeout"/> has elapsed, then periodically with period
		/// <paramref name="repeat"/>.
		/// </summary>
		/// <returns>
		/// The timer watcher.
		/// </returns>
		/// <param name='timeout'>
		/// Timeout after which the first callback invocation occurs.
		/// </param>
		/// <param name='repeat'>
		/// Repeat interval after which all successive callback invocations occur.
		/// </param>
		/// <param name='cb'>
		/// Callback to invoke for every timer event.
		/// </param>
		/// <exception cref="System.ArgumentNullException"><paramref name="c"/> is <c>null</c></exception>
		public abstract ITimerWatcher CreateTimerWatcher (TimeSpan timeout, TimeSpan repeat, Action cb);

		/// <summary>
		/// Creates a new timer watcher. The created watcher will first fire after
		/// <paramref name="timeout"/> has elapsed, then periodically with period
		/// <paramref name="timeout"/>.
		/// </summary>
		/// <returns>
		/// The timer watcher.
		/// </returns>
		/// <param name='timeout'>
		/// Timeout after which the first callback invocation occurs, and the period
		/// after which all successive callback invocations occur.
		/// </param>
		/// <param name='cb'>
		/// Callback to invoke for every timer event.
		/// </param>
		/// <exception cref="System.ArgumentNullException"><paramref name="c"/> is <c>null</c></exception>
		public abstract ITimerWatcher CreateTimerWatcher (TimeSpan timeout, Action cb);
		
		/// <summary>
		/// Creates a new async watcher.
		/// </summary>
		/// <returns>
		/// The async watcher.
		/// </returns>
		/// <param name='cb'>
		/// Callback to invoke at least once for a number of signals received by the watcher.
		/// </param>
		/// <exception cref="System.ArgumentNullException"><paramref name="c"/> is <c>null</c></exception>
		public abstract IAsyncWatcher CreateAsyncWatcher (Action cb);
		
		/// <summary>
		/// Creates a new prepare watcher. The watcher callback will be executed prior to
		/// every other watcher callback per loop iteration.
		/// </summary>
		/// <returns>
		/// The prepare watcher.
		/// </returns>
		/// <param name='cb'>
		/// Callback to invoke per loop iteration.
		/// </param>
		/// <exception cref="System.ArgumentNullException"><paramref name="c"/> is <c>null</c></exception>
		public abstract IPrepareWatcher CreatePrepareWatcher (Action cb);

		/// <summary>
		/// Creates a new check watcher. The watcher callback will be executed after
		/// every other watcher callback per loop iteration.
		/// </summary>
		/// <returns>
		/// The check watcher.
		/// </returns>
		/// <param name='cb'>
		/// Callback to invoke per loop iteration.
		/// </param>
		/// <exception cref="System.ArgumentNullException"><paramref name="c"/> is <c>null</c></exception>
		public abstract ICheckWatcher CreateCheckWatcher (Action cb);

		/// <summary>
		/// Creates a new idle watcher. The watcher callback will be executed once per loop iteration.
		/// An active idle watcher will prevent the loop from entering a resting state.
		/// </summary>
		/// <returns>
		/// The idle watcher.
		/// </returns>
		/// <param name='cb'>
		/// Callback to invoke per loop iteration.
		/// </param>
		/// <exception cref="System.ArgumentNullException"><paramref name="c"/> is <c>null</c></exception>
		public abstract IIdleWatcher CreateIdleWatcher (Action cb);
		
		/// <summary>
		/// Creates a new socket. The socket is initially invalid.
		/// </summary>
		/// <returns>
		/// The socket.
		/// </returns>
		public abstract Socket CreateSocket ();
		
		/// <summary>
		/// Creates a new secure socket. The socket is initially invalid.
		/// Currently, only listening operations are supported by secure sockets.
		/// Some operating systems do not support secure sockets at all yet.
		/// </summary>
		/// <returns>
		/// The secure socket.
		/// </returns>
		/// <param name='certFile'>
		/// Cert file in PEM format, must contain a valid X.509 server certificate.
		/// </param>
		/// <param name='keyFile'>
		/// Key file in PEM format, must contain a private key for the certificate.
		/// </param>
		public abstract Socket CreateSecureSocket (string certFile, string keyFile);
		
		/// <summary>
		/// Opens a file for asynchronous operations.
		/// </summary>
		/// <returns>
		/// An asynchronous stream for the given file.
		/// </returns>
		/// <param name='fileName'>
		/// File name.
		/// </param>
		/// <param name='openMode'>
		/// Open mode.
		/// </param>
		/// <param name='blockSize'>
		/// Block size to use for operations on the file. Larger block sizes generally improve
		/// performance, though too large block sizes only use more memory.
		/// </param>
		public abstract ByteStream OpenFile (string fileName, FileAccess openMode, int blockSize);

		/// <summary>
		/// Opens a file for asynchronous operations, creating it if it does not exist yet.
		/// </summary>
		/// <returns>
		/// An asynchronous stream for the given file.
		/// </returns>
		/// <param name='fileName'>
		/// File name.
		/// </param>
		/// <param name='blockSize'>
		/// Block size to use for operations on the file. Larger block sizes generally improve
		/// performance, though too large block sizes only use more memory.
		/// </param>
		public abstract ByteStream CreateFile (string fileName, int blockSize);
		
		public abstract UdpSocket CreateUdpSocket (AddressFamily family);
	}
}

