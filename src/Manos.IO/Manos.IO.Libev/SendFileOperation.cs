using System;
using Mono.Unix.Native;
using System.Collections.Generic;

namespace Manos.IO.Libev
{
	class SendFileOperation : IDisposable, IEnumerable<ByteBuffer>
	{
		int sourceFd;
		EventedByteStream target;
		string file;
		long position, length;
		bool completed;
		Context context;

		public SendFileOperation (Context context, EventedByteStream target, string file)
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

		IEnumerable<ByteBuffer> Run ()
		{
			while (!completed) {
				if (sourceFd == 0) {
					OpenFile ();
				}
				SendNextBlock ();
				yield return null;
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

