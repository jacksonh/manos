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

        public SocketStream() { }
        public SocketStream(Socket sock)
        {
            socket = sock;
            StartTimeout();
            address = ((IPEndPoint)socket.RemoteEndPoint).Address.ToString();
            port = ((IPEndPoint)socket.RemoteEndPoint).Port;
                        
        }

        void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            
        }
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
                        if (Error != null)
                            Error(this, EventArgs.Empty);
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
                        if (Error != null)
                            Error(this, EventArgs.Empty);
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
                if (Error != null)
                    Error(this, EventArgs.Empty);
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
                            Connected(this);
                    }
                    catch
                    {
                        if (Error != null)
                            Error(this, EventArgs.Empty);
                    }
                }, null);
            }
            catch
            {
                if (Error != null)
                    Error(this, EventArgs.Empty);
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
                        TimedOut(this, EventArgs.Empty);
                    Close();
                };
            }
            if (timer.Enabled)
                timer.Stop();
            timer.Start();
            
        }

        private void AcceptCallback(IAsyncResult ar)
        {
            try
            {
                Socket sock = socket.EndAccept(ar);

                if (this.ConnectionAccepted != null)
                {
                    ConnectionAccepted(this, new IO.ConnectionAcceptedEventArgs(new SocketStream(sock)));
                }
                else
                {
                    sock.Dispose();
                }
                socket.BeginAccept(AcceptCallback, null);
            }
            catch
            {
                if (Error != null)
                    Error(this, EventArgs.Empty);
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
            var bytes = new List<ByteBuffer>();
            bytes.Add(new ByteBuffer(data, offset, count));

            var write_bytes = new SendBytesOperation(bytes, callback);
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

                    StartSending();
                }
            }
        }

        private bool sending;
        public int Send(Collections.ByteBufferS[] buffers, int length, out int error)
        {
            SocketError er;
            lock (write_ops)
            {
                sending = true;
            }
            socket.BeginSend(buffers.Select(a => new ArraySegment<byte>(a.Bytes, a.Position, a.Length)).ToArray(), SocketFlags.None, out er, (ar) =>
            {
                StartTimeout();
                socket.EndSend(ar, out er);
                if (er != SocketError.Success)
                {
                    if (Error != null) Error(this, EventArgs.Empty);
                }
                lock (write_ops)
                {
                    StartSending();
                }
            }, null);
            error = (int)er;

            return buffers.Sum(a => a.Length);
        }



        private void StartSending()
        {
            if (current_write_op == null) return;
            bool cont = true;
            while (cont)
            {
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
                        if (!sending)
                            cont = true;
                    }
                    else
                    {
                        current_write_op = null;
                    }
                }
                else
                {
                    current_write_op.HandleWrite(this);
                }
            }
            
        }
        
        public event EventHandler Error;

        public event EventHandler Closed;

        public event EventHandler TimedOut;

        
        byte[] receiveBuffer;
        public void ReadBytes(IO.ReadCallback callback)
        {
            if (receiveBuffer == null) receiveBuffer = new byte[4096];
            SocketError se;
            socket.BeginReceive(receiveBuffer, 0, receiveBuffer.Length, SocketFlags.None, out se, (ar) =>
            {
                StartTimeout();
                int len = socket.EndReceive(ar, out se);
                if (se != SocketError.Success)
                {
                    if (Error != null)
                        Error(this, EventArgs.Empty);
                }
                else
                {
                    callback(this, receiveBuffer, 0, len);
                }
            }, null);
            if (se != SocketError.Success)
                if (Error != null)
                    Error(this, EventArgs.Empty);
        }

        public void Close()
        {
            if (socket.Connected)
            {
                socket.BeginDisconnect(true, (ar) =>
                {
                    try
                    {
                        if (timer != null)
                        {
                            timer.Dispose();
                            timer = null;
                        }
                        socket.EndDisconnect(ar);
                        if (Closed != null)
                            Closed(this, EventArgs.Empty);
                    }
                    catch
                    {

                        if (Error != null)
                            Error(this, EventArgs.Empty);

                    }
                }, null);
            }
        }

        public void Dispose()
        {
            if (timer != null) timer.Dispose();
            socket.Dispose();
        }
    }
}
