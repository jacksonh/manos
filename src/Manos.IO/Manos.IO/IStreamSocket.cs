using System;
using System.Net;

namespace Manos.IO
{
	/// <summary>
	/// Every stream socket is bound to an event loop, delivers data in a stream of fragments
	/// via an appropriate stream, and has it's end points identified by an appropriate
	/// implementing class of <see cref="System.Net.EndPoint"/>.
	/// <seealso cref="Manos.IO.IStream{TFragment}"/>
	/// </summary>
	public interface IStreamSocket<TFragment, TStream, TEndPoint> : ISocket<TEndPoint>
		where TFragment : class
		where TStream : IStream<TFragment>
		where TEndPoint : EndPoint
	{
		/// <summary>
		/// Gets the stream used by the socket to deliver data fragments and other events.
		/// </summary>
		/// <returns>
		/// The socket stream.
		/// </returns>
		TStream GetSocketStream ();
	}
}

