using System;

namespace Manos.IO
{
	/// <summary>
	/// Marks a stream as sendfile-capable, i.e. the streams supports
	/// efficient methods to write the contents of a given file to
	/// the stream.
	/// </summary>
	public interface ISendfileCapable
	{
		/// <summary>
		/// Sends a file to the stream efficiently.
		/// </summary>
		/// <param name='file'>
		/// Name of the file to send.
		/// </param>
		void SendFile (string file);
	}
}

