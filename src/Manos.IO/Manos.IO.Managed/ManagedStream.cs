using System;

namespace Manos.IO.Managed
{
	abstract class ManagedStream : FragmentStream<ByteBuffer>, IByteStream
	{
		internal ManagedStream (Context ctx)
			: base (ctx)
		{
		}
		
		public new Context Context {
			get { return (Context) base.Context; }
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

