using System;
using Mono.Unix.Native;

namespace Manos.IO.Libev
{
	class SendFileOperation : IDisposable
	{
		int sourceFd;
		EventedStream target;
		string file;
		long position, length;
		bool completed;

		public SendFileOperation (EventedStream target, string file)
		{
			this.target = target;
			this.file = file;
		}

		~SendFileOperation ()
		{
			if (sourceFd > 0) {
				Dispose ();
			}
		}

		public void Dispose ()
		{
			if (sourceFd > 0) {
				CloseFile ();
			}
			GC.SuppressFinalize (this);
		}

		void OpenFile ()
		{
			target.PauseWriting ();
			Libeio.Libeio.open (file, OpenFlags.O_RDONLY, FilePermissions.ALLPERMS, (fd, err) => {
				this.sourceFd = fd;
				if (fd == -1) {
					completed = true;
					Console.Error.WriteLine ("Error sending file '{0}' errno: '{1}'", file, err);
				} else {
					Libeio.Libeio.fstat (fd, (r, stat, error) => {
						if (r == -1) {
							completed = true;
						} else {
							length = stat.st_size;
							target.ResumeWriting ();
						}
					});
				}
			});
		}

		void CloseFile ()
		{
			Libeio.Libeio.close (sourceFd, err => { });
			sourceFd = 0;
		}

		void SendNextBlock ()
		{
			target.PauseWriting ();
			Libeio.Libeio.sendfile (target.Handle.ToInt32 (), sourceFd, position, length - position, (len, err) => {
				if (len >= 0) {
					position += len;
				} else {
					completed = true;
				}
				if (position == length) {
					completed = true;
				}
				target.ResumeWriting ();
			});
		}

		public bool Run ()
		{
			if (!completed) {
				if (sourceFd == 0) {
					OpenFile ();
				} else {
					SendNextBlock ();
				}
			} else {
				CloseFile ();
			}
			return completed;
		}
	}
}

