using System;
using Mono.Unix.Native;
using System.Net.Sockets;
using System.IO;

namespace Manos.IO.Libev
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
			return new Manos.IO.SocketException (text, err ?? SocketError.Fault);
		}
	}
}

