using System;

namespace Manos.IO
{
	public interface IBaseWatcher : IDisposable
	{
		void Start ();

		void Stop ();

		object Tag {
			get;
			set;
		}

		bool IsRunning {
			get;
		}
	}
}

