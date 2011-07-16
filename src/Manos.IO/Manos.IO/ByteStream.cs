using System;
using System.Collections.Generic;
using System.Linq;

namespace Manos.IO
{
	/// <summary>
	/// Base class for asynchronous byte streams. This stream uses ByteBuffers as
	/// fragments, and bytes as fragment units.
	/// </summary>
	public abstract class ByteStream : FragmentStream<ByteBuffer>
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="Manos.IO.ByteStream"/> class.
		/// </summary>
		/// <param name='context'>
		/// The context this instance will be bound to.
		/// </param>
		protected ByteStream (Context context)
			: base (context)
		{
		}

		/// <summary>
		/// Places a single byte array into the write queue.
		/// </summary>
		public virtual void Write (byte[] data)
		{
			Write (new ByteBuffer (data, 0, data.Length));
		}
	}
}

