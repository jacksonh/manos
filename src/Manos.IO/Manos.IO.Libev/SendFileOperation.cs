using System;
using Mono.Unix.Native;
using System.Collections.Generic;

namespace Manos.IO.Libev
{
	class SendFileOperation : IDisposable, IEnumerable<ByteBuffer>
	{
		int sourceFd;
		EventedStream target;
		string file;
		long position, length;
		bool completed;
		Context context;

		public SendFileOperation (Context context, EventedStream target, string file)
		{
			this.context = context;
			this.target = target;
			this.file = file;
		}

		~SendFileOperation ()
		{
			if (sourceFd > 0) {
				CloseFile ();
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
			this.sourceFd = Syscall.open (file, OpenFlags.O_RDONLY, FilePermissions.ACCESSPERMS);
			if (sourceFd == -1) {
				completed = true;
				Console.Error.WriteLine ("Error sending file '{0}' error: '{1}'", file, Syscall.GetLastError ());
			} else {
				Stat stat;
				var r = Syscall.fstat (sourceFd, out stat);
				if (r == -1) {
					completed = true;
				} else {
					length = stat.st_size;
					target.ResumeWriting ();
				}
			}
		}

		void CloseFile ()
		{
			Syscall.close (sourceFd);
			sourceFd = 0;
		}

		void SendNextBlock ()
		{
			context.Eio.SendFile (target.Handle.ToInt32 (), sourceFd, position, length - position, (len, err) => {
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

		static ByteBuffer emptyBuffer = new ByteBuffer (new byte[0], 0, 0);

		IEnumerable<ByteBuffer> Run ()
		{
			while (!completed) {
				target.PauseWriting ();
				if (sourceFd == 0) {
					OpenFile ();
				}
				SendNextBlock ();
				yield return emptyBuffer;
			}
			Dispose ();
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

