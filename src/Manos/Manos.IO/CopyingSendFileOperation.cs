using System;
using System.Collections.Generic;
using Manos.Collections;

namespace Manos.IO
{
	public class CopyingSendFileOperation : SendFileOperation
	{
		byte [] transferBuffer = new byte[32768];

		public CopyingSendFileOperation (string filename, WriteCallback callback)
			: base (filename, callback)
		{
		}

		protected override void SendNextBlock ()
		{
			stream.DisableWriting ();
			Libeio.Libeio.read (fd, transferBuffer, position, transferBuffer.Length, (len, buf, err) => {
				if (position == Length) {
					OnComplete (len, err);
				}
				if (len > 0) {
					position += len;
					currentPrefixBlock = new SendBytesOperation (new ByteBuffer (transferBuffer, 0, len), null);
					currentPrefixBlock.BeginWrite (stream);
				} else {
					OnComplete (len, err);
				}
				stream.EnableWriting ();
			});
		}
	}
}

