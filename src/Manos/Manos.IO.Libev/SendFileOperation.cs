using System;
using Mono.Unix.Native;
using System.Collections.Generic;
using Manos.Collections;

namespace Manos.IO.Libev
{
	class SendFileOperation : IDisposable, IEnumerable<ByteBuffer>
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
			Libeio.open (file, OpenFlags.O_RDONLY, FilePermissions.ALLPERMS, (fd, err) => {
				this.sourceFd = fd;
				if (fd == -1) {
					completed = true;
					Console.Error.WriteLine ("Error sending file '{0}' errno: '{1}'", file, err);
				} else {
					Libeio.fstat (fd, (r, stat, error) => {
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
			Libeio.close (sourceFd, err => { });
			sourceFd = 0;
		}

		void SendNextBlock ()
		{
			Libeio.sendfile (target.Handle.ToInt32 (), sourceFd, position, length - position, (len, err) => {
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

		ByteBuffer emptyBuffer = new ByteBuffer (new byte[0], 0, 0);

		IEnumerable<ByteBuffer> Run ()
		{
			while (!completed) {
				target.PauseWriting ();
				if (sourceFd == 0) {
					OpenFile ();
				} else {
					SendNextBlock ();
				}
				yield return emptyBuffer;
			}
			CloseFile ();
		}

		public IEnumerator<ByteBuffer> GetEnumerator ()
		{
			return Run ().GetEnumerator ();
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator ()
		{
			return GetEnumerator ();
		}
	}
}

