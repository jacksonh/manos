//
// Copyright (C) 2010 Jackson Harper (jackson@manosdemono.com)
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//
//

//

using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

namespace Manos.Http {

	public interface IUploadedFileCreator {

		UploadedFile Create (string name);
	}

	public class TempFileUploadedFileCreator : IUploadedFileCreator {

		public UploadedFile Create (string name)
		{
			string temp_file = Path.GetTempFileName ();
			return new TempFileUploadedFile (name, temp_file);
		}
	}

	public class InMemoryUploadedFileCreator : IUploadedFileCreator {

		public UploadedFile Create (string name)
		{
			return new InMemoryUploadedFile (name);
		}
	}
	
	public abstract class UploadedFile : IDisposable {

	  	 public UploadedFile (string name)
		 {
			Name = name;
		 }

		~UploadedFile ()
		{
			Dispose ();
		}

		public string Name {
		 	get;
			private set;
		 }

		public void Dispose ()
		{
			if (Contents != null)
				Contents.Close ();
		}

		public string ContentType {
		 	get;
			set;
		 }

		 public abstract long Length {
		 	get;
		 }

		 public abstract Stream Contents {
		 	get;
		 }

		public virtual void Finish ()
		{
		}
	  }

	  public class InMemoryUploadedFile : UploadedFile {

		  private MemoryStream stream = new MemoryStream ();

	  	 public InMemoryUploadedFile (string name) : base (name)
		 {
		 }
		 
		 public override long Length {
		 	get {
				return stream.Length;
			}
		 }

		 public override Stream Contents {
		 	get {
				return stream;
			}
		 }

		  public override void Finish ()
		  {
			  stream.Position = 0;
		  }
	  }

	  public class TempFileUploadedFile : UploadedFile {

		  FileStream stream;
		  
	  	 public TempFileUploadedFile (string name, string temp_file) : base (name)
		 {
			TempFile = temp_file;
		 }
	  	 
		 public string TempFile {
		 	get;
			private set;
		 }

		 public override long Length {
		 	get {
			    FileInfo f = new FileInfo (TempFile);
			    return f.Length;
			}
		 }

		 public override Stream Contents {
		 	get {
				if (stream == null)
					stream = File.OpenWrite (TempFile);
				return stream;
			}
		 }

		 public override void Finish ()
		 {
			 stream.Flush ();
			 stream.Close ();
			 stream = null;
		 }
	  }
}

