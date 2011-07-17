using System;

namespace Manos.IO.Managed
{
	abstract class ManagedByteStream : ManagedStream<ByteBuffer>, IByteStream
	{
		internal ManagedByteStream (Context ctx)
			: base (ctx)
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

