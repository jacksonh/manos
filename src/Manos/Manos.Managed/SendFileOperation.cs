using System;
using Mono.Unix.Native;
using System.Text;
using Manos.Collections;
using System.Collections.Generic;
using Manos.IO;
using System.IO;

namespace Manos.Managed
{
    public class SendFileOperation : ISendFileOperation
    {
        string fileName;
        WriteCallback callback;
        protected FileStream fd;
        protected ISocketStream stream;
        protected long position = 0;
        protected IWriteOperation currentPrefixBlock;

        public SendFileOperation(string filename, WriteCallback callback)
        {
            this.fileName = filename;
            this.callback = callback;

            Length = -1;
            fd = null;
        }

        byte [] transferBuffer = new byte[32768];

		protected void SendNextBlock ()
		{
			stream.DisableWriting ();
            Libeio.read(fd, transferBuffer, position, transferBuffer.Length, (len, buf, err) =>
            {
				if (position == Length) {
					OnComplete (len, err);
				}
				if (len > 0) {
					position += len;
					currentPrefixBlock = new SendBytesOperation (new[] {
						new ByteBuffer (transferBuffer, 0, len)
					}, null);
					currentPrefixBlock.BeginWrite (stream);
				} else {
					OnComplete (len, err);
				}
				stream.EnableWriting ();
			});
		}

        public bool Chunked
        {
            get;
            set;
        }

        public long Length
        {
            get;
            set;
        }

        
        public void Dispose()
        {
            if (fd != null)
            {
                Libeio.close(fd, err => { });
            }
        }

        public void BeginWrite(IIOStream stream)
        {
        }

        public bool Combine(IWriteOperation other)
        {
            return false;
        }

        public void EndWrite(IIOStream stream)
        {
            Libeio.close(fd, err =>
            {
                fd = null;
            });
            if (callback != null)
            {
                callback();
            }
        }

        void OpenFile()
        {
            stream.DisableWriting();
            Libeio.open(fileName, OpenFlags.O_RDONLY, FilePermissions.ALLPERMS, (fd, err) =>
            {
                if (fd == null)
                {
                    OnComplete(-1, err);
                }
                else
                {
                    this.fd = fd;
                    stream.EnableWriting();
                    HandleWrite(stream);
                }
            });
        }

        void InitializeTransfer()
        {
            stream.DisableWriting();
            Libeio.fstat(fd, (stat, err) =>
            {
                if (err != null)
                {
                    OnComplete(-1, err);
                }
                else
                {
                    Length = stat.Length;
                    var chunkHeader = string.Format("{0:x}\r\n", Length);
                    var headerBytes = Encoding.ASCII.GetBytes(chunkHeader);
                    currentPrefixBlock = new SendBytesOperation(new[] {
						new ByteBuffer (headerBytes, 0, headerBytes.Length)
					}, null);
                    stream.EnableWriting();
                    currentPrefixBlock.BeginWrite(stream);
                }
            });
        }

        public void HandleWrite(IIOStream stream)
        {
            this.stream = (ISocketStream)stream;
            if (currentPrefixBlock != null && !currentPrefixBlock.IsComplete)
            {
                currentPrefixBlock.HandleWrite(stream);
                if (currentPrefixBlock.IsComplete)
                {
                    currentPrefixBlock.EndWrite(stream);
                    currentPrefixBlock.Dispose();
                    currentPrefixBlock = null;
                }
            }
            if (fd == null)
            {
                OpenFile();
            }
            else if (Length == -1)
            {
                if (!Chunked)
                {
                    stream.DisableWriting();
                }
                else
                {
                    InitializeTransfer();
                }
            }
            else if (position != Length)
            {
                SendNextBlock();
            }
            else
            {
                OnComplete(0, null);
            }
        }

        public bool IsComplete
        {
            get;
            private set;
        }

        public void SetLength(long l)
        {
            Length = l;
            if (stream != null)
                stream.EnableWriting();
        }

        protected void OnComplete(long length, Exception error)
        {
            if (length == -1)
            {
                // hmmm, how best to propogate this?
                Console.Error.WriteLine("Error sending file '{0}' errno: '{1}'", fileName, error);
                // Let it at least complete.
            }

            IsComplete = true;

            if (Completed != null)
                Completed(this, EventArgs.Empty);
        }

        public event EventHandler Completed;
    }
}

