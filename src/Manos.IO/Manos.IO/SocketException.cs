using System;
using System.Net.Sockets;

namespace Manos.IO
{
	[Serializable]
	public class SocketException : Exception
	{
		public SocketError ErrorCode {
			get;
			private set;
		}
		
		/// <summary>
		/// Initializes a new instance of the <see cref="T:SocketException"/> class
		/// </summary>
		public SocketException (SocketError code)
		{
			this.ErrorCode = code;
		}
		
		/// <summary>
		/// Initializes a new instance of the <see cref="T:SocketException"/> class
		/// </summary>
		/// <param name="message">A <see cref="T:System.String"/> that describes the exception. </param>
		public SocketException (string message, SocketError code)
			: base (message)
		{
			this.ErrorCode = code;
		}
		
		/// <summary>
		/// Initializes a new instance of the <see cref="T:SocketException"/> class
		/// </summary>
		/// <param name="message">A <see cref="T:System.String"/> that describes the exception. </param>
		/// <param name="inner">The exception that is the cause of the current exception. </param>
		public SocketException (string message, SocketError code, Exception inner)
			: base (message, inner)
		{
			this.ErrorCode = code;
		}
		
		/// <summary>
		/// Initializes a new instance of the <see cref="T:SocketException"/> class
		/// </summary>
		/// <param name="context">The contextual information about the source or destination.</param>
		/// <param name="info">The object that holds the serialized object data.</param>
		protected SocketException (System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base (info, context)
		{
		}
	}
}

