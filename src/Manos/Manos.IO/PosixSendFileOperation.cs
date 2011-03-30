using System;

namespace Manos.IO
{
#if !DISABLE_POSIX
	public class PosixSendFileOperation : SendFileOperation
	{
		public PosixSendFileOperation (string filename, WriteCallback callback)
			: base (filename, callback)
		{
		}

		protected override void SendNextBlock ()
		{
			stream.DisableWriting ();
			Libeio.Libeio.sendfile (stream.Handle.ToInt32 (), fd, position, Length - position, (len, err) => {
				if (len >= 0) {
					position += len;
				} else {
					OnComplete (len, err);
				}
				if (position == Length) {
					OnComplete (len, err);
				}
				stream.EnableWriting ();
			});
		}
	}
#endif
}

