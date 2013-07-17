using System;
using System.Runtime.InteropServices;
using Libev;

namespace Manos.IO.Libev
{
	class Notifier : INotifier, IBaseWatcher, IDisposable
	{
		Pipe pipe;
		IOWatcher iowatcher;
		IntPtr data;

		public Notifier (Context context, Action callback)
		{
			data = Marshal.AllocHGlobal(1);

			pipe = new Pipe();
			iowatcher = new IOWatcher(pipe.Out, EventTypes.Read, context.Loop, (iow, ev) => {
				pipe.Read (data, 1);
				if (callback != null) {
					callback();
				}
			});
		}

		~Notifier ()
		{
			Dispose (false);
		}

		public void Notify ()
		{
			pipe.Write (data, 1);
		}

		public void Start ()
		{
			iowatcher.Start();
		}

		public void Stop ()
		{
			iowatcher.Stop ();
		}

		public bool IsRunning {
			get {
				return iowatcher.IsRunning;
			}
		}

		public void Dispose ()
		{
			Dispose (true);
			GC.SuppressFinalize (this);
		}

		protected void Dispose (bool disposing)
		{
			if (data != IntPtr.Zero) {
				Marshal.FreeHGlobal(data);
				data = IntPtr.Zero;
			}
		}
	}
}

