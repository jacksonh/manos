using System;
using System.Collections.Generic;

namespace Manos.IO
{
	/// <summary>
	/// Base class for asynchronous udp handling.
	/// </summary>
	public abstract class UdpSocket : IDisposable
	{
		internal UdpSocket (Context context)
		{
			this.Context = context;
		}
		
		public AddressFamily AddressFamily {
			get;
			protected set;
		}
		
		public Context Context {
			get;
			private set;
		}
		
		protected bool ValidAddress (string host)
		{
			var family = System.Net.IPAddress.Parse (host).AddressFamily;
			switch (AddressFamily) {
				case AddressFamily.InterNetwork:
					return family == System.Net.Sockets.AddressFamily.InterNetwork;
				default:
					return family == System.Net.Sockets.AddressFamily.InterNetworkV6;
			}
		}
		
		protected void CheckAddress (string host)
		{
			if (!ValidAddress (host)) {
				throw new Exception (string.Format ("Address is not of a valid family type"));
			}
		}
		
		public abstract IStream<UdpPacket> GetSocketStream ();
		
		public abstract void Bind (string host, int port);

		/// <summary>
		/// Closes the socket and frees the resources taken by it.
		/// </summary>
		public virtual void Close ()
		{
		}
		
		/// <summary>
		/// Releases all resource used by the <see cref="Manos.IO.UdpSocket"/> object.
		/// </summary>
		/// <remarks>
		/// Call <see cref="Dispose()"/> when you are finished using the <see cref="Manos.IO.UdpSocket"/>. The
		/// <see cref="Dispose()"/> method leaves the <see cref="Manos.IO.UdpSocket"/> in an unusable state. After calling
		/// <see cref="Dispose()"/>, you must release all references to the <see cref="Manos.IO.UdpSocket"/> so the garbage
		/// collector can reclaim the memory that the <see cref="Manos.IO.UdpSocket"/> was occupying.
		/// </remarks>
		public void Dispose ()
		{
			Dispose (true);
			GC.SuppressFinalize (this);
		}

		/// <summary>
		/// Dispose the current instance.
		/// </summary>
		/// <param name='disposing'>
		/// <c>true</c>, if the method was called by <see cref="Dispose()"/>,
		/// <c>false</c> if it was called from a finalizer.
		/// </param>
		protected virtual void Dispose (bool disposing)
		{
			Close ();
		}
	}
}

