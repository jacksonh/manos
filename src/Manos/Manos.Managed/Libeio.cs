using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Text;
using Mono.Unix.Native;
using Manos.Threading;

namespace Manos.Managed
{
    public static class Libeio
    {
        public static void close(FileStream fd, Action<int> callback)
        {
            ThreadPool.QueueUserWorkItem(a =>
            {
                try
                {
                    fd.Close();
                    Boundary.Instance.ExecuteOnTargetLoop( () => callback( 0 ));
                }
                catch
                {
                    Boundary.Instance.ExecuteOnTargetLoop( () => callback( -1 ));
                }
            });
        }
        public static void read(FileStream fd, byte[] buffer, long offset, long length, Action<int, byte[], Exception> callback) // len, buf, err
        {
            try
            {
                fd.BeginRead(buffer, (int)offset, (int)length, ar =>
                {
                    try
                    {
                        int len = fd.EndRead(ar);
	                    Boundary.Instance.ExecuteOnTargetLoop( () => callback( len, buffer, null ));
                    } catch (Exception e){
	                    Boundary.Instance.ExecuteOnTargetLoop( () => callback( 0, buffer, null ));
                    }
                }, null);
            }
            catch (Exception e)
            {
                callback(0, buffer, e);
            }
        }
        public static void open(string path, OpenFlags flags, Mono.Unix.Native.FilePermissions mode, Action<FileStream, Exception> callback) // fd, error
        {
            ThreadPool.QueueUserWorkItem(a =>
            {
                try
                {
                    FileMode fm;
                    FileAccess fa;
                    FileShare fs = FileShare.ReadWrite;

                    if (0 != (flags & OpenFlags.O_CREAT))
                        fm = FileMode.Create;
                    else
                        fm = FileMode.Open;

                    if (0 != (flags & OpenFlags.O_RDWR))
                        fa = FileAccess.ReadWrite;
                    else if (0 != (flags & OpenFlags.O_WRONLY))
                        fa = FileAccess.Write;
                    else 
                        fa = FileAccess.Read;
                    

                    var stream = new FileStream(path, fm, fa, fs);
					Boundary.Instance.ExecuteOnTargetLoop (() => callback (stream, null));
                }
                catch(Exception e)
                {
					Boundary.Instance.ExecuteOnTargetLoop (() => callback (null, e));
                }
            });
        }
        public static void stat(string path, Action<FileInfo, Exception> callback)
        {
        }

        public static void fstat(FileStream fd, Action<FileInfo, Exception> callback)
        {
            ThreadPool.QueueUserWorkItem(a =>
            {
                try
                {
                    FileInfo info = new FileInfo(fd.Name);
                    info.Refresh();
					Boundary.Instance.ExecuteOnTargetLoop (() => callback (info, null));
                }
                catch (Exception e)
                {
					Boundary.Instance.ExecuteOnTargetLoop (() => callback (null, e));
                }
            });
        }
           
    }
}
