using System;
using Mono.Unix.Native;

namespace Manos.IO
{
	static class Errors
	{
		public static string ErrorToString (int errno)
		{
			return Syscall.strerror (NativeConvert.ToErrno (errno));
		}
		
		public static SocketError? ErrorToSocketError (int errno)
		{
			var err = NativeConvert.ToErrno (errno);
			switch (err) {
				case Errno.EAFNOSUPPORT:
					return SocketError.AddressFamilyNotSupported;
					
				case Errno.EMFILE:
				case Errno.ENFILE:
					return SocketError.TooManyOpenSockets;
					
				case Errno.EPROTONOSUPPORT:
					return SocketError.ProtocolNotSupported;
					
				case Errno.EPROTOTYPE:
					return SocketError.ProtocolType;
					
				case Errno.EOPNOTSUPP:
					return SocketError.OperationNotSupported;
					
				case Errno.ENOTCONN:
					return SocketError.NotConnected;
					
				case Errno.EADDRINUSE:
					return SocketError.AddressAlreadyInUse;
					
				case Errno.EADDRNOTAVAIL:
					return SocketError.AddressNotAvailable;
					
				case Errno.EISCONN:
					return SocketError.IsConnected;
					
				case Errno.ECONNRESET:
					return SocketError.ConnectionReset;
					
				case Errno.ENETDOWN:
					return SocketError.NetworkDown;
					
				case Errno.EHOSTUNREACH:
					return SocketError.HostUnreachable;
					
				case Errno.ENETUNREACH:
					return SocketError.NetworkUnreachable;
					
				case Errno.EALREADY:
					return SocketError.AlreadyInProgress;
					
				case Errno.ECONNREFUSED:
					return SocketError.ConnectionRefused;
					
				case Errno.ETIMEDOUT:
					return SocketError.TimedOut;
				
				default:
					return null;
			}
		}
		
		public static SocketError ErrorToSocketError (System.Net.Sockets.SocketError err)
		{
			switch (err) {
				case System.Net.Sockets.SocketError.AddressFamilyNotSupported:
					return SocketError.AddressFamilyNotSupported;
					
				case System.Net.Sockets.SocketError.TooManyOpenSockets:
					return SocketError.TooManyOpenSockets;
					
				case System.Net.Sockets.SocketError.ProtocolNotSupported:
					return SocketError.ProtocolNotSupported;
					
				case System.Net.Sockets.SocketError.ProtocolType:
					return SocketError.ProtocolType;
					
				case System.Net.Sockets.SocketError.OperationNotSupported:
					return SocketError.OperationNotSupported;
					
				case System.Net.Sockets.SocketError.NotConnected:
					return SocketError.NotConnected;
					
				case System.Net.Sockets.SocketError.AddressAlreadyInUse:
					return SocketError.AddressAlreadyInUse;
					
				case System.Net.Sockets.SocketError.AddressNotAvailable:
					return SocketError.AddressNotAvailable;
					
				case System.Net.Sockets.SocketError.IsConnected:
					return SocketError.IsConnected;
					
				case System.Net.Sockets.SocketError.ConnectionReset:
					return SocketError.ConnectionReset;
					
				case System.Net.Sockets.SocketError.NetworkDown:
					return SocketError.NetworkDown;
					
				case System.Net.Sockets.SocketError.HostUnreachable:
					return SocketError.HostUnreachable;
					
				case System.Net.Sockets.SocketError.NetworkUnreachable:
					return SocketError.NetworkUnreachable;
					
				case System.Net.Sockets.SocketError.AlreadyInProgress:
					return SocketError.AlreadyInProgress;
					
				case System.Net.Sockets.SocketError.ConnectionRefused:
					return SocketError.ConnectionRefused;
					
				case System.Net.Sockets.SocketError.TimedOut:
					return SocketError.TimedOut;
				
				default:
					return SocketError.Failure;
			}
		}
		
		public static IOException SocketStreamFailure (string text, int errno)
		{
			var err = ErrorToSocketError (errno);
			if (err != null) {
				return new IOException (text, new Manos.IO.SocketException (err.Value));
			} else {
				return new IOException (text, new InvalidOperationException (ErrorToString (errno)));
			}
		}
		
		public static Manos.IO.SocketException SocketFailure (string text, int errno)
		{
			var err = ErrorToSocketError (errno);
			return new Manos.IO.SocketException (text, err ?? SocketError.Failure);
		}
	}
}

