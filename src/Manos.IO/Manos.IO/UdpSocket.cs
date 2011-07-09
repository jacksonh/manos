using System;
using System.Collections.Generic;

namespace Manos.IO
{
	/// <summary>
	/// Udp Packet representation for handling udp sockets
	/// </summary>
	public class UdpPacket
	{
		/// <summary>
		/// The IP address in string format of the sender/receiver depending on UdpSocket function
		/// </summary>
		public string Address { get; set; }
		/// <summary>
		/// The port of the sender/receiver depending on UdpSocket functino
		/// </summary>
		public int Port { get; set; }
		/// <summary>
		/// The received buffer
		/// </summary>
		public ByteBuffer Buffer { get; set; }
	}
	
	/// <summary>
	/// Base class for asynchronous udp handling.
	/// </summary>
	public abstract class UdpSocket : IDisposable
	{
		UdpPacket currentPacket;
		IEnumerator<UdpPacket> currentWriter;
		Queue<IEnumerable<UdpPacket>> writeQueue;
		
		/// <summary>
		/// Bind the socket to listen on a host and port
		/// </summary>
		/// <param name="host">
		/// The ip address on which to bind <see cref="System.String"/>
		/// </param>
		/// <param name="port">
		/// The port on which to bind <see cref="System.Int32"/>
		/// </param>
		/// <param name="readCallback">
		/// A callback which receives an instande of the UdpPacket class <see cref="Action<UdpPacket>"/>
		/// </param>
		public abstract void Listen (string host, int port, Action<UdpPacket> readCallback);
		
		public abstract void Bind (int port);
		
		public virtual void Send (IEnumerable<UdpPacket> packet)
		{
			if (packet == null)
				throw new ArgumentNullException ("data");
			
			if (writeQueue == null) {
				writeQueue = new Queue<IEnumerable<UdpPacket>> ();
			}
			
			writeQueue.Enqueue (packet);
		}
		
		public virtual void Send (UdpPacket packet)
		{
			Send (SinglePacket (packet));
		}
		
		static IEnumerable<UdpPacket> SinglePacket(UdpPacket packet)
		{
			yield return packet;
		}
		
		/// <summary>
		/// Resumes writing.
		/// </summary>
		public abstract void ResumeWriting ();
		
		/// <summary>
		/// Pauses writing.
		/// </summary>
		public abstract void PauseWriting ();
			
		/// <summary>
		/// Writes a single buffer to the stream. Must return a positive value or <c>0</c>
		/// for successful writes, and a negative value for unsuccessful writes.
		/// Unsuccessful write pause the writing process, successful writes consume the
		/// returned number of bytes from the write queue.
		/// </summary>
		/// <returns>
		/// The number of bytes written, or a negative value on unsuccessful write.
		/// </returns>
		protected abstract int WriteSinglePacket (UdpPacket packet);
		
		/// <summary>
		/// Handles one write operation. If the write queue is empty, or the buffer
		/// produced by the currently writing sequence is <c>null</c>, the writing
		/// process is paused.
		/// </summary>
		protected virtual void HandleWrite ()
		{
			if (writeQueue == null) {
				throw new InvalidOperationException ();
			}
			if (!EnsureActivePacket () || currentPacket == null) {
				PauseWriting ();
			} else {
				WriteCurrentPacket ();
			}
		}
		
		/// <summary>
		/// Writes the current buffer to the stream via <see cref="WriteSingleBuffer"/>.
		/// A non-negative value returned by <see cref="WriteSingleBuffer"/> consumes that
		/// number of bytes from the write queue, a negative value pauses the writing
		/// process.
		/// </summary>
		protected virtual void WriteCurrentPacket ()
		{
			var sent = WriteSinglePacket (currentPacket);
			if (sent >= 0) {
				currentPacket.Buffer.Skip (sent);
			} else {
				PauseWriting ();
			}
			if (currentPacket.Buffer.Length == 0) {
				currentPacket = null;
			}
		}
		
		/// <summary>
		/// Ensures that a buffer to be written to the stream exists.
		/// </summary>
		/// <returns>
		/// <c>true</c>, iff there is a buffer that can be written to the stream.
		/// </returns>
		protected virtual bool EnsureActivePacket ()
		{
			if (currentPacket == null && EnsureActiveWriter ()) {
				if (currentWriter.MoveNext ()) {
					currentPacket = currentWriter.Current;
					return true;
				} else {
					currentWriter.Dispose ();
					currentWriter = null;
					return EnsureActivePacket ();
				}
			}
			return currentPacket != null;
		}
		
		/// <summary>
		/// Ensures that a sequence to be written to the stream exists.
		/// </summary>
		/// <returns>
		/// <c>true</c>, iff there is a sequence that can be written to the stream.
		/// </returns>
		protected virtual bool EnsureActiveWriter ()
		{
			if (currentWriter == null && writeQueue.Count > 0) {
				currentWriter = writeQueue.Dequeue ().GetEnumerator ();
			}
			return currentWriter != null;
		}

		/// <summary>
		/// Closes the socket and frees the resources taken by it.
		/// </summary>
		public abstract void Close ();
		
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

