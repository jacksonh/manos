using System;

namespace Manos.IO.Managed
{
	abstract class ManagedByteStream : ManagedStream<ByteBuffer>, IByteStream
	{
		protected ManagedByteStream (Context ctx, int bufferSize)
			: base (ctx, bufferSize)
		{
		}
		
		public void Write(byte[] data)
		{
			Write (new ByteBuffer (data));
		}
		
		protected override long FragmentSize(ByteBuffer fragment)
		{
			return fragment.Length;
		}
	}
}

