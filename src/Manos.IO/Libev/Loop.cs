using System;
using System.Runtime.InteropServices;
using System.Net.Sockets;

namespace Libev
{
	class Loop
	{
		private IntPtr _native;
		private GCHandle _handle;
		private static readonly bool _isV4;

		static Loop ()
		{
			_isV4 = ev_version_major () >= 4;
		}

		public static bool IsV4 { get { return _isV4; } }

		public Loop ()
		{
			int backends = ev_supported_backends ();
			if (backends == 0)
				throw new InvalidOperationException ("No supported backend in libev");
			
			_native = ev_loop_new (0);
			
			if (_native == IntPtr.Zero)
				throw new Exception ("Unable to create native loop");
			
			_handle = GCHandle.Alloc (this);
		}

		~Loop ()
		{
			Dispose ();
		}

		public IntPtr Handle {
			get {
				ThrowIfDisposed ();
				return _native; 
			}
		}

		public void Dispose ()
		{
			if (_native == IntPtr.Zero)
				return;
			
			ev_loop_destroy (_native);
			_native = IntPtr.Zero;
			_handle.Free ();
		}

		public void RunBlocking ()
		{
			ThrowIfDisposed ();
			
			Run (LoopType.Blocking);
		}

		public void RunNonBlocking ()
		{
			ThrowIfDisposed ();
			
			Run (LoopType.NonBlocking);
		}

		public void RunOneShot ()
		{
			ThrowIfDisposed ();
			
			Run (LoopType.Oneshot);
		}

		public void Run (LoopType type)
		{
			ThrowIfDisposed ();

			if (IsV4)
				ev_run (_native, type);
			else
				ev_loop (_native, type);
		}

		public void Unloop (UnloopType type)
		{
			ThrowIfDisposed ();

			if (IsV4)
				ev_break (_native, type);
			else 
				ev_unloop (_native, type);
		}

		private void ThrowIfDisposed ()
		{
			if (_native == IntPtr.Zero)
				throw new ObjectDisposedException ("native object has been disposed.");
		}

		[DllImport ("libev", CallingConvention = CallingConvention.Cdecl)]
		private static extern int ev_version_major ();

		[DllImport ("libev", CallingConvention = CallingConvention.Cdecl)]
		private static extern int ev_version_minor ();

		[DllImport ("libev", CallingConvention = CallingConvention.Cdecl)]
		private static extern IntPtr ev_loop_new (uint flags);

		[DllImport ("libev", CallingConvention = CallingConvention.Cdecl)]
		private static extern void ev_loop (IntPtr loop, LoopType type);

		[DllImport ("libev", CallingConvention = CallingConvention.Cdecl)]
		private static extern void ev_loop_destroy (IntPtr loop);

		[DllImport ("libev", CallingConvention = CallingConvention.Cdecl)]
		private static extern void ev_unloop (IntPtr loop, UnloopType flags);

		[DllImport("libev", CallingConvention = CallingConvention.Cdecl)]
		private static extern void ev_run (IntPtr loop, LoopType type);

		[DllImport("libev", CallingConvention = CallingConvention.Cdecl)]
		private static extern void ev_break (IntPtr loop, UnloopType flags);

		[DllImport("libev", CallingConvention = CallingConvention.Cdecl)]
		private static extern int ev_supported_backends ();
	}
}

