using System;
using Mono.Unix.Native;

namespace Manos.IO.Libev
{
	static class Errors
	{
		public static string ErrorToString (int errno)
		{
			return Syscall.strerror (NativeConvert.ToErrno (errno));
		}
	}
}

