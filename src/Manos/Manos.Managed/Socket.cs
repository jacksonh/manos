using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using Manos.IO;
using Manos.Collections;

namespace Manos.Managed
{
    class SocketStream: Manos.IO.ISocketStream
    {
        private Socket socket;
        private string address;
        private int port;
        private System.Timers.Timer timer;
        private IOLoop loop;

        public ISendFileOperation MakeSendFile(string fn)
        {
            return new SendFileOperation (fn, null);
        }

        public IntPtr Handle
        {
            get
            {
                return socket.Handle;
            }
        }

        public SocketStream(IOLoop loop)
        {
            this.loop = loop;
        }

        public SocketStream(IOLoop loop, Socket sock):this(loop)
        {
            socket = sock;
            StartTimeout();
            address = ((IPEndPoint)socket.RemoteEndPoint).Address.ToString();
            port = ((IPEndPoint)socket.RemoteEndPoint).Port;
                        
        }


        public Manos.IO.IOLoop IOLoop { get { return loop; } }


        public void Connect(string host, int port)
        {
            address = host;
            this.port = port;
            IPAddress addr;
            if (!IPAddress.TryParse(host, out addr))
            {
                Dns.BeginGetHostEntry(host, (a) =>
                {
                    try
                    {
                        IPHostEntry ep = Dns.EndGetHostEntry(a);
                        StartConnectingSocket(ep.AddressList[0], port);
                    }
                    catch
                    {
                        OnError();
                    }
                }, null);
            }
            else
            {
                StartConnectingSocket(addr, port);
            }
        }

        public void Connect(int port)
        {
            Connect("127.0.0.1", port);
        }

        public void Listen(string host, int port)
        {
            address = host;
            this.port = port; 
            IPAddress addr;
            if (!IPAddress.TryParse(host, out addr))
            {
                Dns.BeginGetHostEntry(host, (a) =>
                {
                    try
                    {
                        IPHostEntry ep= Dns.EndGetHostEntry(a);
                        StartListeningSocket(ep.AddressList[0], port);
                    }
                    catch
                    {
                        OnError();
                    }
                }, null);
            } else {
                StartListeningSocket(addr, port);
            }
        }

        private void StartListeningSocket(IPAddress addr, int port)
        {
            socket = new Socket(addr.AddressFamily, 
                SocketType.Stream, 
                ProtocolType.Tcp);
            try
            {
                socket.Bind(new IPEndPoint(addr, port));
                socket.Listen(5);
                socket.BeginAccept(AcceptCallback, null);
            }
            catch
            {
                OnError();
            }
        }

        private void StartConnectingSocket(IPAddress addr, int port)
        {
            StartTimeout();
            socket = new Socket(addr.AddressFamily, 
                SocketType.Stream, 
                ProtocolType.Tcp);
            try
            {
                socket.BeginConnect(addr, port, (ar) =>
                {
                    try
                    {
                        socket.EndConnect(ar);
                        if (Connected != null)
                            loop.NonBlockInvoke(delegate
                            {
                                Connected(this);
                            });
                    }
                    catch
                    {
                        OnError();
                    }
                }, null);
            }
            catch
            {
                OnError();
            }
        }

        private void StartTimeout()
        {
            if (timer == null)
            {
                timer = new System.Timers.Timer(60 * 1000);
                timer.Elapsed += (s, e) =>
                {
                    if (TimedOut != null)
                    {
                        loop.BlockInvoke(delegate
                        {
                            TimedOut(this, EventArgs.Empty);
                        });
                    }
                    Close();
                };
            }
            if (timer.Enabled)
            {
                timer.Interval = timer.Interval;
            } else
                timer.Enabled = true;
            
        }

        private void AcceptCallback(IAsyncResult ar)
        {
            try
            {
                Socket sock = socket.EndAccept(ar);

                if (this.ConnectionAccepted != null)
                {
                    loop.NonBlockInvoke(delegate
                    {
                        ConnectionAccepted(this, new IO.ConnectionAcceptedEventArgs(new SocketStream(loop, sock)));
                    });
                }
                else
                {
                    sock.Dispose();
                }
                socket.BeginAccept(AcceptCallback, null);
            }
            catch
            {
                OnError();
            }
        }
        public string Address
        {
            get { return address; }
        }

        public int Port
        {
            get { return port; }
        }

        public event Action<IO.ISocketStream> Connected;

        public event EventHandler<IO.ConnectionAcceptedEventArgs> ConnectionAccepted;

        public void Write(byte[] data, IO.WriteCallback callback)
        {
            Write(data, 0, data.Length, callback);
        }

        public void Write(byte[] data, int offset, int count, IO.WriteCallback callback)
        {
            var write_bytes = new SendBytesOperation(new ByteBuffer[] {new ByteBuffer(data, offset, count)}, callback);
            QueueWriteOperation(write_bytes);
        }

        Queue<IWriteOperation> write_ops = new Queue<IWriteOperation>();
        IWriteOperation current_write_op;

        public void QueueWriteOperation(IO.IWriteOperation op)
        {
            lock (write_ops)
            {
                if (write_ops.Count < 1 || !write_ops.Last().Combine(op))
                    write_ops.Enqueue(op);

                if (current_write_op == null)
                {
                    current_write_op = write_ops.Dequeue();
                    op.BeginWrite(this);

                    StartSending();
                }
            }
        }

        private bool sending;
        public int Send(Collections.ByteBuffer buffer, out int error)
        {
            SocketError er;
            lock (write_ops)
            {
                sending = true;
            }
            socket.BeginSend(buffer.buffer.Bytes, buffer.buffer.Position, buffer.buffer.Length, SocketFlags.None, out er, (ar) =>
            {
                StartTimeout();
                socket.EndSend(ar, out er);
                if (er != SocketError.Success)
                {
                    OnError();
                }
                if (ar.CompletedSynchronously)
                {
                    lock (write_ops)
                    {
                        sending = false;
                    }
                } else {
                    lock (write_ops)
                    {
                        loop.NonBlockInvoke(StartSending);
                    }
                }
            }, null);
            error = (int)er;

            return buffer.Length;
        }




        private void StartSending()
        {
            if (current_write_op == null) return;
            bool cont = true;
            while (cont)
            {
                if (disabledsending)
                {
                    sending = false;
                    return;
                }
                cont = false;
                if (current_write_op.IsComplete)
                {
                    current_write_op.EndWrite(this);

                    if (write_ops.Count > 0)
                    {
                        IWriteOperation op = write_ops.Dequeue();
                        sending = false;
                        op.BeginWrite(this);
                        current_write_op = op;
                        current_write_op.HandleWrite(this);
                        lock (this)
                        {
                            if (!sending)
                                cont = true;
                        }
                    }
                    else
                    {
                        current_write_op = null;
                    }
                }
                else
                {
                    current_write_op.HandleWrite(this);
                    lock (this)
                    {
                        if (!sending)
                            cont = true;
                    }
                }
            }
            
        }
        
        public event EventHandler Error;

        public event EventHandler Closed;

        public event EventHandler TimedOut;

        private void OnError()
        {
            if (Error != null)
                loop.NonBlockInvoke(delegate
                {
                    Error(this, EventArgs.Empty);
                });
        }

        
        byte[] receiveBuffer;
        public void ReadBytes(IO.ReadCallback callback)
        {
            if (receiveBuffer == null) receiveBuffer = new byte[4096];
            SocketError se;
            socket.BeginReceive(receiveBuffer, 0, receiveBuffer.Length, SocketFlags.None, out se, ReadCallback, callback);
            if (se != SocketError.Success)
                OnError();
        }

        private void ReadCallback(IAsyncResult ar)
        {
            StartTimeout();
            SocketError se;
            var callback = ((IO.ReadCallback)ar.AsyncState);
            int len = socket.EndReceive(ar, out se);
            
            if (se != SocketError.Success)
            {
                OnError();
            }
            else
            {
                loop.NonBlockInvoke(delegate
                {
                    if (len == 0)
                    {
                        if (Closed != null)
                            Closed(this, EventArgs.Empty);
                    }
                    else
                    {
                        callback(this, receiveBuffer, 0, len);
                        socket.BeginReceive(receiveBuffer, 0, receiveBuffer.Length, SocketFlags.None, out se, ReadCallback, callback);
                        if (se != SocketError.Success)
                            OnError();
                    }
                });
            }
            if (len == 0)
            {
                if (Closed != null)
                {
                    loop.NonBlockInvoke(delegate
                    {
                        Closed(this, EventArgs.Empty);
                    });
                }
            }
        }

        private bool disabledsending;

        public void EnableWriting()
        {
            if (disabledsending)
            {
                disabledsending = false;
                if (!sending)
                    loop.NonBlockInvoke(() => {
                        lock (write_ops)
                        {
                            StartSending();
                        }
                    });
            }
        }

        public void DisableWriting()
        {
            disabledsending = true;
        }


        public int SendFile (string name, bool chunked, long length, Action<long, int> callback)
        {
            throw new NotSupportedException();
        }

        public void Close()
        {
            if (socket.Connected)
            {
                socket.BeginDisconnect(true, (ar) =>
                {
                    try
                    {
                        socket.EndDisconnect(ar);
                        if (timer != null)
                        {
                            var t = timer;
                            timer = null;
                            t.Enabled = false;
                            t.Dispose();
                        }


                        if (Closed != null)
                        {
                            loop.NonBlockInvoke(delegate
                            {
                                Closed(this, EventArgs.Empty);
                            });
                        }
                    }
                    catch
                    {
                        OnError();

                    }
                }, null);
            }
        }

        public void Dispose()
        {
            socket.Dispose();
            var t = timer;
            if (t != null)
            {
                timer = null;
                t.Dispose();
            }
        }
    }
}
