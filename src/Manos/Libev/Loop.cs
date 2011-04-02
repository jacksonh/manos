

using System;
using System.Runtime.InteropServices;
using System.Net.Sockets;


namespace Libev {

	public class LibEvLoop: Manos.Loop {

		private IntPtr _native;
        private static readonly bool _isV4;

        static LibEvLoop()
        {
            _isV4 = ev_version_major() >= 4;
        }

        public static bool IsV4 { get { return _isV4; } }

		internal LibEvLoop (IntPtr native)
		{
			if (native == IntPtr.Zero)
				throw new InvalidOperationException ("Unable to create native libev loop object.");
			_native = native;
		}
		
		~LibEvLoop ()
		{
			Dispose ();	
		}
		
		public IntPtr Handle {
			get { 
				
				ThrowIfDisposed ();
				
				return _native; 
			}	
		}
		
		public override void Dispose () 
		{
			if (_native == IntPtr.Zero)
				return;
			
			// This crashes, I assume you can't destroy the default loop, so not sure whats 
			// needed in terms of cleanup here.
			
			// ev_loop_destroy (_native);
			_native = IntPtr.Zero;
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
		
		public static LibEvLoop CreateDefaultLoop ()
		{
			return CreateDefaultLoop (0);	
		}
		
		public static LibEvLoop CreateDefaultLoop (uint flags)
		{
			IntPtr native;
			int backends = ev_supported_backends ();
			if (backends == 0)
				throw new Exception ("No supported backend in libev");

			if (IsV4)
				native = ev_default_loop (flags);
			else
				native = ev_default_loop_init (flags);
			
			if (native == IntPtr.Zero)
				throw new Exception ("Unable to create default loop");
			
			return new LibEvLoop (native);
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
		private static extern IntPtr ev_default_loop (uint flags);

        [DllImport("libev", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr ev_default_loop_init (uint flags);

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

