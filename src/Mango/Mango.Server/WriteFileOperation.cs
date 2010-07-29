

using System;


namespace Mango.Server {

	public class WriteFileOperation : IWriteOperation {

		private string file;
		private WriteCallback callback;

		public WriteFileOperation (string file, WriteCallback callback)
		{
			this.file = file;
			this.callback = callback;
		}

		public string File {
			get { return file; }
			set {
				if (value == null)
					throw new ArgumentNullException ("value");
				file = value;
			}
		}

		public WriteCallback Callback {
			get { return callback; }
			set {
				if (value == null)
					throw new ArgumentNullException ("value");
				callback = value;
			}
		}
		
		public void Write (IOStream stream)
		{
			stream.SendFile (file, callback);
		}
	}
}

