

using System;
using System.Runtime.InteropServices;


namespace Libev {

	public class Loop : IDisposable {

		private IntPtr _native;
		
		internal Loop (IntPtr native)
		{
			if (native == IntPtr.Zero)
				throw new InvalidOperationException ("Unable to create native libev loop object.");
			_native = native;
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
			
			ev_loop (_native, type);
		}
		
		public void Unloop (UnloopType type)
		{
			ThrowIfDisposed ();
			
			ev_unloop (_native, type);	
		}
		
		public static Loop CreateDefaultLoop ()
		{
			return CreateDefaultLoop (0);	
		}
		
		public static Loop CreateDefaultLoop (uint flags)
		{
			IntPtr native = ev_default_loop_init (flags);
			
			if (native == IntPtr.Zero)
				throw new Exception ("Unable to create default loop");
			
			return new Loop (native);
		}
		
		private void ThrowIfDisposed ()
		{
			if (_native == IntPtr.Zero)
				throw new ObjectDisposedException ("native object has been disposed.");
		}
		
		[DllImport ("libev")]
		private static extern IntPtr ev_default_loop_init (uint flags);
		
		[DllImport ("libev")]
		private static extern void ev_loop (IntPtr loop, LoopType type);
		
		[DllImport ("libev")]
		private static extern void ev_loop_destroy (IntPtr loop);
		
		[DllImport ("libev")]
		private static extern void ev_unloop (IntPtr loop, UnloopType flags);

	}
}

