using System;
using System.Collections.Generic;
using System.Linq;

namespace Manos.IO
{
	/// <summary>
	/// Represents an asynchronous byte stream. This stream uses ByteBuffers as
	/// fragments, and bytes as fragment units.
	/// </summary>
	public interface IByteStream : IStream<ByteBuffer>
	{
		/// <summary>
		/// Places a single byte array into the write queue.
		/// </summary>
		void Write (byte[] data);
	}
}

