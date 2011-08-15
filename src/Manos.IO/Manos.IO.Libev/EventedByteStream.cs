using System;

namespace Manos.IO.Libev
{
	abstract class EventedByteStream : EventedStream<ByteBuffer>, IByteStream
	{
		internal EventedByteStream (Context context, IntPtr handle)
			: base (context, handle)
		{
		}
		
		public void Write (byte[] data)
		{
			Write (new ByteBuffer (data));
		}
		
		protected override long FragmentSize (ByteBuffer data)
		{
			return data.Length;
		}
	}
}

