			TcpListener listener = new TcpListener (8080);
			listener.Start (); 

			Socket socket = new Socket (AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

			socket.Bind (new IPEndPoint (IPAddress.Parse ("127.0.0.1"), 8081));
			socket.Listen (128);
