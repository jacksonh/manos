
using System;
using System.Collections;
using System.Collections.Generic;

namespace Mango.Server {

	public class WriteBytesOperation : IWriteOperation {

		private IList<ArraySegment<byte>> bytes;
		private WriteCallback callback;
		
		public WriteBytesOperation (IList<ArraySegment<byte>> bytes, WriteCallback callback)
		{
			this.bytes = bytes;
			this.callback = callback;
		}

		public IList<ArraySegment<byte>> Bytes {
			get { return bytes; }
			set {
				if (value == null)
					throw new ArgumentNullException ("value");
				bytes = value;
			}
		}

		public WriteCallback Callback {
			get { return callback; }
			set {
				if (value == null)
					throw new ArgumentNullException ("value");
				callback = value;
			}
		}
		
		public void Write (IOStream stream)
		{
			stream.Write (bytes, callback);
		}
	}
}

