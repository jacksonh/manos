using System;
using System.Runtime.InteropServices;
using Mono.Unix;
using Mono.Unix.Native;

namespace Libev
{
	public class Pipe
	{
		int[] pipe = new int[2];

		public Pipe()
		{
			Syscall.pipe(pipe);
		}

		public IntPtr Out {
			get {
				return new IntPtr(pipe[0]);
			}
		}

		public IntPtr In {
			get {
				return new IntPtr(pipe[1]);
			}
		}

		public void Write(IntPtr buf, ulong count)
		{
			Syscall.write(pipe[1], buf, count);
		}

		public void Write(byte[] buffer, int start, int count)
		{
			if (start + count >= buffer.Length) {
				throw new ArgumentException();
			}
			var ptr = Marshal.AllocHGlobal(count);
			Marshal.Copy(buffer, start, ptr, count);
			Write(ptr, (ulong)count);
			Marshal.FreeHGlobal(ptr);
		}

		public void Read(IntPtr buf, ulong count)
		{
			Syscall.read(pipe[0], buf, count);
		}

		public void Close()
		{
			for (int i = 0; i < pipe.Length; i++) {
				Syscall.close(pipe[i]);
			}
		}
	}
}

