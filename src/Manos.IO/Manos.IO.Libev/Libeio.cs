using System;
using System.Runtime.InteropServices;
using Mono.Unix.Native;

using size_t = System.UIntPtr;
using off_t = System.Int64;
using mode_t = Mono.Unix.Native.FilePermissions;
using eio_tstamp = System.Double;
using uid_t = System.Int32;
using gid_t = System.Int32;

namespace Manos.IO.Libev
{
	static class Libeio
	{
		delegate void eio_cb (ref eio_req req);

		[StructLayout (LayoutKind.Sequential)]
		struct eio_req
		{
			public IntPtr next;
			public IntPtr result;
			public Int64 offs;  // We are forcing 64bit off_t's by using -D_FILE_OFFSET_BITS=64
			public UIntPtr size;
			public IntPtr ptr1;
			public IntPtr ptr2;
			public double nv1;
			public double nv2;
			public int type;
			public int int1;
			public IntPtr int2;
			public IntPtr int3;
			public int errorno;
			public byte flags;
			public byte pri;
			public IntPtr data;
			public IntPtr finish;
			public IntPtr destroy;
			public IntPtr feed;
			public IntPtr grp;
			public IntPtr grp_prev;
			public IntPtr grp_next;
			public IntPtr grp_first;
		};

//		[DllImport ("libeio", CallingConvention = CallingConvention.Cdecl)]
//		static extern IntPtr eio_nop (int pri, eio_cb cb, IntPtr data);
//
//		[DllImport ("libeio", CallingConvention = CallingConvention.Cdecl)]
//		static extern IntPtr eio_busy (eio_tstamp delay, int pri, eio_cb cb, IntPtr data); 
//
//		[DllImport ("libeio", CallingConvention = CallingConvention.Cdecl)]
//		static extern IntPtr eio_sync (int pri, eio_cb cb, IntPtr data);
//
//		[DllImport ("libeio", CallingConvention = CallingConvention.Cdecl)]
//		static extern IntPtr eio_fsync (int fd, int pri, eio_cb cb, IntPtr data);
//
//		[DllImport ("libeio", CallingConvention = CallingConvention.Cdecl)]
//		static extern IntPtr eio_fdatasync (int fd, int pri, eio_cb cb, IntPtr data);
//
//		[DllImport ("libeio", CallingConvention = CallingConvention.Cdecl)]
//		static extern IntPtr eio_msync (IntPtr addr, UIntPtr length, MsyncFlags flags, int pri, eio_cb cb, IntPtr data);
//
//		[DllImport ("libeio", CallingConvention = CallingConvention.Cdecl)]
//		static extern IntPtr eio_mtouch (IntPtr addr, size_t length, int flags, int pri, eio_cb cb, IntPtr data);
//
//		[DllImport ("libeio", CallingConvention = CallingConvention.Cdecl)]
//		static extern IntPtr eio_mlock (IntPtr addr, size_t length, int pri, eio_cb cb, IntPtr data);
//
//		[DllImport ("libeio", CallingConvention = CallingConvention.Cdecl)]
//		static extern IntPtr eio_mlockall (int flags, int pri, eio_cb cb, IntPtr data);
//
//		[DllImport ("libeio", CallingConvention = CallingConvention.Cdecl)]
//		static extern IntPtr eio_sync_file_range (int fd, IntPtr offset, UIntPtr nbytes, uint flags, int pri, eio_cb cb, IntPtr data);
//
//		[DllImport ("libeio", CallingConvention = CallingConvention.Cdecl)]
//		static extern IntPtr eio_close (int fd, int pri, eio_cb cb, IntPtr data);
//
//		[DllImport ("libeio", CallingConvention = CallingConvention.Cdecl)]
//		static extern IntPtr eio_readahead (int fd, off_t offset, size_t length, int pri, eio_cb cb, IntPtr data);
		
		static eio_cb readCB = ReadCallback;

		static void ReadCallback (ref eio_req req)
		{
			var handle = GCHandle.FromIntPtr (req.data);
			var tuple = (Tuple<byte [], Action<int, byte [], int>>) handle.Target;
			tuple.Item2 (req.result.ToInt32 (), tuple.Item1, req.errorno);
			handle.Free ();
		}

		public static void read (int fd, byte[] buffer, long offset, long length, Action<int, byte[], int> callback)
		{
			eio_read (fd, buffer, (UIntPtr) length, offset, 0, readCB, GCHandle.ToIntPtr (GCHandle.Alloc (Tuple.Create (buffer, callback))));
		}

		[DllImport ("libeio", CallingConvention = CallingConvention.Cdecl)]
		static extern IntPtr eio_read (int fd, byte [] buf, size_t length, off_t offset, int pri, eio_cb cb, IntPtr data);

		static eio_cb writeCB = WriteCallback;

		static void WriteCallback (ref eio_req req)
		{
			var handle = GCHandle.FromIntPtr (req.data);
			((Action<int, int>) handle.Target) (req.result.ToInt32 (), req.errorno);
			handle.Free ();
		}

		public static void write (int fd, byte[] buffer, long offset, long length, Action<int, int> callback)
		{
			eio_write (fd, buffer, (UIntPtr) length, offset, 0, writeCB, GCHandle.ToIntPtr (GCHandle.Alloc (callback)));
		}

		[DllImport ("libeio", CallingConvention = CallingConvention.Cdecl)]
		static extern IntPtr eio_write (int fd, byte[] buf, size_t length, off_t offset, int pri, eio_cb cb, IntPtr data);

//
//		[DllImport ("libeio", CallingConvention = CallingConvention.Cdecl)]
//		static extern IntPtr eio_fstat (int fd, int pri, eio_cb cb, IntPtr data);
//
//		[DllImport ("libeio", CallingConvention = CallingConvention.Cdecl)]
//		static extern IntPtr eio_fstatvfs (int fd, int pri, eio_cb cb, IntPtr data); 
//
//		[DllImport ("libeio", CallingConvention = CallingConvention.Cdecl)]
//		static extern IntPtr eio_futime (int fd, eio_tstamp atime, eio_tstamp mtime, int pri, eio_cb cb, IntPtr data);
//
//		[DllImport ("libeio", CallingConvention = CallingConvention.Cdecl)]
//		static extern IntPtr eio_ftruncate (int fd, off_t offset, int pri, eio_cb cb, IntPtr data);
//
//		[DllImport ("libeio", CallingConvention = CallingConvention.Cdecl)]
//		static extern IntPtr eio_fchmod (int fd, mode_t mode, int pri, eio_cb cb, IntPtr data);
//
//		[DllImport ("libeio", CallingConvention = CallingConvention.Cdecl)]
//		static extern IntPtr eio_fchown (int fd, uid_t uid, gid_t gid, int pri, eio_cb cb, IntPtr data);
//
//		[DllImport ("libeio", CallingConvention = CallingConvention.Cdecl)]
//		static extern IntPtr eio_dup2 (int fd, int fd2, int pri, eio_cb cb, IntPtr data);
		static eio_cb sendfileCB = SendfileCallback;

		static void SendfileCallback (ref eio_req req)
		{
			var handle = GCHandle.FromIntPtr (req.data);
			((Action<long, int>) handle.Target) (req.result.ToInt64 (), req.errorno);
			handle.Free ();
		}

		public static void sendfile (int out_fd, int in_fd, long offset, long length, Action<long, int> callback)
		{
			eio_sendfile (out_fd, in_fd, offset, (UIntPtr) length, 0, sendfileCB, GCHandle.ToIntPtr (GCHandle.Alloc (callback)));
		}

		[DllImport ("libeio", CallingConvention = CallingConvention.Cdecl)]
		static extern IntPtr eio_sendfile (int out_fd, int in_fd, off_t in_offset, size_t length, int pri, eio_cb cb, IntPtr data);
//
//		[DllImport ("libeio", CallingConvention = CallingConvention.Cdecl)]
//		static extern IntPtr eio_open (string path, OpenFlags flags, mode_t mode, int pri, eio_cb cb, IntPtr data);
//
//		[DllImport ("libeio", CallingConvention = CallingConvention.Cdecl)]
//		static extern IntPtr eio_utime (string path, eio_tstamp atime, eio_tstamp mtime, int pri, eio_cb cb, IntPtr data);
//
//		[DllImport ("libeio", CallingConvention = CallingConvention.Cdecl)]
//		static extern IntPtr eio_truncate (string path, off_t offset, int pri, eio_cb cb, IntPtr data);
//
//		[DllImport ("libeio", CallingConvention = CallingConvention.Cdecl)]
//		static extern IntPtr eio_chown (string path, uid_t uid, gid_t gid, int pri, eio_cb cb, IntPtr data);
//
//		[DllImport ("libeio", CallingConvention = CallingConvention.Cdecl)]
//		static extern IntPtr eio_chmod (string path, mode_t mode, int pri, eio_cb cb, IntPtr data);
//
//		[DllImport ("libeio", CallingConvention = CallingConvention.Cdecl)]
//		static extern IntPtr eio_mkdir (string path, mode_t mode, int pri, eio_cb cb, IntPtr data);
//
//		[DllImport ("libeio", CallingConvention = CallingConvention.Cdecl)]
//		static extern IntPtr eio_readdir (string path, int flags, int pri, eio_cb cb, IntPtr data); 
//
//		[DllImport ("libeio", CallingConvention = CallingConvention.Cdecl)]
//		static extern IntPtr eio_rmdir (string path, int pri, eio_cb cb, IntPtr data);
//
//		[DllImport ("libeio", CallingConvention = CallingConvention.Cdecl)]
//		static extern IntPtr eio_unlink (string path, int pri, eio_cb cb, IntPtr data);
//
//		[DllImport ("libeio", CallingConvention = CallingConvention.Cdecl)]
//		static extern IntPtr eio_readlink (string path, int pri, eio_cb cb, IntPtr data);
//
//		[DllImport ("libeio", CallingConvention = CallingConvention.Cdecl)]
//		static extern IntPtr eio_stat (string path, int pri, eio_cb cb, IntPtr data); 
//
//		[DllImport ("libeio", CallingConvention = CallingConvention.Cdecl)]
//		static extern IntPtr eio_lstat (string path, int pri, eio_cb cb, IntPtr data);
//
//		[DllImport ("libeio", CallingConvention = CallingConvention.Cdecl)]
//		static extern IntPtr eio_statvfs (string path, int pri, eio_cb cb, IntPtr data);
//
//		[DllImport ("libeio", CallingConvention = CallingConvention.Cdecl)]
//		static extern IntPtr eio_mknod (string path, mode_t mode, dev_t dev, int pri, eio_cb cb, IntPtr data);
//
//		[DllImport ("libeio", CallingConvention = CallingConvention.Cdecl)]
//		static extern IntPtr eio_link (string path, string new_path, int pri, eio_cb cb, IntPtr data);
//
//		[DllImport ("libeio", CallingConvention = CallingConvention.Cdecl)]
//		static extern IntPtr eio_symlink (string path, string new_path, int pri, eio_cb cb, IntPtr data);
//
//		[DllImport ("libeio", CallingConvention = CallingConvention.Cdecl)]
//		static extern IntPtr eio_rename (string path, string new_path, int pri, eio_cb cb, IntPtr data);
//
//		[DllImport ("libeio", CallingConvention = CallingConvention.Cdecl)]
//		static extern IntPtr eio_custom (eio_cb execute, int pri, eio_cb cb, IntPtr data);
	}
}

